using amulware.Graphics;
using Bearded.Utilities;
using Lidgren.Network;

namespace Syzygy.GameManagement
{
    abstract class GenericGameHandler<TPeer> : IGameHandler
        where TPeer : NetPeer
    {
        protected readonly TPeer peer;
        private bool dontHandleMessages;

        public event GenericEventHandler<IGameHandler> Stopped;

        protected GenericGameHandler(TPeer peer)
        {
            this.peer = peer;
        }

        public virtual void Update(UpdateEventArgs e)
        {
            if (this.dontHandleMessages)
                return;

            NetIncomingMessage message;
            while ((message = this.peer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        {
                            this.onDataMessage(message);
                            break;
                        }
                    default:
                        {
                            this.unhandledMessage(message);
                            break;
                        }
                }
                if (this.dontHandleMessages)
                    break;
            }
        }

        protected virtual void onDataMessage(NetIncomingMessage message)
        {
            this.unhandledMessage(message);
        }

        private void unhandledMessage(NetIncomingMessage message)
        {
            if (message.MessageType == NetIncomingMessageType.DebugMessage)
            {
                Log.Debug("unhandled debug message:");
                Log.Debug(message.ReadString());
            }
            else
                Log.Line("unhandled message with type: " + message.MessageType);
        }

        protected void stop(IGameHandler handler)
        {
            this.Stopped(handler);
        }

        protected void stopMessageHandling()
        {
            this.dontHandleMessages = true;
        }
    }
}
