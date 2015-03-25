using System;
using Lidgren.Network;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    class RequestReader
    {
        private readonly GameState game;
        private readonly PlayerConnectionLookup connections;

        public RequestReader(GameState game, PlayerConnectionLookup connections)
        {
            this.game = game;
            this.connections = connections;
        }

        public IRequest FromBuffer(NetIncomingMessage message)
        {
            var connection = message.SenderConnection;
            var type = (RequestType)message.ReadByte();
            switch (type)
            {
                case RequestType.ShootDebugParticleFromPlanet:
                    return ShootDebugParticleFromPlanet.Request(this.game,
                        this.connections, connection, message);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
