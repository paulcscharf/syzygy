using amulware.Graphics;
using Bearded.Utilities;
using Lidgren.Network;

namespace Syzygy.GameManagement.Client
{
    sealed class ConnectingGameHandler : IGameHandler
    {
        private readonly GameWindow gameWindow;
        private readonly string playerName;
        private readonly NetClient client;

        public event GenericEventHandler<IGameHandler> Stopped;

        public ConnectingGameHandler(GameWindow gameWindow, string playerName, string ipAddress)
        {
            this.gameWindow = gameWindow;
            this.playerName = playerName;
            var config = new NetPeerConfiguration(Settings.Network.AppName);
            
            this.client = new NetClient(config);
            this.client.Start();

            var hail = this.client.CreateMessage();
            hail.Write(playerName);

            this.client.Connect(ipAddress, Settings.Network.DefaultPort, hail);
        }


        public void Update(UpdateEventArgs e)
        {
            NetIncomingMessage message;
            while ((message = this.client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                    {
                        this.onStatusChanged();
                        return; // if status changed we are either connected or disconnected
                    }
                    default:
                    {
                        Log.Line("unhandled message with type: " + message.MessageType);
                        if(message.MessageType == NetIncomingMessageType.DebugMessage)
                            Log.Debug(message.ReadString());
                        break;
                    }
                }
                Log.Line("");
            }
        }

        private void onStatusChanged()
        {
            switch (this.client.ConnectionStatus)
            {
                case NetConnectionStatus.Connected:
                {
                    this.Stopped(new LobbyGameHandler(this.gameWindow, this.client, this.playerName));
                    break;
                }
                case NetConnectionStatus.Disconnected:
                {
                    this.Stopped(new ConnectionGameHandler(this.gameWindow));
                    break;
                }
                    
            }
        }
    }
}
