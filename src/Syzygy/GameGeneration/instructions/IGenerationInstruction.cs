using Lidgren.Network;
using Syzygy.Game;

namespace Syzygy.GameGeneration
{
    interface IGenerationInstruction
    {
        void Execute(GameState game);
        void WriteMessage(NetBuffer buffer);
    }

}
