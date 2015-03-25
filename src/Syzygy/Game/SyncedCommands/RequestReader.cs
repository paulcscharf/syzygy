using System;
using Lidgren.Network;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    class RequestReader
    {
        private readonly GameState game;
        private readonly PlayerConnectionLookup connectionLookup;

        public RequestReader(GameState game, PlayerConnectionLookup connectionLookup)
        {
            this.game = game;
            this.connectionLookup = connectionLookup;
        }

        public IRequest FromBuffer(NetIncomingMessage message)
        {
            var connection = message.SenderConnection;
            var type = (RequestType)message.ReadByte();
            switch (type)
            {
                case RequestType.ShootDebugParticleFromPlanet:
                    return ShootDebugParticleFromPlanet.Request(this.game,
                        this.connectionLookup, connection, message);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }
}
