using System;
using Riddles.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.Services
{
    public class AnswerHistoryService
    {
        private readonly HttpClient client;

        public AnswerHistoryService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["hostAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task CreateHistory(int gameSessionId, int userId, int riddleId, string answer, bool correct)
        {
            try
            {
                var history = new GameSessionAnswerHistory()
                {
                    GameSessionId = gameSessionId,
                    UserId = userId,
                    RiddleId = riddleId,
                    Answer = answer,
                    Correct = correct,
                    AnswerDate = DateTime.Now
                };
                await client.PostAsJsonAsync<GameSessionAnswerHistory>($"api/answerhistory/createanswerhistory", history);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
