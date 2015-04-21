using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game;
using Syzygy.Game.Astronomy;

namespace Syzygy.Rendering.Game
{
    sealed class PlayerGameView : GameObject, IGameView
    {
        private IBody body;
        private float zoom = 2;
        private float zoomGoal = 2;

        public float Zoom
        {
            get { return this.zoomGoal; }
            set { this.zoomGoal = value.Clamped(1, 20); }
        }

        public PlayerGameView(GameState game)
            : base(game)
        {
        }

        public void FocusOnBody(IBody body)
        {
            this.body = body;
        }

        #region IGameView

        public ViewParameters ViewParameters
        {
            get
            {
                var p = this.body.Shape.Center;

                return new ViewParameters(p.Vector.WithZ(5 * this.zoom));
            }
        }

        public void DrawGame()
        {
            this.game.Draw();
        }

        #endregion

        public override void Update(TimeSpan t)
        {
            this.zoom += (this.zoomGoal - this.zoom) * ((float)t.NumericValue * 10);
        }

        public override void Draw(GeometryManager geos)
        {
        }

    }
}
