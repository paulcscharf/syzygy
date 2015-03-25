using Lidgren.Network;

namespace Syzygy.Game.SyncedCommands
{
    internal abstract class BaseCommand<T> : ICommand
        where T : struct
    {
        private readonly CommandType type;

        protected BaseCommand(CommandType type)
        {
            this.type = type;
        }

        public abstract void Execute();

        public void WriteToBuffer(NetBuffer buffer)
        {
            buffer.Write((byte)this.type);
            buffer.Write(this.getParameters());
        }

        protected abstract T getParameters();
    }
}
