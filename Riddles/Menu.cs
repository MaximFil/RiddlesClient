using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddles.DAL;

namespace Riddles
{
    public partial class Menu : Form
    {
        public User User { get; set; }
        public Menu(User user)
        {
            InitializeComponent();
            this.User = user;
        }
        public Menu()
        {
            InitializeComponent();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            level_hardly level_Hardly = new level_hardly(this.User);
            level_Hardly.Show();
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
