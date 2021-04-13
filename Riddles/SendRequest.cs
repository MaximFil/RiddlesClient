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
        private bool dispose;
        //private readonly HubService hubService;
        private HashSet<string> freeUserNames { get; set; }
        public Level Level { get; set; }
        public SendRequest(/*HubService hubService = null*/)
        {
            InitializeComponent();
            userService = new UserService();
            longOperation = new LongOperation(this);
            dispose = true;
            //this.hubService = hubService;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            freeUserNames = userService.GetFreeUserNames().GetAwaiter().GetResult();
            if (!freeUserNames.Contains(textBox1.Text))
            {
                MessageBox.Show("Пользователь с таким именем не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dispose = false;
                //longOperation.Start();
                await HubService.SendInvite(textBox1.Text, this);
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
            //longOperation.Stop();
            if (accept)
            {
                //Playground playground = new Playground(this.Level);
                //playground.Show();
                //this.Close();
                MessageBox.Show("All good!");
            }
            else
            {
                MessageBox.Show($"Пользователь {textBox1.Text} отклонил ваше прглашение!", "Приглашения", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SendRequest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dispose)
            {
                Application.Exit();
            }
        }
    }
}
