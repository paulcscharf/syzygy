using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities;
using Bearded.Utilities.SpaceTime;
using Lidgren.Network;
using Syzygy.Game.Astronomy;

namespace Syzygy.Game.SyncedCommands
{
    static class ParticleUpdate
    {
        public static ICommand Command(GameState game, List<FreeObject> objects)
        {
            return new CommandImplementation(game, objects);
        }

        public static ICommand Command(GameState game, NetBuffer buffer)
        {
            return  new CommandImplementation(game, buffer);
        }

        struct SingleParameters
        {
            private readonly Id id;
            private readonly Position2 position;
            private readonly Velocity2 velocity;

            public Id<FreeObject> ID { get { return this.id.Generic<FreeObject>(); } }
            public Position2 Position { get { return this.position; } }
            public Velocity2 Velocity { get { return this.velocity; } }

            private SingleParameters(Id<FreeObject> id, Position2 position, Velocity2 velocity)
            {
                this.id = id.Simple;
                this.position = position;
                this.velocity = velocity;
            }

            public static SingleParameters FromParticle(FreeObject obj)
            {
                return new SingleParameters(obj.Id, obj.Position, obj.Velocity);
            }
        }

         private class CommandImplementation : ICommand
         {
             private readonly GameState game;
             private readonly List<SingleParameters> parameters;

             public CommandImplementation(GameState game, List<FreeObject> objects)
             {
                 this.game = game;
                 this.parameters = objects.Select(SingleParameters.FromParticle).ToList();
             }

             public CommandImplementation(GameState game, NetBuffer buffer)
             {
                 this.game = game;
                 var time = new Instant(buffer.ReadDouble());

                 this.game.SetTime(time);

                 int count = buffer.ReadByte();
                 this.parameters = new List<SingleParameters>(count);

                 for (int i = 0; i < count; i++)
                 {
                     this.parameters.Add(buffer.Read<SingleParameters>());
                 }
             }

             public bool IsServerOnlyCommand { get { return false; } }

             public void Execute()
             {
                 foreach (var p in this.parameters)
                 {
                     var obj = this.game.FreeObjects[p.ID];
                     if(obj != null)
                        obj.UpdatePositionAndVelocity(p.Position, p.Velocity);
                 }
             }

             public void WriteToBuffer(NetBuffer buffer)
             {
                 buffer.Write((byte)CommandType.ParticleUpdate);
                 buffer.Write(this.game.Time.NumericValue);
                 buffer.Write((byte)this.parameters.Count);

                 foreach (var p in this.parameters)
                 {
                     buffer.Write(p);
                 }
             }
         }
    }
}
