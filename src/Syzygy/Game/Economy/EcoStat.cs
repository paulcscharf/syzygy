using System;
using Bearded.Utilities.Math;

namespace Syzygy.Game.Economy
{
    sealed class EcoStat
    {
        private readonly double returnFactor;
        private double investment = 0.5f;

        public EcoStat(double value = 0, double returnFactor = 1)
        {
            this.returnFactor = returnFactor;
            this.Value = value;
        }

        public double Value { get; private set; }

        public double ReturnFactor { get { return this.returnFactor; } }

        public double Investment
        {
            get { return this.investment; }
            set { this.investment = value.Clamped(0, 1); }
        }


        public void Invest(double value)
        {
            this.Value += this.ReturnFactor * value;
        }

        public bool TrySpend(double value)
        {
            if (value <= this.Value)
            {
                this.Value -= value;
                return true;
            }
            return false;
        }

        public void Spend(double value)
        {
            if(value > this.Value)
                throw new Exception("Cannot spend more resources than available.");
            this.Value -= value;
        }
    }
}