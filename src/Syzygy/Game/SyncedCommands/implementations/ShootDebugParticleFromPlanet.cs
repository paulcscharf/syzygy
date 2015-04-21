using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Lidgren.Network;
using Syzygy.Game.Astronomy;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    static class ShootDebugParticleFromPlanet
    {
        public static IRequest Request(GameState game, IPlayerController controller, IBody body, Direction2 direction)
        {
            return new RequestImplementation(controller, game, body, direction);
        }

        public static IRequest Request(GameState game, PlayerConnectionLookup connectionLookup, NetConnection connection,
            NetBuffer buffer)
        {
            return new RequestImplementation(connectionLookup, connection, buffer, game);
        }

        public static ICommand Command(GameState game, NetBuffer buffer)
        {
            return new CommandImplementation(buffer, game);
        }

        private struct RequestParameters
        {
            private readonly Id bodyId;
            private readonly Direction2 direction;

            public Id<IBody> BodyId { get { return this.bodyId.Generic<IBody>(); } }
            public Direction2 Direction { get { return this.direction; } }

            public RequestParameters(IBody body, Direction2 direction)
            {
                this.bodyId = body.Id.Simple;
                this.direction = direction;
            }
        }

        private struct CommandParameters
        {
            private readonly Id id;
            private readonly Position2 position;
            private readonly Velocity2 velocity;

            public Id<FreeObject> ID { get { return this.id.Generic<FreeObject>(); } }
            public Position2 Position { get { return this.position; } }
            public Velocity2 Velocity { get { return this.velocity; } }

            public CommandParameters(Id<FreeObject> id, Position2 position, Velocity2 velocity)
            {
                this.id = id.Simple;
                this.position = position;
                this.velocity = velocity;
            }
        }

        private sealed class RequestImplementation : BaseRequest<RequestParameters>
        {
            private readonly IBody body;
            private readonly Direction2 direction;

            public RequestImplementation(IPlayerController controller, GameState game,
                IBody body, Direction2 direction)
                : base(RequestType.ShootDebugParticleFromPlanet, game, controller)
            {
                this.body = body;
                this.direction = direction;
            }

            public RequestImplementation(PlayerConnectionLookup connectionLookup, NetConnection connection,
                NetBuffer buffer, GameState game)
                : base(RequestType.ShootDebugParticleFromPlanet, game, connectionLookup, connection)
            {
                var p = buffer.Read<RequestParameters>();
                this.body = game.Bodies[p.BodyId];
                this.direction = p.Direction;
            }

            public override bool CheckPreconditions()
            {
#if !DEBUG
                return false;
#else
                return this.game != null && this.body != null;
#endif
            }

            public override ICommand MakeCommand()
            {
                return new CommandImplementation(this.game, this.body, this.direction);
            }

            protected override RequestParameters parameters
            {
                get { return new RequestParameters(body, direction); }
            }
        }

        private sealed class CommandImplementation : BaseCommand<CommandParameters>
        {
            private readonly CommandParameters ps;

            public CommandImplementation(GameState game, IBody body, Direction2 direction)
                : base(CommandType.ShootDebugParticleFromPlanet, game)
            {
                var bodyShape = body.Shape;

                var d = Difference2.In(direction, 1.U());

                this.ps = new CommandParameters(
                    this.game.GetUniqueId<FreeObject>(),
                    bodyShape.Center + d * bodyShape.Radius.NumericValue * 1.5f,
                    d * 1 / TimeSpan.One
                    );
            }

            public CommandImplementation(NetBuffer buffer, GameState game)
                : base(CommandType.ShootDebugParticleFromPlanet, game)
            {
                this.ps = buffer.Read<CommandParameters>();
            }

            public override void Execute()
            {
                new FreeObject(this.game, this.ps.ID, this.ps.Position, this.ps.Velocity);
            }

            protected override CommandParameters parameters
            {
                get { return this.ps; }
            }
        }
    }

}
