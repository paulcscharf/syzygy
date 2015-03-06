using System.Collections;
using System.Collections.Generic;
using Lidgren.Network;

namespace Syzygy.GameManagement
{
    sealed class PlayerList : IEnumerable<Player>
    {
        private readonly List<Player> players = new List<Player>();
        private readonly Dictionary<NetConnection, Player> byConnection;
        private readonly Dictionary<Id<Player>, Player> byId = new Dictionary<Id<Player>, Player>();

        public int Count { get { return this.players.Count; } }

        public PlayerList(bool isServer)
        {
            if(isServer)
                this.byConnection = new Dictionary<NetConnection, Player>();
        }

        public void Add(Player player)
        {
            this.byId.Add(player.ID, player);
            if (this.byConnection != null && player.Connection != null)
                this.byConnection.Add(player.Connection, player);
            this.players.Add(player);
        }

        public void Remove(Player player)
        {
            this.byId.Remove(player.ID);
            if (this.byConnection != null)
                this.byConnection.Remove(player.Connection);
            this.players.Remove(player);
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
        public Player this[NetConnection connection]
        {
            get
            {
                Player p;
                this.byConnection.TryGetValue(connection, out p);
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
