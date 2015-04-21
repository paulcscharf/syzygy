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
        protected readonly GameState game;

        public event VoidEventHandler Deleting;

        public bool Deleted { get; private set; }

        protected GameObject(GameState game)
        {
            this.game = game;
            game.Add(this);
        }

        protected void idAs<TId>()
            where TId : class, IIdable<TId>, IDeletable
        {
#if DEBUG
            var asTId = this as TId;
            if (asTId == null)
                throw new Exception("This instance must inherit from the given id-able interface.");

            this.game.IdDictionary<TId>().Add(asTId);
#else
            this.game.IdDictionary<TId>().Add((TId)this);
#endif
        }

        protected void listAs<T>()
            where T : class, Bearded.Utilities.Collections.IDeletable
        {
#if DEBUG
            var asT = this as T;
            if (asT == null)
                throw new Exception("This instance must inherit from the given deletable interface.");

            this.game.List<T>().Add(asT);
#else
            this.game.List<T>().Add((TId)this);
#endif
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
