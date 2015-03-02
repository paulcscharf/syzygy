using System.Collections.Generic;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;

namespace Syzygy.GameGeneration
{
    sealed class SimpleGenerator : IGenerator
    {
        public IEnumerable<IGenerationInstruction> Generate(IList<int> playerIds)
        {
            var idMan = new IdManager();

            var sun = idMan.GetNext<IBody>();

            yield return new NewFixedBodyInstruction(sun, new Position2(), 1f.Units(), 1f);

            var planet = idMan.GetNext<IBody>();

            yield return new NewOrbitingBodyInstruction(planet, sun, 5f.Units(), Direction2.Zero, 1f.Units(), 1f);
        }
    }
}
