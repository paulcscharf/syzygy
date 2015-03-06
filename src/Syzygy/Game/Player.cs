
namespace Syzygy.Game
{
    class Player
    {
        private readonly Id<Player> id;
        private readonly string name;

        public Player(Id<Player> id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Id<Player> ID { get { return this.id; } }
        public string Name { get { return this.name; } }
    }
}
