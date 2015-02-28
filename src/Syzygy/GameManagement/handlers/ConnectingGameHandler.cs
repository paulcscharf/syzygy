using amulware.Graphics;
using Bearded.Utilities;

namespace Syzygy.GameManagement
{
    sealed class ConnectingGameHandler : IGameHandler
    {
        public ConnectingGameHandler(string playerName, string ipAddress)
        {
        }

        public event GenericEventHandler<IGameHandler> Stopped;

        public void Update(UpdateEventArgs e)
        {
        }
    }
}
