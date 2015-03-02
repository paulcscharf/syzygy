using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using amulware.Graphics;
using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Forms;

namespace Syzygy.GameManagement
{
    sealed class LobbyServerGameHandler : IGameHandler
    {

        private int lastPlayerId;

        private readonly GameWindow gameWindow;
        private readonly List<Player> players = new List<Player>();
        private NetServer server;
        private LobbyForm form;

        public LobbyServerGameHandler(GameWindow gameWindow, string playerName)
        {
            this.gameWindow = gameWindow;
            this.players.Add(new Player(this.newPlayerId(), playerName, null));

            gameWindow.UIActionQueue.RunAndForget(this.makeForm);
        }

        private void makeForm()
        {
            var form  = new LobbyForm(true);

            var me = this.players[0];
            form.AddPlayer(me.ID, me.Name);

            form.Show();

            this.form = form;
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
            var newPlayer = this.players.First(p => p.Connection == connection);

            var newPlayerMessage = this.server.CreateMessage();
            newPlayerMessage.Write((byte)LobbyMessageType.NewPlayer);
            newPlayerMessage.Write((byte)newPlayer.ID);
            newPlayerMessage.Write(newPlayer.Name);
            
            var otherClients = this.players
                .Where(p => p.Connection != null && p.Connection != connection)
                .Where(p => p.Connection.Status == NetConnectionStatus.Connected)
                .Select(p => p.Connection).ToList();

            // tell other clients about this player
            if(otherClients.Count > 0)
                this.server.SendMessage(newPlayerMessage, otherClients, NetDeliveryMethod.ReliableOrdered, 0);

            var allOthersMessage = this.server.CreateMessage();
            allOthersMessage.Write((byte)LobbyMessageType.NewPlayers);
            allOthersMessage.Write((byte)(otherClients.Count + 1));
            foreach (var p in this.players.Where(p => p.Connection != connection))
            {
                // collect all players
                allOthersMessage.Write((byte)p.ID);
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

        private int newPlayerId()
        {
            return ++this.lastPlayerId;
        }

        private void approveClient(NetConnection connection, string name)
        {
            var p = new Player(this.newPlayerId(), name, connection);
            this.players.Add(p);

            var message = this.server.CreateMessage(4);

            message.Write((byte)p.ID);
            connection.Approve(message);
        }
    }
}
