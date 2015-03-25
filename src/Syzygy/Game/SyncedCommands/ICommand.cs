using Lidgren.Network;

namespace Syzygy.Game.SyncedCommands
{
    interface ICommand
    {
        // ctor From(parameters)

        // ctor From(Request) or From(parameters)
        // ctor From(Buffer)

        void Execute();
        void WriteToBuffer(NetBuffer buffer);
    }
}
