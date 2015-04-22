using Lidgren.Network;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    abstract class UnifiedRequestCommand<T> : IRequest, ICommand
        where T : struct
    {
        private readonly Player requester;
        private readonly RequestType requestType;
        private readonly CommandType commandType;
        protected readonly GameState game;

        #region Request
        protected UnifiedRequestCommand(RequestType requestType, CommandType commandType,
            IPlayerController controller, GameState game)
        {
            this.requestType = requestType;
            this.commandType = commandType;
            this.game = game;
            this.requester = controller.Player;
        }

        protected UnifiedRequestCommand(RequestType requestType, CommandType commandType,
            PlayerConnectionLookup connectionLookup, NetConnection connection, GameState game)
        {
            this.requestType = requestType;
            this.commandType = commandType;
            this.game = game;
            this.requester = game.Players[connectionLookup[connection]];
        }
        public virtual bool IsClientOnlyRequest { get { return false; } }
        public Player Requester { get { return this.requester; } }

        public abstract bool CheckPreconditions();

        public ICommand MakeCommand()
        {
            return this;
        }

        void IRequest.WriteToBuffer(NetBuffer buffer)
        {
            buffer.Write((byte)this.requestType);
            buffer.Write(this.parameters);
        }

        #endregion

        #region Command

        protected UnifiedRequestCommand(CommandType commandType, GameState game)
        {
            this.commandType = commandType;
        }


        public virtual bool IsServerOnlyCommand { get { return false; } }
        public abstract void Execute();

        void ICommand.WriteToBuffer(NetBuffer buffer)
        {
            buffer.Write((byte)this.commandType);
            buffer.Write(this.parameters);
        }

        #endregion

        protected abstract T parameters { get; }

    }
}
