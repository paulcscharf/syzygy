using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;
using Syzygy.Rendering;

namespace Syzygy.Game
{
    abstract class GameObject : IDeletable
    {
        private readonly GameState _game;

        protected GameState game { get { return this._game; } }

        public GameObject(GameState game)
        {
            this._game = game;
            game.Add(this);
        }

        public bool Deleted { get; private set; }

        public abstract void Update(TimeSpan t);

        public abstract void Draw(GeometryManager geos);

        protected void delete()
        {
            this.Deleted = true;
        }
    }
}
