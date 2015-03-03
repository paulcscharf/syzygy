using Lidgren.Network;

namespace Syzygy.GameManagement.Server
{
    sealed class ReadyGameHandler : GenericGameHandler<NetServer>
    {
        public ReadyGameHandler(NetServer server)
            : base(server)
        {
        }
    }
}
