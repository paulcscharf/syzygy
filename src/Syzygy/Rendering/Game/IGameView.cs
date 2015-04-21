namespace Syzygy.Rendering.Game
{
    interface IGameView
    {
        ViewParameters ViewParameters { get; }
        void DrawGame();
    }
}
