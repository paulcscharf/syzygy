using System.Collections.Generic;
using System.Windows.Forms;
using Bearded.Utilities;
using Syzygy.Game;
using Syzygy.GameManagement;
using Environment = System.Environment;

namespace Syzygy.Forms
{
    partial class LobbyForm : Form
    {
        private readonly Dictionary<Id<Player>, string> players = new Dictionary<Id<Player>, string>();

        public event VoidEventHandler Started;

        public LobbyForm(bool isServer)
        {
            this.InitializeComponent();

            this.startButton.Enabled = isServer;
        }

        public void AddPlayer(Id<Player> id, string name)
        {
            this.players.Add(id, name);
            this.playersTextBox.AppendText(name + Environment.NewLine);
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            if (this.Started != null)
                this.Started();
        }
    }
}
