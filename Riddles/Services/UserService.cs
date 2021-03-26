using Riddles.DAL;
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
    public class UserService
    {
        private readonly HttpClient client;

        public UserService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["hostAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<string> GetData()
        {
            var result = new List<string>();
            HttpResponseMessage response = client.GetAsync("api/user/getusernames").GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<List<string>>().GetAwaiter().GetResult();
            }

            return result;
        }
    }
}
