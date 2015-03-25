using Lidgren.Network;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    abstract class BaseRequest<T> : IRequest
        where T : struct 
    {
        private readonly Id<Player> requester;
        private readonly RequestType type;

        protected BaseRequest(RequestType type, PlayerController controller)
        {
            this.requester = controller.PlayerId;
            this.type = type;
        }

        protected BaseRequest(RequestType type, PlayerConnectionLookup connectionLookup, NetConnection connection)
        {
            this.requester = connectionLookup[connection];
        }

        public Id<Player> Requester { get { return this.requester; } }

        public abstract bool CheckPreconditions();

        public void WriteToBuffer(NetBuffer buffer)
        {
            buffer.Write((byte)this.type);
            buffer.Write(this.getParameters());
        }

        protected abstract T getParameters();

        public abstract ICommand MakeCommand();
    }

    class PlayerController
    {
        private readonly Id<Player> playerId;

        public PlayerController(Id<Player> playerId)
        {
            this.playerId = playerId;
        }

        public Id<Player> PlayerId { get { return this.playerId; } }
    }
}
