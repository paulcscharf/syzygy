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

            yield return new NewFixedBodyInstruction(sun, new Position2(), Radius.FromValue(1), 1f);

            var planet = idMan.GetNext<IBody>();

            yield return new NewOrbitingBodyInstruction(planet, sun, Radius.FromValue(5), Direction2.Zero, Radius.FromValue(1), 1f);
        }
    }
}
