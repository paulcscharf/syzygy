using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Rendering.Game;

namespace Syzygy.GameManagement
{
    interface IStateContainer<out TPeer>
        where TPeer : NetPeer
    {
        TPeer Peer { get; }
        GameState Game { get; }
        PlayerLookup Players { get; }
        IGameView View { get; }
    }
}