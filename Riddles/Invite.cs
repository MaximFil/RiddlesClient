using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Riddles.Services;

namespace Riddles
{
    public partial class Invite : Form
    {
        private readonly string userName;
        private readonly string levelName;
        private int waitingTime = 0;
        private const int leftIndent = 30;
        public Invite(string userName = "", string levelName = "", string message = "")
        {
            InitializeComponent();
            label1.Text = message;
            this.userName = userName;
            this.levelName = levelName;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await HubService.AcceptInvite(userName, true);
            UserProfile.Level = Levels.DictionaryLevels[levelName];
            UserProfile.RivalName = userName;
            this.Close();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await HubService.AcceptInvite(userName, false);
            this.Close();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            waitingTime++;
            if(waitingTime == 31)
            {
                await HubService.AcceptInvite(userName, false);
                this.Close();
            }
        }

        private void Invite_Load(object sender, EventArgs e)
        {
            label1.MaximumSize = new Size(this.Size.Width - leftIndent, this.Size.Height);
        }

        private void Invite_Resize(object sender, EventArgs e)
        {
            label1.MaximumSize = new Size(this.Size.Width - leftIndent, this.Size.Height);
        }
    }
}
