using Lidgren.Network;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    abstract class BaseRequest<T> : IRequest
        where T : struct
    {
        private readonly RequestType type;
        protected readonly GameState game;
        private readonly Player requester;

        protected BaseRequest(RequestType type, GameState game, IPlayerController controller)
        {
            this.type = type;
            this.game = game;
            this.requester = controller.Player;
        }

        protected BaseRequest(RequestType type, GameState game, PlayerConnectionLookup connectionLookup, NetConnection connection)
        {
            this.type = type;
            this.game = game;
            this.requester = game.Players[connectionLookup[connection]];
        }

        public Player Requester { get { return this.requester; } }

        public abstract bool CheckPreconditions();

        public void WriteToBuffer(NetBuffer buffer)
        {
            buffer.Write((byte)this.type);
            buffer.Write(this.parameters);
        }

        protected abstract T parameters { get; }

        public abstract ICommand MakeCommand();
    }
}
