using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Forms;

namespace Syzygy.GameManagement.Client
{
    sealed class LobbyGameHandler : GenericGameHandler<NetClient>
    {
        private readonly GameWindow gameWindow;
        private readonly string playerName;
        private readonly int playerId;
        private readonly List<Player> players = new List<Player>();
        private LobbyForm form;
        private bool addedSelf;

        public LobbyGameHandler(GameWindow gameWindow, NetClient peer, string playerName)
            : base(peer)
        {
            this.gameWindow = gameWindow;
            this.playerName = playerName;
            this.playerId = this.peer.ServerConnection.RemoteHailMessage.ReadByte();

            gameWindow.UIActionQueue.RunAndForget(this.makeForm);
        }

        private void makeForm()
        {
            var form = new LobbyForm(false);

            form.Show();

            this.form = form;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (this.form == null)
                return;
            if (!this.addedSelf)
            {
                this.addPlayer(this.playerId, this.playerName);
                this.addedSelf = true;
            }

            base.Update(e);
        }

        protected override void onDataMessage(NetIncomingMessage message)
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
