using Bearded.Utilities.Input;
using OpenTK.Input;

namespace Syzygy.Game
{
    sealed class PlayerControls
    {
        private readonly IAction rotateClockwise;
        private readonly IAction rotateCounterClockwise;
        private readonly IAction shoot;
        private readonly IAction zoomIn;
        private readonly IAction zoomOut;

        public IAction RotateClockwise { get { return this.rotateClockwise; } }
        public IAction RotateCounterClockwise { get { return this.rotateCounterClockwise; } }
        public IAction Shoot { get { return this.shoot; } }
        public IAction ZoomIn { get { return this.zoomIn; } }
        public IAction ZoomOut { get { return this.zoomOut; } }

        public PlayerControls()
        {
            this.rotateClockwise = KeyboardAction.FromKey(Key.Right);
            this.rotateCounterClockwise = KeyboardAction.FromKey(Key.Left);
            this.shoot = KeyboardAction.FromKey(Key.Space);
            this.zoomIn = KeyboardAction.FromKey(Key.PageDown);
            this.zoomOut = KeyboardAction.FromKey(Key.PageUp);
        }
    }
}
