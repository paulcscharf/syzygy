using System.Linq;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Lidgren.Network;
using Syzygy.Game.Astronomy;
using Syzygy.Game.FreeObjects;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    static class ShootProjectileFromPlanet
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
            private readonly Id playerId;
            private readonly Direction2 direction;

            public Id<IBody> BodyId { get { return this.bodyId.Generic<IBody>(); } }
            public Id<Player> PlayerId { get { return this.playerId.Generic<Player>(); } }
            public Direction2 Direction { get { return this.direction; } }


            public RequestParameters(IBody body, Player player, Direction2 direction)
            {
                this.bodyId = body.Id.Simple;
                this.playerId = player.ID.Simple;
                this.direction = direction;
            }
        }

        private struct CommandParameters
        {
            private readonly Id id;
            private readonly Id playerId;
            private readonly Position2 position;
            private readonly Velocity2 velocity;

            public Id<FreeObject> ID { get { return this.id.Generic<FreeObject>(); } }
            public Id<Player> PlayerId { get { return this.playerId.Generic<Player>(); } }
            public Position2 Position { get { return this.position; } }
            public Velocity2 Velocity { get { return this.velocity; } }

            public CommandParameters(Id<FreeObject> id, Player player, Position2 position, Velocity2 velocity)
            {
                this.id = id.Simple;
                this.playerId = player.ID.Simple;
                this.position = position;
                this.velocity = velocity;
            }
        }

        private sealed class RequestImplementation : BaseRequest<RequestParameters>
        {
            private readonly IBody body;
            private readonly Direction2 direction;
            private readonly bool validPlayer;

            public RequestImplementation(IPlayerController controller, GameState game,
                IBody body, Direction2 direction)
                : base(RequestType.ShootProjectileFromPlanet, game, controller)
            {
                this.body = body;
                this.direction = direction;
                this.validPlayer = true;
            }

            public RequestImplementation(PlayerConnectionLookup connectionLookup, NetConnection connection,
                NetBuffer buffer, GameState game)
                : base(RequestType.ShootProjectileFromPlanet, game, connectionLookup, connection)
            {
                var p = buffer.Read<RequestParameters>();
                this.body = game.Bodies[p.BodyId];
                this.direction = p.Direction;
                this.validPlayer = this.Requester.ID == p.PlayerId;
            }

            public override bool CheckPreconditions()
            {
                return validPlayer
                    && this.game != null
                    && this.body != null
                    && this.game.Economies
                        .Any(e => e.Body == this.body && e.Player == this.Requester)
                    ;
            }

            public override ICommand MakeCommand()
            {
                return new CommandImplementation(this.game, this.body, this.Requester, this.direction);
            }

            protected override RequestParameters parameters
            {
                get { return new RequestParameters(this.body, this.Requester, this.direction); }
            }
        }

        private sealed class CommandImplementation : BaseCommand<CommandParameters>
        {
            private readonly CommandParameters ps;

            public CommandImplementation(GameState game, IBody body, Player player, Direction2 direction)
                : base(CommandType.ShootProjectileFromPlanet, game)
            {
                var bodyShape = body.Shape;

                var d = Difference2.In(direction, 0.9f.U());

                this.ps = new CommandParameters(
                    this.game.GetUniqueId<FreeObject>(),
                    player,
                    bodyShape.Center + d * bodyShape.Radius.NumericValue * 1.5f,
                    body.Velocity + d / TimeSpan.One
                    );
            }

            public CommandImplementation(NetBuffer buffer, GameState game)
                : base(CommandType.ShootProjectileFromPlanet, game)
            {
                this.ps = buffer.Read<CommandParameters>();
            }

            public override void Execute()
            {
                new Projectile(this.game, this.ps.ID, this.game.Players[this.ps.PlayerId], this.ps.Position, this.ps.Velocity);
            }

            protected override CommandParameters parameters
            {
                get { return this.ps; }
            }
        }
    }
}
