using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Math;
using Syzygy.Game.Astronomy;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game.Economy
{
    sealed class Economy : GameObject
    {
        private readonly Player player;
        private readonly IBody body;

        private readonly Dictionary<EcoValue, EcoStat> ecoStats = new Dictionary<EcoValue, EcoStat>(3)
        {
            { EcoValue.Income, new EcoStat(1, 0.005f) },
            { EcoValue.Projectiles, new EcoStat(0, 0.5f) },
            { EcoValue.FireRate, new EcoStat(1, 0.025f) },
            { EcoValue.Defenses, new EcoStat(0, 0.01f) },
        };

        private double totalInvestment;

        public Economy(GameState game, Id<Player> player, Id<IBody> body)
            : base(game)
        {
            this.player = game.Players[player];
            this.body = game.Bodies[body];

            this.listAs<Economy>();
        }

        public Player Player { get { return this.player; } }
        public IBody Body { get { return this.body; } }

        public double TotalInvestment { get { return this.totalInvestment; } }
        public double Income { get { return this[EcoValue.Income].Value; } }

        public float DamageFactor
        {
            get
            {
                var defense = this[EcoValue.Defenses].Value;
                return (float)(1 - 2 / Math.PI * Math.Atan(defense));
            }
        }

        public EcoStat this[EcoValue value] { get { return this.ecoStats[value]; } }
        public ICollection<EcoValue> Values { get { return this.ecoStats.Keys; } } 

        public override void Update(TimeSpan t)
        {
            this.totalInvestment = this.ecoStats.Values.Sum(stat => stat.Investment);

            var credits = this.Income * t.NumericValue;

            this.invest(credits);
        }

        private void invest(double credits)
        {
            if (this.totalInvestment == 0)
            {
                this.investEvenly(credits);

                return;
            }

            var normalisedCredits = credits / this.totalInvestment;

            foreach (var stat in ecoStats.Values)
                stat.Invest(normalisedCredits * stat.Investment);
        }

        private void investEvenly(double credits)
        {
            var creditsPerItem = credits / this.ecoStats.Count;

            foreach (var stat in ecoStats.Values)
                stat.Invest(creditsPerItem);
        }

        public override void Draw(GeometryManager geos)
        {
        }
    }
}
