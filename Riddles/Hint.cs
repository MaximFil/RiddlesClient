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
    public partial class Hint : Form
    {
        public Hint()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Cursor.Position.X - this.Width, Cursor.Position.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.UseHint(HintType.HalfWord);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.UseHint(HintType.FullWord);
        }
        private void UseHint(HintType hintType)
        {
            var res = Playground.UseHint(hintType);
            if (res.Equals("success", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("You do not have enough hint points", "Error");
            }
        }
    }
}
