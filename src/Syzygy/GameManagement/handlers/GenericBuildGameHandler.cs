using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.Behaviours;
using Syzygy.GameGeneration;

namespace Syzygy.GameManagement
{
    abstract class GenericBuildGameHandler<TPeer> : GenericGameHandler<TPeer>
        where TPeer : NetPeer
    {
        private readonly Id<Player> ownID;
        private readonly GameBuilder gameBuilder;

        protected GenericBuildGameHandler(TPeer peer, PlayerLookup players, Id<Player> ownID, IGameBehaviourProvider behaviourProvider)
            : base(peer)
        {
            this.ownID = ownID;
            this.gameBuilder = new GameBuilder(players, behaviourProvider);
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
