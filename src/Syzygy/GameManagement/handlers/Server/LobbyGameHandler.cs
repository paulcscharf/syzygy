using System;
using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Forms;
using Syzygy.Game;

namespace Syzygy.GameManagement.Server
{
    sealed class LobbyGameHandler : IGameHandler
    {
        private enum LobbyStatus
        {
            Waiting = 0,
            Starting = 1
        }

        private LobbyStatus status = LobbyStatus.Waiting;

        private readonly IdManager idMan = new IdManager();

        private readonly List<Player> players = new List<Player>();
        private readonly PlayerConnectionLookup connections = new PlayerConnectionLookup();
        private readonly Player me;
        private NetServer server;
        private LobbyForm form;

        public LobbyGameHandler(GameWindow gameWindow, string playerName)
        {
            this.players.Add(this.me = new Player(this.idMan.GetNext<Player>(), playerName));

            gameWindow.UIActionQueue.RunAndForget(this.makeForm);
        }

        private void makeForm()
        {
            var form  = new LobbyForm(true);

            form.AddPlayer(this.me.ID, this.me.Name);

            form.Show();

            form.Started += this.clickedStart;

            this.form = form;
        }

        private void clickedStart()
        {
            this.form.Close();
            this.status = LobbyStatus.Starting;
        }

        public event GenericEventHandler<IGameHandler> Stopped;

        public void Update(UpdateEventArgs e)
        {
            if (this.form == null)
                return;
            if (this.server == null)
            {
                this.startServer();
            }

            if (this.status != LobbyStatus.Waiting)
            {
                switch (status)
                {
                    case LobbyStatus.Starting:
                        this.startGame();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return;
            }

            NetIncomingMessage message;
            while ((message = this.server.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                    {
                        this.tryApproveClient(message);
                        break;
                    }
                    case NetIncomingMessageType.StatusChanged:
                    {
                        this.onClientStatusChanged(message);
                        break;
                    }
                    default:
                    {
                        Log.Line("unhandled message with type: " + message.MessageType);
                        if (message.MessageType == NetIncomingMessageType.DebugMessage)
                            Log.Debug(message.ReadString());
                        break;
                    }
                }
            }
        }

        private void startGame()
        {
            var connectionsToKill = this.connections
                .Where(c => c.Status != NetConnectionStatus.Connected)
                .ToList();
            foreach (var connection in connectionsToKill)
            {
                var id = this.connections[connection];
                connection.Disconnect("");
                this.players.RemoveAt(this.players.FindIndex(p => p.ID == id));
            }

            if (this.connections.Count > 0)
            {
                var startMessage = this.server.CreateMessage();
                startMessage.Write((byte)LobbyMessageType.StartGameBuilding);
                this.server.SendMessage(startMessage, this.connections, NetDeliveryMethod.ReliableOrdered, 0);
            }

            this.Stopped(new BuildGameHandler(this.server, new PlayerLookup(this.players), this.connections, this.me.ID));
        }

        private void startServer()
        {
            var config = new NetPeerConfiguration(Settings.Network.AppName)
            {
                Port = Settings.Network.DefaultPort
            };

            this.server = new NetServer(config);
            this.server.Configuration.SetMessageTypeEnabled(NetIncomingMessageType.ConnectionApproval, true);
            this.server.Start();
        }

        private void onClientStatusChanged(NetIncomingMessage message)
        {
            switch (message.SenderConnection.Status)
            {
                case NetConnectionStatus.Connected:
                {
                    this.onClientFullyConnected(message.SenderConnection);
                    break;
                }
                case NetConnectionStatus.Disconnected:
                {
                    Log.Warning("Player disconnected, we cannot handle that yet!");
                    Log.Warning("Prepare for unforeseen consequences.");
                    break;
                }
            }
        }

        private void onClientFullyConnected(NetConnection connection)
        {
            var newPlayerId = this.connections[connection];
            var newPlayer = this.players.First(p => p.ID == newPlayerId);

            var otherClientConnections = this.connections
                .Where(c => c != connection)
                .Where(c => c.Status == NetConnectionStatus.Connected)
                .ToList();

            // tell other clients about this player
            if (otherClientConnections.Count > 0)
            {
                var newPlayerMessage = this.server.CreateMessage();
                newPlayerMessage.Write((byte)LobbyMessageType.NewPlayer);
                newPlayerMessage.Write(newPlayer.ID.Simple);
                newPlayerMessage.Write(newPlayer.Name);

                this.server.SendMessage(newPlayerMessage, otherClientConnections, NetDeliveryMethod.ReliableOrdered, 0);
            }

            var allOthersMessage = this.server.CreateMessage();
            allOthersMessage.Write((byte)LobbyMessageType.NewPlayers);
            allOthersMessage.Write((byte)(otherClientConnections.Count + 1));
            foreach (var p in this.players.Where(p => p.ID != newPlayerId)
                .Where(p =>
                {
                    var c = this.connections[p.ID];
                    return c == null || c.Status == NetConnectionStatus.Connected;
                }))
            {
                // collect all players
                allOthersMessage.Write(p.ID.Simple);
                allOthersMessage.Write(p.Name);
            }
            // tell this client about other players
            connection.SendMessage(allOthersMessage, NetDeliveryMethod.ReliableOrdered, 0);

            // add client to visible player list
            this.form.Invoke(new Action(() =>
                this.form.AddPlayer(newPlayer.ID, newPlayer.Name)
                ));
        }

        private void tryApproveClient(NetIncomingMessage message)
        {
            string name;
            if(message.ReadString(out name))
            {
                var trimmed = name.Trim();
                if (trimmed != "" && trimmed == name)
                {
                    this.approveClient(message.SenderConnection, name);
                    return;
                }
            }
            message.SenderConnection.Deny();
        }
        private void approveClient(NetConnection connection, string name)
        {
            var p = new Player(this.idMan.GetNext<Player>(), name);
            this.players.Add(p);
            this.connections.Add(p.ID, connection);

            var message = this.server.CreateMessage(4);

            message.Write(p.ID.Simple);
            connection.Approve(message);
        }
    }
}
