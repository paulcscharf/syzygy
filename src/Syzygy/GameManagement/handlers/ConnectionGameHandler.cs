using amulware.Graphics;
using Bearded.Utilities;
using Syzygy.Forms;

namespace Syzygy.GameManagement
{
    sealed class ConnectionGameHandler : IGameHandler
    {
        private readonly GameWindow gameWindow;

        private enum Status
        {
            Waiting = 0, Hosting, Connecting
        }

        private Status status = Status.Waiting;

        public event GenericEventHandler<IGameHandler> Stopped;

        private ConnectionForm form;

        public ConnectionGameHandler(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;

            gameWindow.UIActionQueue.RunAndForget(this.makeForm);

        }

        private void makeForm()
        {
            this.form = new ConnectionForm();

            this.form.Connecting += this.connect;
            this.form.Hosting += this.host;

            this.form.Show();
        }

        private void host()
        {
            this.status = Status.Hosting;
        }

        private void connect()
        {
            this.status = Status.Connecting;
        }

        public void Update(UpdateEventArgs e)
        {
            switch (this.status)
            {
                case Status.Hosting:
                    this.Stopped(new LobbyServerGameHandler(this.gameWindow, this.form.PlayerName));
                    this.gameWindow.UIActionQueue.RunAndForget(this.form.Close);
                    break;
                case Status.Connecting:
                    this.Stopped(new ConnectingGameHandler(this.form.PlayerName, this.form.IpAddress));
                    this.gameWindow.UIActionQueue.RunAndForget(this.form.Close);
                    break;
            }
        }
    }
}
