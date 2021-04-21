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

namespace Riddles.Services
{
    public class HintService
    {
        private readonly HttpClient client;

        public HintService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["hostAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Entities.Hint> GetHints()
        {
            var hints = new List<Entities.Hint>();
            var response = client.GetAsync($"api/hint/gethints/").GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsAsync<ApiResponse>().GetAwaiter().GetResult();
                if (apiResponse.Success)
                {
                    hints = JsonConvert.DeserializeObject<List<Entities.Hint>>(apiResponse.Json);
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

            return hints;
        }
    }
}
