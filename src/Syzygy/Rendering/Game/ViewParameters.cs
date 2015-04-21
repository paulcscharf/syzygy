using OpenTK;

namespace Syzygy.Rendering.Game
{
    struct ViewParameters
    {
        private readonly Vector3 eyePoint;

        public Vector3 EyePoint { get { return this.eyePoint; } }

        public ViewParameters(Vector3 eyePoint)
        {
            this.eyePoint = eyePoint;
        }
    }
}
