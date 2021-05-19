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
        private bool dispose;
        public About_The_Game()
        {
            InitializeComponent();
            UserProfile.CurrentForm = this;
            this.dispose = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        public void CloseForm()
        {
            dispose = false;
            this?.Close();
        }

        private void About_The_Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(dispose == true)
            {
                var menu = new Menu();
                menu.Show();
            }
        }
    }
}
