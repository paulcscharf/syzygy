using System.Linq;
using Lidgren.Network;
using Syzygy.Game.Economy;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    static class EconomyValueInvestmentChanged
    {
        public static IRequest Request(GameState game, IPlayerController controller, EcoValue value, double investment)
        {
            return new Implementation(controller, game, value, investment);
        }

        public static IRequest Request(GameState game, PlayerConnectionLookup connectionLookup, NetConnection connection,
            NetBuffer buffer)
        {
            return new Implementation(connectionLookup, connection, buffer, game);
        }

        struct Parameters
        {
            private readonly EcoValue value;
            private readonly double investment;

            public Parameters(EcoValue value, double investment)
            {
                this.value = value;
                this.investment = investment;
            }

            public EcoValue Value { get { return this.value; } }
            public double Investment { get { return this.investment; } }
        }

        class Implementation : UnifiedRequestCommand<Parameters>
        {
            private readonly Parameters p;

            public Implementation(IPlayerController controller, GameState game, EcoValue value, double investment)
                : base(RequestType.EconomyValueInvestmentChanged, CommandType.EconomyValueInvestmentChanged, controller, game)
            {
                this.p = new Parameters(value, investment);
            }

            public Implementation(PlayerConnectionLookup connectionLookup, NetConnection connection, NetBuffer buffer, GameState game)
                : base(RequestType.EconomyValueInvestmentChanged, CommandType.EconomyValueInvestmentChanged, connectionLookup, connection, game)
            {
                this.p = buffer.Read<Parameters>();
            }

            public override bool IsServerOnlyCommand { get { return true; } }

            public override bool CheckPreconditions()
            {
                return true;
            }

            public override void Execute()
            {
                var eco = this.game.Economies.First(e => e.Player == this.Requester);
                eco[this.p.Value].Investment = this.p.Investment;
            }

            protected override Parameters parameters
            {
                get { return this.p; }
            }
        }
    }
}
