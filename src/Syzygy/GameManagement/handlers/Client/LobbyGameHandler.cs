using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Forms;
using Syzygy.Game;

namespace Syzygy.GameManagement.Client
{
    sealed class LobbyGameHandler : GenericGameHandler<NetClient>
    {
        private readonly GameWindow gameWindow;
        private readonly Player me;
        private readonly List<Player> players = new List<Player>();
        private LobbyForm form;
        private bool addedSelf;

        public LobbyGameHandler(GameWindow gameWindow, NetClient peer, string playerName)
            : base(peer)
        {
            this.gameWindow = gameWindow;

            var myId = this.peer.ServerConnection.RemoteHailMessage.Read<Id>().Generic<Player>();

            this.me = new Player(myId, playerName);

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
                this.addPlayer(this.me.ID, this.me.Name);
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
                    this.addPlayer(message.Read<Id>().Generic<Player>(), message.ReadString());
                    break;
                }
                case LobbyMessageType.NewPlayers:
                {
                    var count = message.ReadByte();
                    for (int i = 0; i < count; i++)
                    {
                        this.addPlayer(message.Read<Id>().Generic<Player>(), message.ReadString());
                    }
                    break;
                }
                case LobbyMessageType.StartGameBuilding:
                {
                    this.startGameBuilding();
                    break;
                }
                default:
                {
                    Log.Warning("Received message with unknown type. (value = {0})", type);
                    break;
                }
            }
        }

        private void startGameBuilding()
        {
            this.gameWindow.UIActionQueue.RunAndForget(() => this.form.Close());
            this.stop(new BuildGameHandler(this.peer, new PlayerLookup(this.players)));
        }

        private void addPlayer(Id<Player> id, string name)
        {
            this.players.Add(new Player(id, name));
            this.form.Invoke(new Action(() =>
                this.form.AddPlayer(id, name)
                ));
        }
    }
}
