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

        //public List<Record> GetRecordsByLevel(int levelId)
        //{
        //    try
        //    {

        //    }
        //    catch(Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
