
namespace Syzygy.Game.SyncedCommands
{
    interface IRequestHandler
    {
        void TryDo(IRequest request);
    }
}
