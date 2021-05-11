using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddles;

namespace Riddles
{
    public partial class UseHint : Form
    {
        public UseHint()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Cursor.Position.X - this.Width, Cursor.Position.Y);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await this.UsingHint(HintType.FullWord);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await this.UsingHint(HintType.OneChar);
        }
        
        private async void button1_Click(object sender, EventArgs e)
        {
            await this.UsingHint(HintType.HalfWord);
        }
        private async Task UsingHint(HintType hintType)
        {
            var res = await Playground.UseHint(hintType);
            if (res.Item1.Equals("success", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show(res.Item2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
