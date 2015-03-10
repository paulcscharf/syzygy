using Bearded.Utilities.SpaceTime;

namespace Syzygy.Game.Astronomy
{
    interface IBody : IDeletable<IBody>
    {
        Circle Shape { get; }
        float Mass { get; }
    }
}
