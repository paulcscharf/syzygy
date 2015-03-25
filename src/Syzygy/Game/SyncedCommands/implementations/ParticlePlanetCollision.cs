using Lidgren.Network;
using Syzygy.Game.Astronomy;

namespace Syzygy.Game.SyncedCommands
{
    static class ParticlePlanetCollision
    {
        public static ICommand Command(GameState game, NetBuffer buffer)
        {
            return new CommandImplementation(game, buffer);
        }

        public static ICommand Command(GameState game, FreeObject particle, IBody body)
        {
            return new CommandImplementation(game, particle, body);
        }

        private struct Parameters
        {
            private readonly Id particleId;
            private readonly Id bodyId;

            public Parameters(FreeObject particle, IBody body)
            {
                this.particleId = particle.Id.Simple;
                this.bodyId = body.Id.Simple;
            }

            public Id<FreeObject> ParticleId
            {
                get { return this.particleId.Generic<FreeObject>(); }
            }

            public Id<IBody> BodyId
            {
                get { return this.bodyId.Generic<IBody>(); }
            }
        }

        private class CommandImplementation : BaseCommand<Parameters>
        {
            private readonly FreeObject particle;
            private readonly IBody body;

            public CommandImplementation(GameState game, NetBuffer buffer)
                : base(CommandType.ParticlePlanetCollision, game)
            {
                var parameters = buffer.Read<Parameters>();
                this.body = game.Bodies[parameters.BodyId];
                this.particle = game.FreeObjects[parameters.ParticleId];
            }

            public CommandImplementation(GameState game, FreeObject particle, IBody body)
                : base(CommandType.ParticlePlanetCollision, game)
            {
                this.particle = particle;
                this.body = body;
            }

            public override void Execute()
            {
                this.particle.HitBody(this.body);
            }

            protected override Parameters parameters
            {
                get { return new Parameters(this.particle, this.body); }
            }
        }
    }
}
