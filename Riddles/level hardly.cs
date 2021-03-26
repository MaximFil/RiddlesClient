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
    public partial class level_hardly : Form
    {
        public User User { get; set; }
        public Level Level { get; set; }
        public level_hardly(User user)
        {
            InitializeComponent();
            this.User = user;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                Level = Level.Easy;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                Level = Level.Hard;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void butdton1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Playground playground = new Playground(this.Level, User);
            playground.Show();
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                Level = Level.Middle;
            }
        }

        private void level_hardly_Load(object sender, EventArgs e)
        {

        }
    }
}
