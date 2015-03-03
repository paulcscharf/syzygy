using Lidgren.Network;
using Syzygy.Game;
using Syzygy.GameGeneration;
using Syzygy.GameManagement.Client;

namespace Syzygy.GameManagement
{
    abstract class BuildGameHandler<TPeer> : GenericGameHandler<TPeer>
        where TPeer : NetPeer
    {
        private GameBuilder gameBuilder;

        public BuildGameHandler(TPeer client)
            : base(client)
        {
            this.gameBuilder = new GameBuilder();
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
