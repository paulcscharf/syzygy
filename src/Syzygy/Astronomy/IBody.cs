using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;

namespace Syzygy.Astronomy
{
    interface IBody : IDeletable
    {
        Circle Shape { get; }
        float Mass { get; }
    }
}
