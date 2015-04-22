using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.Behaviours;
using Syzygy.Game.Economy;
using Syzygy.GameGeneration;
using Syzygy.Rendering.Game;

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

        protected void finish(StateContainer<TPeer>.IBuilder state)
        {
            this.gameBuilder.Finish();
            var game = this.gameBuilder.Game;
            var view = new PlayerGameView(game);
            new EconomyController(game, this.ownID, view);

            state.Game = game;
            state.View = view;
        }

    }
}
