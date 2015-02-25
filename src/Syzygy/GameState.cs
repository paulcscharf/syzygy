using System;
using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy
{
    sealed class GameState
    {
        private Instant time = Instant.Zero;

        public GameState()
        {
            
        }


        public void Update(UpdateEventArgs e)
        {
            var elapsed = e.ElapsedTimeInS.Seconds();

            this.update(elapsed);
        }

        private void update(TimeSpan t)
        {
            this.time += t;
        }

        public void Draw()
        {
            var geos = GeometryManager.Instance;

            geos.Primitives.DrawCircle(0, 0, 1);
        }
    }
}
