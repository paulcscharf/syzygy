using Bearded.Utilities.SpaceTime;

namespace Syzygy.Game.Astronomy
{
    interface IBody : IIdable<IBody>, IDeletable
    {
        Circle Shape { get; }
        float Mass { get; }

        Velocity2 Velocity { get; }

        float HealthPercentage { get; }
        void DealDamage(float damage);
    }
}
