using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddles
{
    public partial class About_The_Game : Form, ICloseble
    {
        public About_The_Game()
        {
            InitializeComponent();
            UserProfile.CurrentForm = this;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void About_The_Game_Load(object sender, EventArgs e)
        {

        }
        
        public void CloseForm()
        {
            this?.Close();
        }
    }
}
