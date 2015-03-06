using Lidgren.Network;
using Syzygy.Game;
using Syzygy.GameGeneration;

namespace Syzygy.GameManagement
{
    abstract class GenericBuildGameHandler<TPeer> : GenericGameHandler<TPeer>
        where TPeer : NetPeer
    {
        private GameBuilder gameBuilder;

        public GenericBuildGameHandler(TPeer peer, PlayerLookup players)
            : base(peer)
        {
            this.gameBuilder = new GameBuilder(players);
        }

        protected void executeInstruction(IGenerationInstruction instruction)
        {
            this.gameBuilder.Execute(instruction);
        }

        protected GameState finish()
        {
            this.gameBuilder.Finish();
            return gameBuilder.Game;
        }

    }
}
