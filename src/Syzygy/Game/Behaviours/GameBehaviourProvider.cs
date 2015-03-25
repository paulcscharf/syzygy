
namespace Syzygy.Game.Behaviours
{
    interface IGameBehaviourProvider
    {
        ICollisionHandler GetCollisionHandler(GameState game);
        IRequestHandler GetRequestHandler();
        IContinuousSynchronizer GetContinuousSynchronizer(GameState gameState);
    }
}
