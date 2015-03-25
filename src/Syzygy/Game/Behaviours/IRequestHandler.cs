
using Syzygy.Game.SyncedCommands;

namespace Syzygy.Game.Behaviours
{
    interface IRequestHandler
    {
        void TryDo(IRequest request);
    }
}
