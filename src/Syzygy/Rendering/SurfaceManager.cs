using System;
using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Math;
using OpenTK;
using Syzygy.Rendering.Game;

namespace Syzygy.Rendering
{
    sealed class SurfaceManager : Singleton<SurfaceManager>
    {
        private readonly Matrix4Uniform gameModelview = new Matrix4Uniform("modelviewMatrix");
        private readonly Matrix4Uniform gameProjection = new Matrix4Uniform("projectionMatrix");

        private readonly Matrix4Uniform hudModelview = new Matrix4Uniform("modelviewMatrix");
        private readonly Matrix4Uniform hudProjection = new Matrix4Uniform("projectionMatrix");

        public IndexedSurface<PrimitiveVertexData> Primitives { get; private set; }

        #region text

        public Font MonoFont { get; private set; }
        private TextureUniform fontTexture;

        public IndexedSurface<UVColorVertexData> GameText { get; private set; }
        public IndexedSurface<UVColorVertexData> HudText { get; private set; }

        #endregion

        public SurfaceManager()
        {
            var shaders = ShaderManager.Instance;

            this.Primitives = new IndexedSurface<PrimitiveVertexData>();
            this.Primitives.AddSettings(
                gameModelview,
                gameProjection
                );
            shaders.Primitives.UseOnSurface(this.Primitives);

            this.loadFont();


            this.initMatrices();
        }

        private void loadFont()
        {
            var shaders = ShaderManager.Instance;

            this.MonoFont = Font.FromJsonFile("data/fonts/inconsolata.json");

            this.fontTexture = new TextureUniform("diffuseTexture",
                new Texture("data/fonts/inconsolata.png", true));

            this.GameText = new IndexedSurface<UVColorVertexData>();
            this.GameText.AddSettings(
                this.fontTexture, this.gameModelview, this.gameProjection
                );
            shaders.UVColor.UseOnSurface(this.GameText);

            this.HudText = new IndexedSurface<UVColorVertexData>();
            this.HudText.AddSettings(
                this.fontTexture, this.hudModelview, this.hudProjection
                );
            shaders.UVColor.UseOnSurface(this.HudText);
        }

        private void initMatrices()
        {
            this.gameModelview.Matrix = Matrix4.LookAt(
                new Vector3(0, 0, 20), new Vector3(0, 0, 0), new Vector3(0, 1, 0)
                );

            this.hudModelview.Matrix = Matrix4.LookAt(
                new Vector3(0, 0, 22), new Vector3(0, 0, 0), new Vector3(0, 1, 0)
                );
        }

        public void SetMatrices(ViewParameters viewParameters)
        {
            var eye = viewParameters.EyePoint;

            this.gameModelview.Matrix = Matrix4.LookAt(
                eye, eye.Xy.WithZ(), new Vector3(0, 1, 0)
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

            var matrix = Matrix4.CreatePerspectiveOffCenter(xMin, xMax, yMin, yMax, zNear, zFar);

            this.gameProjection.Matrix = matrix;
            this.hudProjection.Matrix = matrix;
        }

    }
}
