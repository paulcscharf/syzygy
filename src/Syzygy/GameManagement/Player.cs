using Lidgren.Network;

namespace Syzygy.GameManagement
{
    class Player
    {
        private readonly Id<Player> id;
        private readonly string name;
        private readonly NetConnection connection;

        public Player(Id<Player> id, string name, NetConnection connection)
        {
            this.id = id;
            this.name = name;
            this.connection = connection;
        }

        public Id<Player> ID { get { return this.id; } }
        public string Name { get { return this.name; } }
        public NetConnection Connection { get { return this.connection; } }
    }
}
