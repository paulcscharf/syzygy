using Lidgren.Network;

namespace Syzygy.Game.SyncedCommands
{
    interface IRequest
    {
        // ctor From(player controller, parameters) // requesting player is always current player, can be read from player controller
        // ctor From(Buffer, requesting player as gotten from connection)

        Id<Player> Requester { get; } 

        bool CheckPreconditions();
        void WriteToBuffer(NetBuffer buffer);

        ICommand MakeCommand();
    }
}
