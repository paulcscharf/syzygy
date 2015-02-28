using amulware.Graphics;
using Bearded.Utilities;

namespace Syzygy.GameManagement
{
    interface IGameHandler
    {
        void Update(UpdateEventArgs e);

        event GenericEventHandler<IGameHandler> Stopped; 
    }
}
