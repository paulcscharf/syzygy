using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Syzygy.Game;

namespace Syzygy.GameManagement
{
    sealed class PlayerLookup : IEnumerable<Player>
    {
        private readonly ReadOnlyCollection<Player> players;
        private readonly Dictionary<Id<Player>, Player> byId;

        public int Count { get { return this.players.Count; } }

        public PlayerLookup(IEnumerable<Player> players)
        {
            this.players = players.ToList().AsReadOnly();
            this.byId = new Dictionary<Id<Player>, Player>(this.Count);
            foreach (var p in this.players)
                byId.Add(p.ID, p);
        }

        public Player this[Id<Player> id]
        {
            get
            {
                Player p;
                this.byId.TryGetValue(id, out p);
                return p;
            }
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return this.players.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
