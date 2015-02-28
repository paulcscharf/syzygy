using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Bearded.Utilities;

namespace Syzygy.Forms
{
    public partial class ConnectionForm : Form
    {
        public event VoidEventHandler Connecting;
        public event VoidEventHandler Hosting;

        public string PlayerName { get { return this.nameTextBox.Text.Trim(); } }
        public string IpAddress { get { return this.ipTextBox.Text; } }

        public ConnectionForm()
        {
            this.InitializeComponent();

            this.nameTextBox_TextChanged(null, null);
            this.ipTextBox_TextChanged(null, null);
        }

        private bool checkIp()
        {
            IPAddress ip;
            return IPAddress.TryParse(this.IpAddress, out ip);
        }

        private bool checkName()
        {
            return !string.IsNullOrWhiteSpace(this.PlayerName);
        }

        private void connectButton_Click(object sender, System.EventArgs e)
        {
            if (this.checkName() && this.checkIp())
            {
                this.Enabled = false;
                if (this.Connecting != null)
                    this.Connecting();
            }
        }

        private void hostButton_Click(object sender, System.EventArgs e)
        {
            if (this.checkName() && this.checkIp())
            {
                this.Enabled = false;
                if (this.Hosting != null)
                    this.Hosting();
            }
        }

        private void nameTextBox_TextChanged(object sender, System.EventArgs e)
        {
            var valid = this.checkName();
            this.nameTextBox.BackColor = valid ? Color.LightGreen : Color.IndianRed;
            this.hostButton.Enabled = valid;
            if (!valid)
                this.connectButton.Enabled = false;
        }

        private void ipTextBox_TextChanged(object sender, System.EventArgs e)
        {
            var valid = this.checkIp();
            this.ipTextBox.BackColor = valid ? Color.LightGreen : Color.IndianRed;
            this.connectButton.Enabled = this.hostButton.Enabled && valid;
        }
    }
}
