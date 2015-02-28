using amulware.Graphics;
using Bearded.Utilities;

namespace Syzygy.GameManagement
{
    sealed class LobbyClientGameHandler : IGameHandler
    {
        public event GenericEventHandler<IGameHandler> Stopped;

        public void Update(UpdateEventArgs e)
        {
        }
    }
}
