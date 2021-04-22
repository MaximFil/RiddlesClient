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
        public Invite(string userName = "", string levelName = "")
        {
            InitializeComponent();
            label1.Text = string.Format("{0} хочет сыграть с вами на уровне {1}.", userName, Levels.DictionaryLevels[levelName].RussianName);
            this.userName = userName;
            this.levelName = levelName;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await HubService.AcceptInvite(userName, true);
            UserProfile.Level = Levels.DictionaryLevels[levelName];
            UserProfile.RivalName = userName;
            this.Close();
        }

        private void Invite_Load(object sender, EventArgs e)
        {
            //Thread.Sleep(30000);
            //if (!this.IsDisposed)
            //{
            //    this.Close();
            //}
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await HubService.AcceptInvite(userName, false);
            this.Close();
        }
    }
}
