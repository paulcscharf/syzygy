using System;
using amulware.Graphics;
using Bearded.Utilities;
using OpenTK;

namespace Syzygy.Rendering
{
    sealed class SurfaceManager : Singleton<SurfaceManager>
    {
        private Matrix4Uniform gameModelview = new Matrix4Uniform("modelviewMatrix");
        private Matrix4Uniform gameProjection = new Matrix4Uniform("projectionMatrix");

        public IndexedSurface<PrimitiveVertexData> Primitives { get; private set; }

        public SurfaceManager()
        {
            var shaders = ShaderManager.Instance;

            this.Primitives = new IndexedSurface<PrimitiveVertexData>();
            this.Primitives.AddSettings(
                gameModelview,
                gameProjection
                );
            shaders.Primitives.UseOnSurface(this.Primitives);

            this.initMatrices();
        }

        private void initMatrices()
        {
            this.gameModelview.Matrix = Matrix4.LookAt(
                new Vector3(0, 0, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0)
                );

        }

        public void Resize(int width, int height)
        {

            const float zNear = 0.1f;
            const float zFar = 256f;
            const float fovy = (float)Math.PI / 4;

            float ratio = (float)width / height;

            float yMax = zNear * (float)Math.Tan(0.5f * fovy);
            float yMin = -yMax;
            float xMin = yMin * ratio;
            float xMax = yMax * ratio;

            this.gameProjection.Matrix = Matrix4.CreatePerspectiveOffCenter(xMin, xMax, yMin, yMax, zNear, zFar);
        }
    }
}
