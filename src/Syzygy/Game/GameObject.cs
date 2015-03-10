using System;
using Bearded.Utilities;
using Bearded.Utilities.Collections;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game
{
    abstract class GameObject<TId> : GameObject, IDeletable<TId>
        where TId : class, IDeletable<TId>
    {
        private readonly Id<TId> id;

        protected GameObject(GameState game, Id<TId> id)
            : base(game)
        {
            this.id = id;
            this.listAs<TId>();
        }

        public Id<TId> Id { get { return this.id; } }

        public event VoidEventHandler Deleting;

        new protected void delete()
        {
            if(this.Deleting != null)
                this.Deleting();
            base.delete();
        }
    }

    abstract class GameObject : IDeletable
    {
        private readonly GameState _game;

        protected GameState game { get { return this._game; } }

        public GameObject(GameState game)
        {
            this._game = game;
            game.Add(this);
        }

        protected void listAs<TId>()
            where TId : class, IDeletable<TId>
        {
            var asTOtherId = this as TId;
            if (asTOtherId == null)
                throw new Exception("This instance must inherit from the given id type.");

            this.game.List<TId>().Add(asTOtherId);
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
