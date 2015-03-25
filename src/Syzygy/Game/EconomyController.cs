using System.Linq;
using amulware.Graphics;
using Bearded.Utilities.Input;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using OpenTK.Input;
using Syzygy.Game.Astronomy;
using Syzygy.Game.SyncedCommands;
using Syzygy.Rendering;

namespace Syzygy.Game
{
    sealed class EconomyController : GameObject, IPlayerController
    {
        private readonly Player player;
        private readonly IBody body;

        public Id<Player> PlayerId { get { return this.player.ID; } }

        public EconomyController(GameState game, Id<Player> player)
            : base(game)
        {
            this.player = game.Players[player];
            this.body = game.Economies.First(e => e.Player == this.player).Body;
        }

        public override void Update(TimeSpan t)
        {

            if (InputManager.IsKeyHit(Key.Space))
            {
                var request = ShootDebugParticleFromPlanet.Request(this.game, this, this.body, Direction2.Zero);
                this.game.RequestHandler.TryDo(request);
            }

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
