using Syzygy.Game.Astronomy;
using Syzygy.Game.SyncedCommands;

namespace Syzygy.Game.Behaviours
{
    sealed class ServerCollisionHandler : ICollisionHandler
    {
        private readonly GameState game;
        private readonly ServerCommandSender commandSender;

        public ServerCollisionHandler(GameState game, ServerCommandSender commandSender)
        {
            this.game = game;
            this.commandSender = commandSender;
        }

        public void HandleCollision(FreeObject obj)
        {
            var position = obj.Position;

            foreach (var body in this.game.Bodies)
            {
                var shape = body.Shape;

                var difference = shape.Center - position;

                var distanceSquared = difference.LengthSquared;

                if (distanceSquared < shape.Radius.Squared)
                {
                    var command = ParticlePlanetCollision.Command(this.game, obj, body);

                    this.commandSender.ExecuteAndSend(command);

                    if (obj.Deleted)
                        return;
                }
            }
        }
    }
}
