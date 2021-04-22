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
        private int choosedUserId;
        private Entities.Level level;
        private bool dispose { get; set; }
        private Dictionary<string, int> FreeUserNames { get; set; }
        public SendRequest(bool dispose = true)
        {
            InitializeComponent();
            userService = new UserService();
            levelService = new LevelService();
            gameSessionService = new GameSessionService();
            this.dispose = dispose;
            UserProfile.CurrentForm = this;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                UserProfile.Level = Levels.DictionaryLevels[Level.Easy.ToString()];
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                UserProfile.Level = Levels.DictionaryLevels[Level.Hard.ToString()];
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

            if (!Levels.DictionaryLevels.TryGetValue(UserProfile.Level.LevelName, out level))
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
                await HubService.SendInvite(textBox1.Text, UserProfile.Level.LevelName.ToString(), this);
                UserProfile.RivalName = textBox1.Text;
                pictureBox1.Image = Image.FromFile(@"Resources/99px_ru_animacii_20594_kot_krutitsja_kak_kolesiko_zagruzki.gif");
                pictureBox1.Dock = DockStyle.Fill;
                pictureBox1.Visible = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                UserProfile.Level = Levels.DictionaryLevels[Level.Middle.ToString()];
            }
        }

        public async void AcceptInvite(bool accept)
        {
            if (accept)
            {
                var gameSession = await gameSessionService.CreateGameSession(UserProfile.Id, choosedUserId, level.Id);
                await HubService.StartGame(textBox1.Text, gameSession.Id);
                Playground playground = new Playground(gameSession);
                playground.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Пользователь {textBox1.Text} отклонил ваше приглашение!", "Приглашения", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pictureBox1.Visible = false;
            }
        }

        private void SendRequest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dispose)
            {
                Application.Exit();
            }
        }

        public void CloseForm()
        {
            this?.Close();
        }

        private void SendRequest_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
            pictureBox1.Visible = false;
        }
    }
}
