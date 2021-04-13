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
        public Invite(string userName = "")
        {
            InitializeComponent();
            label1.Text = string.Format("{0} хочет сыграть с вами.", userName);
            this.userName = userName;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await HubService.AcceptInvite(userName, true);
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
