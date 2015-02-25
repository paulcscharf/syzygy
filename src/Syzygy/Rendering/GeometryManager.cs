using amulware.Graphics;
using Bearded.Utilities;

namespace Syzygy.Rendering
{
    sealed class GeometryManager : Singleton<GeometryManager>
    {
        public PrimitiveGeometry Primitives { get; private set; }

        public GeometryManager()
        {
            var surfaces = SurfaceManager.Instance;

            this.Primitives = new PrimitiveGeometry(surfaces.Primitives);
        }
    }
}
