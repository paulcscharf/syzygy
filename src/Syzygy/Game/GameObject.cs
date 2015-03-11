using System;
using Bearded.Utilities;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game
{
    abstract class GameObject<TId> : GameObject, IIdable<TId>
        where TId : class, IIdable<TId>, IDeletable
    {
        private readonly Id<TId> id;

        protected GameObject(GameState game, Id<TId> id)
            : base(game)
        {
            this.id = id;
            this.idAs<TId>();
        }

        public Id<TId> Id { get { return this.id; } }
    }

    abstract class GameObject : IDeletable
    {
        private readonly GameState _game;

        protected GameState game { get { return this._game; } }

        public event VoidEventHandler Deleting;

        public bool Deleted { get; private set; }

        public GameObject(GameState game)
        {
            this._game = game;
            game.Add(this);
        }

        protected void idAs<TId>()
            where TId : class, IIdable<TId>, IDeletable
        {
            var asTId = this as TId;
            if (asTId == null)
                throw new Exception("This instance must inherit from the given id-able interface.");

            this.game.IdDictionary<TId>().Add(asTId);
        }
        protected void listAs<T>()
            where T : class, Bearded.Utilities.Collections.IDeletable
        {

            var asT = this as T;
            if (asT == null)
                throw new Exception("This instance must inherit from the given deletable interface.");

            this.game.List<T>().Add(asT);
        }

        public abstract void Update(TimeSpan t);

        public abstract void Draw(GeometryManager geos);

        protected void delete()
        {
            if (this.Deleting != null)
                this.Deleting();
            this.Deleted = true;
        }
    }
}
