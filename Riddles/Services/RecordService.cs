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
    public class RecordService
    {
        private readonly HttpClient client;

        public RecordService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["hostAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Record>> GetRecordsByLevel(string levelName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(levelName)) throw new NullReferenceException();
                var response = await client.GetAsync($"api/record/getrecordsbylevel/{levelName}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<Record>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
