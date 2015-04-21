using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;

namespace Syzygy.Game.FreeObjects
{
    sealed class Projectile : FreeObject
    {
        private readonly Player owner;

        public Projectile(GameState game, Id<FreeObject> id, Player owner,
            Position2 position, Velocity2 velocity)
            : base(game, id, position, velocity)
        {
            this.owner = owner;
        }

        protected override void hitBody(IBody body)
        {
            // TODO: deal damage

            base.hitBody(body);
        }
    }
}
