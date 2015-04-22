using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.SpaceTime;
using Lidgren.Network;
using Syzygy.Game.Economy;

namespace Syzygy.Game.SyncedCommands
{
    sealed class EconomyUpdate
    {
        public static ICommand Command(GameState game)
        {
            return new CommandImplementation(game);
        }

        public static ICommand Command(GameState game, NetBuffer buffer)
        {
            return new CommandImplementation(game, buffer);
        }

        struct ValueParameters
        {
            private readonly EcoValue ecoValue;
            private readonly double value;
            private readonly double investment;

            private ValueParameters(EcoValue ecoValue, double value, double investment)
            {
                this.ecoValue = ecoValue;
                this.value = value;
                this.investment = investment;
            }

            public EcoValue EcoValue { get { return this.ecoValue; } }
            public double Value { get { return this.value; } }
            public double Investment { get { return this.investment; } }

            public static ValueParameters FromEconomy(Economy.Economy eco, EcoValue value)
            {
                var v = eco[value];

                return new ValueParameters(value, v.Value, v.Investment);
            }
        }

        class ParameterGroup
        {
            private readonly Id<Player> id;
            private readonly List<ValueParameters> values;

            public ParameterGroup(Economy.Economy eco)
            {
                this.id = eco.Player.ID;
                this.values = new List<ValueParameters>(eco.Values.Count);
                foreach (var value in eco.Values)
                {
                    this.values.Add(ValueParameters.FromEconomy(eco, value));
                }
            }

            public ParameterGroup(NetBuffer buffer)
            {
                this.id = buffer.Read<Id>().Generic<Player>();
                int count = buffer.ReadByte();
                this.values = new List<ValueParameters>(count);
                for (int i = 0; i < count; i++)
                {
                    this.values.Add(buffer.Read<ValueParameters>());
                }
            }

            public void Write(NetBuffer buffer)
            {
                buffer.Write(this.id.Simple);
                buffer.Write((byte)this.values.Count);
                foreach (var value in this.values)
                {
                    buffer.Write(value);
                }
            }

            public void Apply(GameState game)
            {
                var eco = game.Economies.First(e => e.Player.ID == this.id);

                foreach (var value in this.values)
                {
                    var stat = eco[value.EcoValue];
                    stat.Investment = value.Investment;
                    stat.SetValue(value.Value);
                }
            }
        }

        private class CommandImplementation : ICommand
        {
            private readonly GameState game;
            private readonly List<ParameterGroup> parameters;

            public CommandImplementation(GameState game)
            {
                this.game = game;

                this.parameters = new List<ParameterGroup>(this.game.Players.Count);
                foreach (var economy in this.game.Economies)
                {
                    this.parameters.Add(new ParameterGroup(economy));
                }
            }

            public CommandImplementation(GameState game, NetBuffer buffer)
            {
                this.game = game;
                var time = new Instant(buffer.ReadDouble());

                int count = buffer.ReadByte();
                this.parameters = new List<ParameterGroup>(count);

                for (int i = 0; i < count; i++)
                {
                    this.parameters.Add(new ParameterGroup(buffer));
                }
            }

            public void Execute()
            {
                foreach (var p in this.parameters)
                {
                    p.Apply(this.game);
                }
            }

            public void WriteToBuffer(NetBuffer buffer)
            {
                buffer.Write((byte)CommandType.EconomyUpdate);
                buffer.Write(this.game.Time.NumericValue);

                buffer.Write((byte)this.parameters.Count);

                foreach (var p in this.parameters)
                {
                    p.Write(buffer);
                }
            }
        }
    }
}
