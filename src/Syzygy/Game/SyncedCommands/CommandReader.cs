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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
