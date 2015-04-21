using amulware.Graphics;
using Bearded.Utilities;
using OpenTK;

namespace Syzygy.Rendering
{
    sealed class GeometryManager : Singleton<GeometryManager>
    {
        public PrimitiveGeometry Primitives { get; private set; }

        public FontGeometry GameText { get; private set; }
        public FontGeometry HudText { get; private set; }

        public GeometryManager()
        {
            var surfaces = SurfaceManager.Instance;

            this.Primitives = new PrimitiveGeometry(surfaces.Primitives);
            this.GameText = new FontGeometry(surfaces.GameText, surfaces.MonoFont)
                { SizeCoefficient = new Vector2(1, -1) };
            this.HudText = new FontGeometry(surfaces.HudText, surfaces.MonoFont)
                { SizeCoefficient = new Vector2(1, -1) };
        }

    }
}
