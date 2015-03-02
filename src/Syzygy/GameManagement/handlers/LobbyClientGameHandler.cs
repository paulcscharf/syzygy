using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Forms;

namespace Syzygy.GameManagement
{
    sealed class LobbyClientGameHandler : IGameHandler
    {
        private readonly GameWindow gameWindow;
        private readonly NetClient client;
        private readonly string playerName;
        private readonly int playerId;
        private readonly List<Player> players = new List<Player>();
        private LobbyForm form;
        private bool addedSelf;

        public LobbyClientGameHandler(GameWindow gameWindow, NetClient client, string playerName)
        {
            this.gameWindow = gameWindow;
            this.client = client;
            this.playerName = playerName;
            this.playerId = this.client.ServerConnection.RemoteHailMessage.ReadByte();

            gameWindow.UIActionQueue.RunAndForget(this.makeForm);
        }

        private void makeForm()
        {
            var form = new LobbyForm(false);

            form.Show();

            this.form = form;
        }

        public event GenericEventHandler<IGameHandler> Stopped;

        public void Update(UpdateEventArgs e)
        {
            if (this.form == null)
                return;
            if (!this.addedSelf)
            {
                this.addPlayer(this.playerId, this.playerName);
                this.addedSelf = true;
            }

            NetIncomingMessage message;
            while ((message = this.client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                    {
                        this.handleData(message);
                        break;
                    }
                    case NetIncomingMessageType.StatusChanged:
                    {
                        Log.Warning("status changed to :" + this.client.ConnectionStatus);
                        Log.Warning("We have no idea yet how to handle that yet.");
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
                Log.Line("");
            }
        }

        private void handleData(NetIncomingMessage message)
        {
            var type = (LobbyMessageType)message.ReadByte();
            switch (type)
            {
                case LobbyMessageType.NewPlayer:
                {
                    this.addPlayer(message.ReadByte(), message.ReadString());
                    break;
                }
                case LobbyMessageType.NewPlayers:
                {
                    var count = message.ReadByte();
                    for (int i = 0; i < count; i++)
                    {
                        this.addPlayer(message.ReadByte(), message.ReadString());
                    }
                    break;
                }
                default:
                {
                    Log.Warning("Received message with unknown type. (value = {0})", type);
                    break;
                }
            }
        }

        private void addPlayer(int id, string name)
        {
            this.players.Add(new Player(id, name, null));
            this.form.Invoke(new Action(() =>
                this.form.AddPlayer(id, name)
                ));
        }
    }
}
