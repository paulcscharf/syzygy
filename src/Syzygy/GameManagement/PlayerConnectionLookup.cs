using System.Collections;
using System.Collections.Generic;
using Lidgren.Network;
using Syzygy.Game;

namespace Syzygy.GameManagement
{
    sealed class PlayerConnectionLookup : IReadOnlyList<NetConnection>
    {
        private readonly Dictionary<Id<Player>, NetConnection> idToConnection = new Dictionary<Id<Player>, NetConnection>();
        private readonly Dictionary<NetConnection, Id<Player>> connectionToId = new Dictionary<NetConnection, Id<Player>>();

        private readonly List<NetConnection> allConnections = new List<NetConnection>();

        public void Add(Id<Player> player, NetConnection connection)
        {
            this.idToConnection.Add(player, connection);
            this.connectionToId.Add(connection, player);
            this.allConnections.Add(connection);
        }

        public void Remove(Id<Player> id)
        {
            this.idToConnection.Remove(id);
            var connection = this[id];
            this.connectionToId.Remove(connection);
            this.allConnections.Remove(connection);
        }

        public Id<Player> this[NetConnection connection]
        {
            get { return this.connectionToId[connection]; }
        }
        public NetConnection this[Id<Player> id]
        {
            get { return this.idToConnection[id]; }
        }

        public IEnumerator<NetConnection> GetEnumerator()
        {
            return this.allConnections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count { get { return this.allConnections.Count; } }

        NetConnection IReadOnlyList<NetConnection>.this[int index]
        {
            get { return this.allConnections[index]; }
        }
    }
}
