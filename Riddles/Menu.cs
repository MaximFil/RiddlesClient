using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Riddles.DAL;

namespace Riddles
{
    public partial class Menu : Form
    {
        private bool dispose = true;
       // public User User { get; set; }
        public Menu()
        {
            InitializeComponent();
            //this.User = user;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SendRequest sendRequest = new SendRequest();
            sendRequest.Show();
            dispose = false;
            this.Close();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            About_The_Game about_The_Game = new About_The_Game();
            about_The_Game.Show();
            dispose = false;
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TableRecords records = new TableRecords();
            records.Show();
            dispose = false;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dispose)
            {
                Application.Exit();
            }
        }
    }
}
