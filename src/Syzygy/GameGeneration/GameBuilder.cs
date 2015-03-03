using System;
using Syzygy.Game;

namespace Syzygy.GameGeneration
{
    class GameBuilder
    {
        private readonly GameState game = new GameState();
        private bool finished;

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
