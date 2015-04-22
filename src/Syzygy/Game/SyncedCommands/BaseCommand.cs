using Lidgren.Network;

namespace Syzygy.Game.SyncedCommands
{
    internal abstract class BaseCommand<T> : ICommand
        where T : struct
    {
        private readonly CommandType type;
        protected readonly GameState game;

        protected BaseCommand(CommandType type, GameState game)
        {
            this.type = type;
            this.game = game;
        }

        public virtual bool IsServerOnlyCommand { get { return false; } }
        public abstract void Execute();

        public void WriteToBuffer(NetBuffer buffer)
        {
            buffer.Write((byte)this.type);
            buffer.Write(this.parameters);
        }

        protected abstract T parameters { get; }
    }
}
