using Lidgren.Network;

namespace Syzygy.Game.SyncedCommands
{
    static class Command
    {
        public static IRequest FromBuffer(NetIncomingMessage message)
        {
            // switch case, or later: dictionary build with reflection
            return null;
        }
    }
}