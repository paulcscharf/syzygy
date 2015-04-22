using System;
using Syzygy.Game.SyncedCommands;

namespace Syzygy.Game.Behaviours
{
    sealed class ServerRequestHandler : IRequestHandler
    {
        private readonly ServerCommandSender commandSender;

        public ServerRequestHandler(ServerCommandSender commandSender)
        {
            this.commandSender = commandSender;
        }

        public void TryDo(IRequest request)
        {
            if (request.IsClientOnlyRequest)
                return;

            if (!request.CheckPreconditions())
            {
                Console.WriteLine("Invalid request received and disregarded.");
                return;
            }

            var command = request.MakeCommand();

            this.commandSender.ExecuteAndSend(command);
        }
    }
}
