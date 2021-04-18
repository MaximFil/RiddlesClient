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
    public partial class SendRequest : Form, ICloseble
    {
        private readonly UserService userService;
        private readonly LevelService levelService;
        private readonly GameSessionService gameSessionService;
        private readonly LongOperation longOperation;
        private int choosedUserId;
        private int levelId;
        private bool dispose { get; set; }
        private Dictionary<string, int> FreeUserNames { get; set; }
        private Dictionary<string, int> Levels { get; set; }
        private Level Level { get; set; }
        public SendRequest(bool dispose = true)
        {
            InitializeComponent();
            userService = new UserService();
            levelService = new LevelService();
            gameSessionService = new GameSessionService();
            longOperation = new LongOperation(this);
            this.dispose = dispose;
            UserProfile.CurrentForm = this;
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
            if(UserProfile.Login == textBox1.Text)
            {
                MessageBox.Show("Вы не можете играть с самим собой:=)", "Странный запрос", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FreeUserNames = userService.GetFreeUserNames().GetAwaiter().GetResult();

            if (!Levels.TryGetValue(Level.ToString(), out levelId))
            {
                MessageBox.Show("Возникли проблемы с уровнями. Попробуйте ещё раз!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

        public async void AcceptInvite(bool accept)
        {
            //longOperation.Stop();
            if (accept)
            {
                var gameSession = gameSessionService.CreateGameSession(UserProfile.Id, choosedUserId, levelId);
                await HubService.StartGame(textBox1.Text, gameSession.Id);
                Playground playground = new Playground(gameSession);
                playground.Show();
                this.Close();
                
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

        public void CloseForm()
        {
            this?.Close();
        }
    }
}
