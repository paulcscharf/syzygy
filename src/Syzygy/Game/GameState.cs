using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;
using Syzygy.GameManagement;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game
{
    sealed class GameState
    {
        #region Fields

        #region Bookkeeping

        private readonly Dictionary<Type, object> deletableDictionaries;
        private readonly Dictionary<Type, object> deletableLists;

        #endregion

        #region Lists and Dictionaries

        // more info on this collection at http://genericgamedev.com/general/on-collections
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();

        private readonly PlayerLookup players;
        private readonly DeletableObjectDictionary<IBody> bodies = new DeletableObjectDictionary<IBody>();
        private readonly DeletableObjectDictionary<FreeObject> freeObjects = new DeletableObjectDictionary<FreeObject>();

        #endregion

        #region State

        private Instant time = Instant.Zero;

        #endregion

        #endregion

        #region Properties

        public PlayerLookup Players { get { return this.players; } }
        public DeletableObjectDictionary<IBody> Bodies { get { return this.bodies; } }

        #endregion

        #region Constructor

        public GameState(PlayerLookup players)
        {
            this.players = players;


            this.deletableDictionaries = new Dictionary<Type, object>{
                { typeof (IBody), this.bodies },
                { typeof(FreeObject), this.freeObjects },
            };
            this.deletableLists = new Dictionary<Type, object>{
                // no lists? :(
            };
        }

        #endregion

        #region Methods

        #region Bookkeeping

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
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

        #endregion

        #region Update

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

        #endregion

        #region Draw

        public void Draw()
        {
            var geos = GeometryManager.Instance;

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Draw(geos);
            }
        }

        #endregion

        #endregion
    }
}
