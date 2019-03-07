using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MedicalExaminer.MVCExample.Services
{
    public class MEAPIService : IAPIService
    {
        private HttpClient client = new HttpClient();

        public async Task<IEnumerable<object>> GetValues(string token)
        {
            var values = new List<object>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:44355/values");

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                values = JsonConvert.DeserializeObject<List<object>>(json);
            }
            else
            {
                values = new List<object> { response.StatusCode.ToString(), response.ReasonPhrase };
            }

            return values;
        }
    }
}
