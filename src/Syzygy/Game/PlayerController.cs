namespace Syzygy.Game
{
    class PlayerController
    {
        private readonly Id<Player> playerId;

        public PlayerController(Id<Player> playerId)
        {
            this.playerId = playerId;
        }

        public Id<Player> PlayerId { get { return this.playerId; } }
    }
}
