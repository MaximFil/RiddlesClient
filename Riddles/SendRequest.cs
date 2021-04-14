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
        private readonly LevelService levelService;
        private readonly GameSessionService gameSessionService;
        private readonly LongOperation longOperation;
        private int choosedUserId;
        private bool dispose { get; set; }
        private Dictionary<string, int> FreeUserNames { get; set; }
        private Dictionary<string, int> Levels { get; set; }
        private Level Level { get; set; }
        public SendRequest()
        {
            InitializeComponent();
            userService = new UserService();
            levelService = new LevelService();
            gameSessionService = new GameSessionService();
            longOperation = new LongOperation(this);
            dispose = true;
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
            FreeUserNames = userService.GetFreeUserNames().GetAwaiter().GetResult();

            if (!FreeUserNames.TryGetValue(textBox1.Text, out choosedUserId))
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
                int levelId;
                if(!Levels.TryGetValue(Level.ToString(), out levelId))
                {
                    MessageBox.Show("Возникли проблемы с уровнями. Попробуйте ещё раз!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    gameSessionService.CreateGameSession(UserProfile.Id, choosedUserId, levelId);
                    MessageBox.Show("All good!");
                }
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

        private async void SendRequest_Load(object sender, EventArgs e)
        {
            Levels = await levelService.GetLevels();
        }
    }
}
