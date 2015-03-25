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

        private IAction rotateClockwiseAction;
        private IAction rotateCounterClockwiseAction;
        private IAction shootAction;

        private Direction2 aimDirection;

        public EconomyController(GameState game, Id<Player> player)
            : base(game)
        {
            this.player = game.Players[player];
            this.body = game.Economies.First(e => e.Player == this.player).Body;

            this.rotateClockwiseAction = KeyboardAction.FromKey(Key.Right);
            this.rotateCounterClockwiseAction = KeyboardAction.FromKey(Key.Left);
            this.shootAction = KeyboardAction.FromKey(Key.Space);
        }

        public override void Update(TimeSpan t)
        {
            var rotation = this.rotateCounterClockwiseAction.AnalogAmount - this.rotateClockwiseAction.AnalogAmount;

            this.aimDirection += 2f.Radians() * rotation * (float)t.NumericValue;
    
            if (this.shootAction.Hit)
            {
                var request = ShootDebugParticleFromPlanet.Request(this.game, this, this.body, this.aimDirection);
                this.game.RequestHandler.TryDo(request);
            }
        }

        public override void Draw(GeometryManager geos)
        {

            var geo = geos.Primitives;

            geo.Color = Color.Lime * 0.25f;
            geo.LineWidth = 0.1f;

            var shape = this.body.Shape;
            var center = shape.Center.Vector;

            var r = shape.Radius.NumericValue + 0.2f;

            geo.DrawCircle(center, r, false);

            var dVector = this.aimDirection.Vector;

            geo.LineWidth = 0.05f;
            geo.DrawLine(center + dVector * r, center + dVector * (r + 0.25f));

        }

    }
}
