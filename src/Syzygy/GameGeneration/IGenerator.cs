using System.Collections.Generic;
using Syzygy.Game;
using Syzygy.GameManagement;

namespace Syzygy.GameGeneration
{
    interface IGenerator
    {
        IEnumerable<IGenerationInstruction> Generate(IList<Id<Player>> playerIds);
    }
}
