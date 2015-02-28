using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Syzygy.Forms
{
    public partial class LobbyForm : Form
    {
        private readonly Dictionary<int, string> players = new Dictionary<int, string>();

        public LobbyForm()
        {
            this.InitializeComponent();
        }

        public void AddPlayer(int id, string name)
        {
            this.players.Add(id, name);
            this.playersTextBox.AppendText(name + Environment.NewLine);
        }
    }
}
