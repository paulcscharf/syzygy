using System.Collections.Generic;

namespace Syzygy.GameGeneration
{
    interface IGenerator
    {
        IEnumerable<IGenerationInstruction> Generate(IList<int> playerIds);
    }
}
