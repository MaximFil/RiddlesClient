using Newtonsoft.Json;
using Riddles.Entities;
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

        public async Task<HashSet<string>> GetUsedUserNames()
        {
            var result = new List<string>();
            HttpResponseMessage response = await client.GetAsync("api/user/getusernames");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<List<string>>();
            }

            return result.ToHashSet();
        }

        public User LogIn(string login, string password)
        {
            User user = new User();
            var response = client.GetAsync($"api/user/login/{login}/{password}").GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsAsync<ApiResponse>().GetAwaiter().GetResult();
                if (apiResponse.Success)
                {
                    user = JsonConvert.DeserializeObject<User>(apiResponse.Json);
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

            return user;
        }

        public User SignUp(string login, string password)
        {
            User user = new User() { Name = login, Password = password };
            var response = client.PostAsJsonAsync("api/user/signup", user).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsAsync<ApiResponse>().GetAwaiter().GetResult();
                if (apiResponse.Success)
                {
                    user = JsonConvert.DeserializeObject<User>(apiResponse.Json);
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

            return user;
        }

        public bool ChangeActivityOfUser(int userId, bool isActive)
        {
            if (userId < 1) return false;
            var response = client.PutAsJsonAsync($"api/user/ChangeActivityOfUser/{userId}", isActive).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<bool>().GetAwaiter().GetResult();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public bool ChangeIsPlayingOfUser(int userId, bool isPlaying)
        {
            if (userId < 1) return false;
            var response = client.PutAsJsonAsync($"api/user/ChangeIsPlayingOfUser/{userId}", isPlaying).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<bool>().GetAwaiter().GetResult();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
