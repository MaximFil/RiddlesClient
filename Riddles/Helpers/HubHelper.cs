using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddles.Services;

namespace Riddles.Helpers
{
    public class HubHelper
    {
        private readonly GameSessionService gameSessionService;

        public HubHelper()
        {
            gameSessionService = new GameSessionService();
        }

        public void SendInvite(string userName, string levelName, string message)
        {
            var inviteForm = new Invite(userName, levelName, message);
            inviteForm.Show();
        }

        public void StartGame(int gameSessionId)
        {
            var gameSession = gameSessionService.GetGameSessionById(gameSessionId);
            Playground playground = new Playground(gameSession);
            playground.Show();
            UserProfile.CurrentForm.CloseForm();
        }

        public void Surrender(string userName)
        {
            Playground.SurrenderRival(userName);
        }

        public void RivalFinished()
        {
            Playground.RivalFinishedGame();
        }

        public void RivalExitedGame()
        {
            Playground.RivalExitedGame();
        }
    }
}
