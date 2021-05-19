using Riddles.Services;
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
    public partial class ResultForm : Form, ICloseble, IAcceptInite
    {
        private readonly GameSessionService gameSessionService;
        private readonly UserService userService;

        private string TotalTime { get; set; }

        private int TotalPoints { get; set; }

        private string RivalTotalTime { get; set; }

        private int RivalTotalPoints { get; set; }

        private bool? Surrender { get; set; }

        private bool? RivalExited { get; set; }

        private bool? TimeOver { get; set; }

        private bool? RivalTimeOver { get; set; }

        private bool dispose { get; set; }

        private const string WinMessage = "\U0001F60E  Поздравляем! Вы победили игрока {0}!";
        private const string LoseMessage = "\U0001F62D Вы проиграли игроку {0}!";
        private const string DrawMessage = "\U0001F44A Ничья c игроком {0}!";//ничья
        private const string UserTemplateMessage = "Вы ответили на все загадки за {0} и набрали {1} очков.";
        private const string RivalTemplateMessage = "Ваш соперник ответил на все загадки за {0} и набрал {1} очков.";
        private const string SurrenderMassage = "Ваш соперник сдался.";
        private const string RivalExitedMessage = "Ваш соперник вышел из игры.";
        private const string TimeOverMessage = "Ваше время вышло.";
        private const string RivalTimeOverMessage = "Ваш соперник {0} не успел ответить на все загадки за отведенное время.";
        private const int leftIndent = 35;

        public ResultForm(string totalTime, int userTotalPoints, string rivalTotalTime, int rivalTotalPoints, bool? surrender = null, bool? exited = null, bool dispose = true, bool? timeOver = null)
        {
            InitializeComponent();
            this.TotalTime = totalTime;
            this.TotalPoints = userTotalPoints;
            this.RivalTotalTime = rivalTotalTime;
            this.RivalTotalPoints = rivalTotalPoints;
            this.Surrender = surrender;
            this.RivalExited = exited;
            this.TimeOver = timeOver;
            this.gameSessionService = new GameSessionService();
            this.userService = new UserService();
            UserProfile.CurrentForm = this;
            this.dispose = dispose;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var haveUnfinishedChame = await userService.HaveUnFinishedGameSession(UserProfile.RivalName);
            if (haveUnfinishedChame == false)
            {
                pictureBox2.Image = Image.FromFile(@"Resources/99px_ru_animacii_20594_kot_krutitsja_kak_kolesiko_zagruzki.gif");
                pictureBox2.Dock = DockStyle.Fill;
                pictureBox2.Visible = true;
                var message = String.Format("Игрок {0} хочет сыграть с вами ещё раз на уровне {1}!", UserProfile.Login, UserProfile.Level.RussianName);
                await HubService.SendInvite(UserProfile.RivalName, UserProfile.Level.LevelName, message, this);
            }
            else
            {
                MessageBox.Show(string.Format("{0} уже играет с другим игроком, либо вышел из игры!", UserProfile.RivalName), "Реванш", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void ResultForm_Load(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
            label1.MaximumSize = new Size(this.Width - leftIndent, this.Height);
            label2.MaximumSize = new Size(this.Width - leftIndent, this.Height);
            label3.MaximumSize = new Size(this.Width - leftIndent, this.Height);
            var result = String.Empty;
            if(Surrender.HasValue && Surrender.Value)
            {
                label3.Text = string.Format(WinMessage, UserProfile.RivalName);
                label1.Visible = false;
                label2.Text = SurrenderMassage;
                result = "Won";
            }
            else if(RivalExited.HasValue && RivalExited.Value)
            {
                label3.Text = string.Format(WinMessage, UserProfile.RivalName);
                label1.Visible = false;
                label2.Text = RivalExitedMessage;
                result = "Won";
            }
            else if(TimeOver.HasValue && TimeOver.Value == true)
            {
                label3.Text = TimeOverMessage;
                label2.Visible = false;
                label1.Visible = false;
            }
            else
            {
                var userTime = TotalTime.Split(':');
                var userMinutes = Convert.ToInt32(userTime[0]);
                var userSeconds = Convert.ToInt32(userTime[1]);
                var userTotalSeconds = userMinutes * 60 + userSeconds;
                
                var rivalTime = RivalTotalTime.Split(':');
                var rivalMinutes = Convert.ToInt32(rivalTime[0]);
                var rivalSeconds = Convert.ToInt32(rivalTime[1]);
                var rivalTotalSeconds = rivalMinutes * 60 + rivalSeconds;

                label1.Text = string.Format(UserTemplateMessage, TotalTime, TotalPoints);
                label2.Text = string.Format(RivalTemplateMessage, RivalTotalTime, RivalTotalPoints);
                
                if(TotalPoints > RivalTotalPoints || (TotalPoints == RivalTotalPoints && userTotalSeconds < rivalTotalSeconds))
                {
                    label3.Text = string.Format(WinMessage, UserProfile.RivalName);
                    result = "Won";
                }
                else if(TotalPoints == RivalTotalPoints && userTotalSeconds == rivalTotalSeconds)
                {
                    label3.Text = string.Format(DrawMessage, UserProfile.RivalName);
                    result = "Draw";
                }
                else
                {
                    label3.Text = string.Format(LoseMessage, UserProfile.RivalName);
                    result = "Lost";
                }
            }

            await gameSessionService.AddResultToGameSessionUser(UserProfile.GamaSessionId, UserProfile.Id, result);
        }

        private void ResultForm_Resize(object sender, EventArgs e)
        {
            label1.MaximumSize = new Size(this.Width - leftIndent, this.Height);
            label2.MaximumSize = new Size(this.Width - leftIndent, this.Height);
            label3.MaximumSize = new Size(this.Width - leftIndent, this.Height);
        }

        private void ResultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dispose)
            {
                var menu = new Menu();
                menu.Show();
            }
        }

        public void CloseForm()
        {
            this.dispose = false;
            this.Close();
        }

        public async void AcceptInvite(bool accept)
        {
            if (accept)
            {
                dispose = false;
                var gameSession = await gameSessionService.CreateGameSession(UserProfile.Id, UserProfile.RivalName, UserProfile.Level.Id);
                await HubService.StartGame(UserProfile.RivalName, gameSession.Id);
                Playground playground = new Playground(gameSession);
                playground.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Пользователь {UserProfile.RivalName} отклонил ваше приглашение!", "Приглашения", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pictureBox2.Visible = false;
            }
        }
    }
}
