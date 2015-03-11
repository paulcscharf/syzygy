using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;
using Syzygy.Rendering;

namespace Syzygy.Game
{
    sealed class Economy : GameObject
    {
        private readonly Player player;
        private readonly IBody body;

        public Economy(GameState game, Id<Player> player, Id<IBody> body)
            : base(game)
        {
            this.player = game.Players[player];
            this.body = game.Bodies[body];

            this.listAs<Economy>();
        }

        public Player Player { get { return this.player; } }

        public IBody Body { get { return this.body; } }

        public override void Update(TimeSpan t)
        {
        }

        public override void Draw(GeometryManager geos)
        {
        }
    }
}
