using System.Text;
using amulware.Graphics;
using Bearded.Utilities.Input;
using Bearded.Utilities.SpaceTime;
using OpenTK;
using Syzygy.Rendering;

namespace Syzygy.Game.Economy
{
    sealed class EcoStatController
    {
        private readonly Economy economy;
        private readonly EcoValue value;
        private readonly IAction decreaseInvestment;
        private readonly IAction increaseInvestment;
        private readonly string controlString;

        public EcoStatController(Economy economy, EcoValue value,
            IAction decreaseInvestment, IAction increaseInvestment, string controlString)
        {
            this.economy = economy;
            this.value = value;
            this.decreaseInvestment = decreaseInvestment;
            this.increaseInvestment = increaseInvestment;
            this.controlString = controlString;
        }

        public void Update(TimeSpan t)
        {
            if (this.increaseInvestment.Hit)
                this.economy[this.value].Investment += 0.1;

            if (this.decreaseInvestment.Hit)
                this.economy[this.value].Investment -= 0.1;
        }

        public void Draw(GeometryManager geos)
        {
            var text = geos.HudText;
            text.Height = 0.5f;

            var p = new Vector2(-16f, 9f - 0.5f * (int)this.value);

            var investmentBarBuilder = new StringBuilder(13);

            text.Color = Color.White;
            text.DrawString(p, this.value.ToString());

            var stats = this.economy[this.value];

            text.Color = Color.White;
            text.DrawString(p + new Vector2(3, 0), stats.Value.ToString("0.00"));

            var income = stats.ReturnFactor * stats.Investment * this.economy.Income / this.economy.TotalInvestment;

            text.Color = Color.Gray;
            text.DrawString(p + new Vector2(4.25f, 0), string.Format("+{0:0.000}", income));

            var investment = stats.Investment;
            var barBefore = (int)(investment * 10);

            investmentBarBuilder.Clear();
            investmentBarBuilder.Append('<');
            investmentBarBuilder.Append('-', barBefore);
            investmentBarBuilder.Append('#');
            investmentBarBuilder.Append('-', 10 - barBefore);
            investmentBarBuilder.Append('>');

            text.Color = Color.White;
            text.DrawString(p + new Vector2(6f, 0), investmentBarBuilder.ToString());

            text.Color = Color.Gray;
            text.DrawString(p + new Vector2(9.5f, 0), this.controlString);
        }
    }
}
