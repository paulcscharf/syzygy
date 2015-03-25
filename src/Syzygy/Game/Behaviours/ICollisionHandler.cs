using Syzygy.Game.Astronomy;

namespace Syzygy.Game.Behaviours
{
    interface ICollisionHandler
    {
        void HandleCollision(FreeObject obj);
    }
}
