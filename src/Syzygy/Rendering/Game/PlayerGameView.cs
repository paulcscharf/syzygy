using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game;
using Syzygy.Game.Astronomy;

namespace Syzygy.Rendering.Game
{
    sealed class PlayerGameView : GameObject, IGameView
    {
        private IBody body;

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

                return new ViewParameters(p.Vector.WithZ(20));
            }
        }

        public void DrawGame()
        {
            this.game.Draw();
        }

        #endregion

        public override void Update(TimeSpan t)
        {
        }

        public override void Draw(GeometryManager geos)
        {
        }

    }
}
