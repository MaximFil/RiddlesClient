using Newtonsoft.Json;
using Riddles.ResponseModels;
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
    public class LevelService
    {
        private readonly HttpClient client;

        public LevelService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["hostAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Entities.Level>> GetLevels()
        {
            var levels = new List<Entities.Level>();
            var response = await client.GetAsync($"api/level/getlevels/");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponse>();
                if (apiResponse.Success)
                {
                    levels = JsonConvert.DeserializeObject<List<Entities.Level>>(apiResponse.Json);
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

            return levels;
        }
    }
}
