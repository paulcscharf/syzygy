using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;
using Syzygy.Game.SyncedCommands;
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

        private readonly IdManager idManager = new IdManager();

        private readonly IRequestHandler requestHandler;
        #endregion

        #region Lists and Dictionaries

        // more info on this collection at http://genericgamedev.com/general/on-collections
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();

        private readonly PlayerLookup players;
        private readonly DeletableObjectDictionary<IBody> bodies = new DeletableObjectDictionary<IBody>();
        private readonly DeletableObjectDictionary<FreeObject> freeObjects = new DeletableObjectDictionary<FreeObject>();

        private readonly DeletableObjectList<Economy> economies = new DeletableObjectList<Economy>();

        #endregion

        #region State

        private Instant time = Instant.Zero;

        #endregion

        #endregion

        #region Properties

        public PlayerLookup Players { get { return this.players; } }
        public DeletableObjectDictionary<IBody> Bodies { get { return this.bodies; } }

        public DeletableObjectList<Economy> Economies { get { return this.economies; } }

        public IRequestHandler RequestHandler { get { return this.requestHandler; } }

        #endregion

        #region Constructor

        public GameState(PlayerLookup players, IRequestHandler requestHandler)
        {
            this.players = players;
            this.requestHandler = requestHandler;

            this.deletableDictionaries = new Dictionary<Type, object>{
                { typeof (IBody), this.bodies },
                { typeof (FreeObject), this.freeObjects },
            };
            this.deletableLists = new Dictionary<Type, object>{
                { typeof (Economy), this.economies },
            };


            for (int i = 0; i < 100; i++)
            {
                new FreeObject(this, this.GetUniqueId<FreeObject>(), new Position2(i * 0.1f, 0), new Velocity2());
            }
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

        public Id<T> GetUniqueId<T>()
        {
            return this.idManager.GetNext<T>();
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
