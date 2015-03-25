using System;
using System.Collections.Generic;
using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Game.Astronomy;
using Syzygy.Game.SyncedCommands;
using Syzygy.GameManagement;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game.Behaviours
{
    sealed class ServerContinuousSynchronizer : GameObject, IContinuousSynchronizer
    {
        private readonly NetServer server;
        private readonly PlayerConnectionLookup connections;

        private readonly Queue<FreeObject> objects = new Queue<FreeObject>();

        private TimeSpan timeToNextUpdate;

        public ServerContinuousSynchronizer(GameState game, NetServer server, PlayerConnectionLookup connections)
            : base(game)
        {
            this.server = server;
            this.connections = connections;
            this.timeToNextUpdate = TimeSpan.One;
        }

        public void Sync(FreeObject obj)
        {
            this.objects.Enqueue(obj);
        }

        public override void Update(TimeSpan t)
        {
            this.timeToNextUpdate -= t;

            if (timeToNextUpdate < TimeSpan.Zero)
            {
                this.sendUpdate();
                // TODO: check math (intent: sends packages more often when there are more objects, to keep update frequency of single objects stable)
                this.timeToNextUpdate = TimeSpan.One * (0.5f / (this.objects.Count + 2));
            }
        }

        private void sendUpdate()
        {
            if (this.objects.Count == 0 || this.connections.Count == 0)
                return;

            var numberToUpdate = Math.Min(20, this.objects.Count); // TODO: replace ugly constant (20)

            var objectsToUpdate = new List<FreeObject>(numberToUpdate);

            while (this.objects.Count > 0 && numberToUpdate > 0)
            {
                var obj = this.objects.Dequeue();

                if (obj.Deleted)
                    continue;
                objectsToUpdate.Add(obj);

                numberToUpdate--;
            }

            foreach (var obj in objectsToUpdate)
            {
                this.objects.Enqueue(obj);
            }

            if (objectsToUpdate.Count == 0)
                return;

            var command = ParticleUpdate.Command(this.game, objectsToUpdate);

            var message = this.server.CreateMessage();
            message.Write((byte)IngameMessageType.Command);
            command.WriteToBuffer(message);

            this.server.SendMessage(message, this.connections, NetDeliveryMethod.UnreliableSequenced, 1);  // TODO: replace ugly constant (1)
        }

        public override void Draw(GeometryManager geos)
        {
        }
    }
}
