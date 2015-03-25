using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Lidgren.Network;
using Syzygy.Game.Astronomy;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    static class ShootDebugParticleFromPlanet
    {
        public static IRequest Request(GameState game, PlayerController controller, IBody body, Direction2 direction)
        {
            return new Implementation(controller, game, body, direction);
        }

        public static IRequest Request(GameState game, PlayerConnectionLookup connectionLookup, NetConnection connection,
            NetBuffer buffer)
        {
            return new Implementation(connectionLookup, connection, buffer, game);
        }

        public static ICommand Command(NetBuffer buffer, GameState game)
        {
            return new Implementation(buffer, game);
        }

        private struct Parameters
        {
            private readonly Id bodyId;
            private readonly Direction2 direction;

            public Id<IBody> BodyId { get { return this.bodyId.Generic<IBody>(); } }
            public Direction2 Direction { get { return this.direction; } }

            public Parameters(IBody body, Direction2 direction)
            {
                this.bodyId = body.Id.Simple;
                this.direction = direction;
            }
        }

        private sealed class Implementation : UnifiedRequestCommand<Parameters>
        {
            private readonly IBody body;
            private readonly Direction2 direction;

            public Implementation(PlayerController controller, GameState game,
                IBody body, Direction2 direction)
                : base(RequestType.ShootDebugParticleFromPlanet, CommandType.ShootDebugparticleFromPlanet, controller, game)
            {
                this.body = body;
                this.direction = direction;
            }

            public Implementation(PlayerConnectionLookup connectionLookup, NetConnection connection,
                NetBuffer buffer, GameState game)
                : base(RequestType.ShootDebugParticleFromPlanet, CommandType.ShootDebugparticleFromPlanet, connectionLookup, connection, game)
            {
                this.init(buffer, out this.body, out this.direction);
            }

            public Implementation(NetBuffer buffer, GameState game)
                : base(CommandType.ShootDebugparticleFromPlanet, game)
            {
                this.init(buffer, out this.body, out this.direction);
            }

            private void init(NetBuffer buffer, out IBody body, out Direction2 direction)
            {
                var p = buffer.Read<Parameters>();
                body = game.Bodies[p.BodyId];
                direction = p.Direction;
            }

            public override bool CheckPreconditions()
            {
#if !DEBUG
                return false;
#else
                return this.game != null && this.body != null;
#endif
            }

            public override void Execute()
            {
                var bodyShape = body.Shape;

                var d = Difference2.In(this.direction, 1.U());

                new FreeObject(this.game, this.game.GetUniqueId<FreeObject>(),
                    bodyShape.Center + d * bodyShape.Radius.NumericValue * 1.5f,
                    d * 5 / TimeSpan.One);
            }

            protected override Parameters parameters
            {
                get { return new Parameters(body, direction); }
            }
        }
    }

}
