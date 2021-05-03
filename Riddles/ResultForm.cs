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
    public partial class ResultForm : Form
    {
        private readonly GameSessionService gameSessionService;

        private string TotalTime { get; set; }

        private int TotalPoints { get; set; }

        private string RivalTotalTime { get; set; }

        private int RivalTotalPoints { get; set; }

        private bool? Surrender { get; set; }

        private const string WinMessage = "\U0001F60E  Поздравляем! Вы победили игрока {0}!";
        private const string LoseMessage = "\U0001F62D Вы проиграли игроку {0}!";
        private const string DrawMessage = "\U0001F44A Ничья c игроком {0}!";//ничья
        private const string UserTemplateMessage = "Вы ответели на все загадки за {0} и набрали {1} очков.";
        private const string RivalTemplateMessage = "Ваш соперник ответил на все загадки за {0} и набрал {1} очков.";
        private const string SurrenderMassage = "Ваш соперник сдался.";

        public ResultForm(string totalTime, int userTotalPoints, string rivalTotalTime, int rivalTotalPoints, bool? surrender = null)
        {
            InitializeComponent();
            this.TotalTime = totalTime;
            this.TotalPoints = userTotalPoints;
            this.RivalTotalTime = rivalTotalTime;
            this.RivalTotalPoints = rivalTotalPoints;
            this.Surrender = surrender;
            this.gameSessionService = new GameSessionService();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private async void ResultForm_Load(object sender, EventArgs e)
        {
            var result = String.Empty;
            if(Surrender.HasValue && Surrender.Value)
            {
                label3.Text = string.Format(WinMessage, UserProfile.RivalName);
                label2.Text = SurrenderMassage;
                result = "Won";
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
    }
}
