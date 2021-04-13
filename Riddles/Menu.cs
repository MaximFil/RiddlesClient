using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddles.Services;
//using Riddles.DAL;

namespace Riddles
{
    public partial class Menu : Form
    {
        // public User User { get; set; }
        private readonly HubService hubService;
        public Menu(HubService hubService = null)
        {
            InitializeComponent();
            this.hubService = hubService;
            //this.User = user;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SendRequest sendRequest = new SendRequest(hubService);
            sendRequest.Show();
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            About_The_Game about_The_Game = new About_The_Game();
            about_The_Game.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TableRecords records = new TableRecords();
            records.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
