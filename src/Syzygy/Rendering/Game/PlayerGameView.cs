using Bearded.Utilities.SpaceTime;
using OpenTK;
using Syzygy.Game;

namespace Syzygy.Rendering.Game
{
    sealed class PlayerGameView : GameObject, IGameView
    {
        public PlayerGameView(GameState game)
            : base(game)
        {
        }

        #region IGameView

        public ViewParameters ViewParameters
        {
            get
            {
                return new ViewParameters(new Vector3(0, 0, 20));
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
