using System.Linq;
using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;
using Syzygy.Rendering;

namespace Syzygy.Game
{
    sealed class EconomyController : GameObject
    {
        private readonly Player player;
        private readonly IBody body;

        public EconomyController(GameState game, Id<Player> player)
            : base(game)
        {
            this.player = game.Players[player];
            this.body = game.Economies.First(e => e.Player == this.player).Body;
        }

        public override void Update(TimeSpan t)
        {



        }

        public override void Draw(GeometryManager geos)
        {

            var geo = geos.Primitives;

            geo.Color = Color.Lime * 0.25f;
            geo.LineWidth = 0.1f;

            var shape = this.body.Shape;

            geo.DrawCircle(shape.Center.Vector, shape.Radius.NumericValue + 0.2f, false);

        }
    }
}
