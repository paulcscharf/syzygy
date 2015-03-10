using System;
using System.Collections;
using System.Collections.Generic;
using Bearded.Utilities;
using Bearded.Utilities.Collections;

namespace Syzygy
{
    interface IIdable<TId>
        where TId : IIdable<TId>
    {
        Id<TId> Id { get; }
    }

    interface IDeletable : Bearded.Utilities.Collections.IDeletable
    {
        event VoidEventHandler Deleting;
    }

    sealed class DeletableObjectDictionary<TId> : IEnumerable<TId>
        where TId : class, IIdable<TId>, IDeletable
    {
        private readonly DeletableObjectList<TId> list = new DeletableObjectList<TId>();
        private readonly Dictionary<Id<TId>, TId> dictionary = new Dictionary<Id<TId>, TId>();

        public int Count { get { return this.dictionary.Count; } }

        public void Add(TId obj)
        {
            if(obj == null)
                throw new ArgumentNullException("obj");

            this.list.Add(obj);
            this.dictionary.Add(obj.Id, obj);
            obj.Deleting += () => this.dictionary.Remove(obj.Id);
        }

        public TId this[Id<TId> id]
        {
            get { return this.dictionary[id]; }
        }

        public IEnumerator<TId> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
