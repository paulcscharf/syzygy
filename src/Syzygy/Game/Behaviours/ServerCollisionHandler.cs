using Syzygy.Game.Astronomy;
using Syzygy.Game.Behaviours;

namespace Syzygy.Game
{
    sealed class ServerCollisionHandler : ICollisionHandler
    {
        private readonly GameState game;

        public ServerCollisionHandler(GameState game)
        {
            this.game = game;
        }

        public void HandleCollision(FreeObject obj)
        {
            var position = obj.Position;

            foreach (var body in this.game.Bodies)
            {
                var shape = body.Shape;

                var difference = shape.Center - position;

                var distanceSquared = difference.LengthSquared;

                if (distanceSquared < shape.Radius.Squared)
                {
                    obj.HitBody(body);
                    if (obj.Deleted)
                        return;
                }
            }
        }
    }
}
