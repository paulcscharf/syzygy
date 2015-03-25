using System;
using Lidgren.Network;

namespace Syzygy.Game.SyncedCommands
{
    sealed class CommandReader
    {
        private readonly GameState game;

        public CommandReader(GameState game)
        {
            this.game = game;
        }

        public ICommand FromBuffer(NetIncomingMessage message)
        {
            var type = (CommandType)message.ReadByte();
            switch (type)
            {
                case CommandType.ShootDebugparticleFromPlanet:
                    return ShootDebugParticleFromPlanet.Command(message, this.game);
                case CommandType.ParticlePlanetCollision:
                    return ParticlePlanetCollision.Command(this.game, message);
                case CommandType.ParticleUpdate:
                    return ParticleUpdate.Command(this.game, message);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
