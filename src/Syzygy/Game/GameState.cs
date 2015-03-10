using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game
{
    sealed class GameState
    {
        private Instant time = Instant.Zero;

        // more info on this collection at http://genericgamedev.com/general/on-collections
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();

        private readonly DeletableObjectDictionary<IBody> bodies = new DeletableObjectDictionary<IBody>();
        public DeletableObjectDictionary<IBody> Bodies { get { return this.bodies; } }

        private readonly DeletableObjectList<FreeObject> freeObjects = new DeletableObjectList<FreeObject>();

        private readonly Dictionary<Type, object> deletableDictionaries;
        private readonly Dictionary<Type, object> deletableLists;

        public GameState()
        {
            this.deletableDictionaries = new Dictionary<Type, object>{
                { typeof (IBody), this.bodies }
            };
            this.deletableLists = new Dictionary<Type, object>{
                { typeof(FreeObject), this.freeObjects }
            };
        }

        public DeletableObjectDictionary<TId> IdDictionary<TId>()
            where TId : class, IIdable<TId>, IDeletable
        {
            return (DeletableObjectDictionary<TId>)this.deletableDictionaries[typeof (TId)];
        }

        public DeletableObjectList<T> List<T>()
            where T : class, Bearded.Utilities.Collections.IDeletable
        {
            return (DeletableObjectList<T>)this.deletableLists[typeof (T)];
        }

        public void Update(UpdateEventArgs e)
        {
            var elapsed = new TimeSpan(e.ElapsedTimeInS);

            this.update(elapsed);
        }

        private void update(TimeSpan t)
        {
            this.time += t;

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Update(t);
            }
        }

        public void Draw()
        {
            var geos = GeometryManager.Instance;

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Draw(geos);
            }
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }
    }
}
