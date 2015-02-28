using amulware.Graphics;
using Bearded.Utilities;
using Syzygy.Forms;

namespace Syzygy.GameManagement
{
    sealed class ConnectionGameHandler : IGameHandler
    {
        private enum Status
        {
            Waiting = 0, Hosting, Connecting
        }

        private Status status = Status.Waiting;

        public event GenericEventHandler<IGameHandler> Stopped;

        private readonly ConnectionForm form;

        public ConnectionGameHandler()
        {
            this.form = new ConnectionForm();

            this.form.Connecting += this.connect;
            this.form.Hosting += this.host;

            this.form.Show();
        }

        private void host()
        {
            this.status = Status.Hosting;
            this.form.Close();
        }

        private void connect()
        {
            this.status = Status.Connecting;
            this.form.Close();
        }

        public void Update(UpdateEventArgs e)
        {
            switch (this.status)
            {
                case Status.Hosting:
                    this.Stopped(new LobbyServerGameHandler(this.form.PlayerName));
                    break;
                case Status.Connecting:
                    this.Stopped(new ConnectingGameHandler(this.form.PlayerName, this.form.IpAddress));
                    break;
            }
        }
    }
}
