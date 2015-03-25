using Lidgren.Network;
using Syzygy.Game.Astronomy;

namespace Syzygy.Game.Behaviours
{
    sealed class ClientBehaviourProvider : IGameBehaviourProvider
    {
        private readonly NetClient client;

        public ClientBehaviourProvider(NetClient client)
        {
            this.client = client;
        }

        public ICollisionHandler GetCollisionHandler(GameState game)
        {
            return new ClientCollisionHandler();
        }

        public IRequestHandler GetRequestHandler()
        {
            return new ClientRequestHandler(this.client);
        }
    }
}
