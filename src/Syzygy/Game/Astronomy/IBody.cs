using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;

namespace Syzygy.Game.Astronomy
{
    interface IBody : IDeletable
    {
        Circle Shape { get; }
        float Mass { get; }
    }
}
