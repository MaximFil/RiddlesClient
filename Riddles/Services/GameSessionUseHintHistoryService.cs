using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Riddles.Entities;

namespace Riddles.Services
{
    public class GameSessionUseHintHistoryService
    {
        private readonly HttpClient client;

        public GameSessionUseHintHistoryService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["hostAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task CreateHistory(int gameSessionId, int userId, int riddleId, string hintName, string oldValue, string newValue)
        {
            try
            {
                var history = new GameSessionUseHintHistory()
                {
                    GameSessionId = gameSessionId,
                    UserId = userId,
                    HintId = Hints.DictionaryHints.ContainsKey(hintName) ? Hints.DictionaryHints[hintName].Id : 1,
                    RiddleId = riddleId, 
                    OldAnswerValue = oldValue, 
                    NewAnswerValue = newValue, 
                    UseDate = DateTime.Now
                };
                await client.PostAsJsonAsync<GameSessionUseHintHistory>($"api/hinthistory/createhinthistory", history);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
