using System.Linq;
using amulware.Graphics;
using Bearded.Utilities.Input;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;
using Syzygy.Game.SyncedCommands;
using Syzygy.Rendering;
using Syzygy.Rendering.Game;

namespace Syzygy.Game
{
    sealed class EconomyController : GameObject, IPlayerController
    {
        private readonly PlayerGameView view;
        private readonly Player player;
        private readonly IBody body;
        private readonly Economy economy;

        public Id<Player> PlayerId { get { return this.player.ID; } }

        private readonly PlayerControls controls;

        private Direction2 aimDirection;

        public EconomyController(GameState game, Id<Player> player, PlayerGameView view)
            : base(game)
        {
            this.view = view;
            this.player = game.Players[player];
            this.economy = game.Economies.First(e => e.Player == this.player);
            this.body = this.economy.Body;

            view.FocusOnBody(this.body);

            this.controls = new PlayerControls();
        }

        public override void Update(TimeSpan t)
        {
            var rotation = this.controls.RotateCounterClockwise.AnalogAmount
                - this.controls.RotateClockwise.AnalogAmount;

            this.aimDirection += 2f.Radians() * rotation * (float)t.NumericValue;
    
            if (this.controls.Shoot.Hit)
            {
                var request = ShootDebugParticleFromPlanet.Request(this.game, this, this.body, this.aimDirection);
                this.game.RequestHandler.TryDo(request);
            }

            this.updateZoom(t);
        }

        private void updateZoom(TimeSpan t)
        {
            float zoomDelta = InputManager.DeltaScroll;
            zoomDelta += (float)t.NumericValue * 15
                * (this.controls.ZoomOut.AnalogAmount - this.controls.ZoomIn.AnalogAmount)
                + (this.controls.ZoomOut.Hit ? 1 : 0)
                + (this.controls.ZoomIn.Hit ? -1 : 0);

            if (zoomDelta != 0)
            {
                var zoomFactor = GameMath.Pow(1 / 1.4f, zoomDelta);
                this.view.Zoom *= zoomFactor;
            }
        }

        public override void Draw(GeometryManager geos)
        {

            var geo = geos.Primitives;

            geo.Color = Color.Lime * 0.5f;
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
