﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riddles.Entities;
using Riddles.ResponseModels;

namespace Riddles.Services
{
    public class GameSessionService
    {
        private readonly HttpClient client;

        public GameSessionService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["hostAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public GameSession CreateGameSession(int firstUserId, int secondUserId, int levelId, int riddlesCount = 5)
        {
            var gameSession = new GameSession() { LevelId = levelId, StartedDate = DateTime.Now, FinishedDate = null, IsCompleted = false };
            var response = client.PostAsJsonAsync<GameSession>($"api/gamesession/creategamesession/{firstUserId}/{secondUserId}/{riddlesCount}", gameSession).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsAsync<ApiResponse>().GetAwaiter().GetResult();
                if (apiResponse.Success)
                {
                    gameSession = JsonConvert.DeserializeObject<GameSession>(apiResponse.Json);
                }
                else
                {
                    throw new Exception(apiResponse.Message);
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return gameSession;
        }

        public GameSession GetGameSessionById(int gameSessionId)
        {
            GameSession gameSession = null;
            var response = client.GetAsync($"api/gamesession/getgamesessionbyid/{gameSessionId}").GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                gameSession = response.Content.ReadAsAsync<GameSession>().GetAwaiter().GetResult();
            }

            return gameSession;
        }
    }
}
