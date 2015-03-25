using System;
using Syzygy.Game;
using Syzygy.Game.Behaviours;
using Syzygy.GameManagement;

namespace Syzygy.GameGeneration
{
    class GameBuilder
    {
        private readonly GameState game;
        private bool finished;

        public GameBuilder(PlayerLookup players, IGameBehaviourProvider behaviourProvider)
        {
            this.game = new GameState(players, behaviourProvider);
        }

        public void Execute(IGenerationInstruction instruction)
        {
            if(this.finished)
                throw new Exception("Cannot modify game after finishing building game.");
            instruction.Execute(this.game);
        }

        public void Finish()
        {
            if (this.finished)
                throw new Exception("Cannot finish building game more than once.");
            this.finished = true;
        }

        public GameState Game { get { return this.game; } }
    }
}
