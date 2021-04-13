using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoadingIndicator.WinForms;
using Riddles.Services;

namespace Riddles
{
    public partial class SendRequest : Form
    {
        private readonly UserService userService;
        private readonly LongOperation longOperation;
        private readonly HubService hubService;
        private HashSet<string> freeUserNames { get; set; }
        public Level Level { get; set; }
        public SendRequest(HubService hubService = null)
        {
            InitializeComponent();
            userService = new UserService();
            longOperation = new LongOperation(this);
            this.hubService = hubService;
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

        private void button1_Click(object sender, EventArgs e)
        {
            freeUserNames = userService.GetFreeUserNames().GetAwaiter().GetResult();
            if (!freeUserNames.Contains(textBox1.Text))
            {
                MessageBox.Show("Пользователь с таким именем не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                longOperation.Start();
                hubService.SendInvite(textBox1.Text, this).GetAwaiter().GetResult();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                Level = Level.Middle;
            }
        }

        public void AcceptInvite(bool accept)
        {
            if (accept)
            {
                longOperation.Stop();
                Playground playground = new Playground(this.Level);
                playground.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Пользователь {textBox1.Text} отклонил вашу прглашение!", "Приглашения", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
