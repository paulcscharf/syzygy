using Syzygy.Game.SyncedCommands;

namespace Syzygy.Game.Behaviours
{
    interface IGameBehaviourProvider
    {
        ICollisionHandler GetCollisionHandler(GameState game);
        IRequestHandler GetRequestHandler();
    }
}
