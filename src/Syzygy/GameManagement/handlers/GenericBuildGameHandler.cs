using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.SyncedCommands;
using Syzygy.GameGeneration;

namespace Syzygy.GameManagement
{
    abstract class GenericBuildGameHandler<TPeer> : GenericGameHandler<TPeer>
        where TPeer : NetPeer
    {
        private readonly Id<Player> ownID;
        private readonly GameBuilder gameBuilder;

        public GenericBuildGameHandler(TPeer peer, PlayerLookup players, Id<Player> ownID, IRequestHandler requestHandler)
            : base(peer)
        {
            this.ownID = ownID;
            this.gameBuilder = new GameBuilder(players, requestHandler);
        }

        protected void executeInstruction(IGenerationInstruction instruction)
        {
            this.gameBuilder.Execute(instruction);
        }

        protected GameState finish()
        {
            new EconomyController(this.gameBuilder.Game, this.ownID);
            this.gameBuilder.Finish();
            return gameBuilder.Game;
        }

    }
}
