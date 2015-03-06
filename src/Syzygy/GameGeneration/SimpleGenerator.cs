using System.Collections.Generic;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game;
using Syzygy.Game.Astronomy;
using Syzygy.GameManagement;

namespace Syzygy.GameGeneration
{
    sealed class SimpleGenerator : IGenerator
    {
        public IEnumerable<IGenerationInstruction> Generate(IList<Id<Player>> playerIds)
        {
            var idMan = new IdManager();

            var sun = idMan.GetNext<IBody>();

            yield return new NewFixedBodyInstruction(sun, new Position2(), Radius.FromValue(1), 1f);

            var planet = idMan.GetNext<IBody>();

            yield return new NewOrbitingBodyInstruction(planet, sun, Radius.FromValue(5), Direction2.Zero, Radius.FromValue(0.5f), 0.25f);
        }
    }
}
