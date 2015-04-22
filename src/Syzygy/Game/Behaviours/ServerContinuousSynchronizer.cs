using System;
using System.Collections.Generic;
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

        private TimeSpan timeToNextObjectUpdate;
        private TimeSpan timeToNextEcoUpdate;

        public ServerContinuousSynchronizer(GameState game, NetServer server, PlayerConnectionLookup connections)
            : base(game)
        {
            this.server = server;
            this.connections = connections;
            this.timeToNextObjectUpdate = TimeSpan.One;
            this.timeToNextEcoUpdate = TimeSpan.One;
        }

        public void Sync(FreeObject obj)
        {
            this.objects.Enqueue(obj);
        }

        public override void Update(TimeSpan t)
        {
            this.timeToNextObjectUpdate -= t;
            this.timeToNextEcoUpdate -= t;

            if (this.timeToNextEcoUpdate < TimeSpan.Zero)
            {
                this.sendEcoUpdate();
                this.timeToNextEcoUpdate = TimeSpan.One;
            }

            if (this.timeToNextObjectUpdate < TimeSpan.Zero)
            {
                this.sendObjectUpdate();
                // TODO: check math (intent: sends packages more often when there are more objects, to keep update frequency of single objects stable)
                this.timeToNextObjectUpdate = TimeSpan.One * (0.5f / (this.objects.Count + 2));
            }
        }

        private void sendEcoUpdate()
        {
            if (this.connections.Count == 0)
                return;

            var command = EconomyUpdate.Command(this.game);

            var message = this.server.CreateMessage();
            message.Write((byte)IngameMessageType.Command);
            command.WriteToBuffer(message);

            this.server.SendMessage(message, this.connections,
                NetDeliveryMethod.UnreliableSequenced, Settings.Network.Channel.EconomyUpdates);
        }

        private void sendObjectUpdate()
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

            this.server.SendMessage(message, this.connections,
                NetDeliveryMethod.UnreliableSequenced, Settings.Network.Channel.ParticleUpdates);
        }

        public override void Draw(GeometryManager geos)
        {
        }
    }
}
