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
    public class RiddleService
    {
        private readonly HttpClient client;

        public RiddleService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["hostAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Riddle> GetRiddlesByGameSessionId(int gameSessionId)
        {
            var result = new List<Riddle>();
            try
            {
                HttpResponseMessage response = client.GetAsync($"api/riddles/getriddlesbygamesessionid/{gameSessionId}").GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<List<Riddle>>().GetAwaiter().GetResult();
                }
            }
            catch(Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
