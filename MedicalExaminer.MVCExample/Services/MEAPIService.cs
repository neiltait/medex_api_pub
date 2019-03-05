using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MedicalExaminer.MVCExample.Services
{
    public class MEAPIService : IAPIService
    {
        private HttpClient client = new HttpClient();

        private readonly ITokenService tokenService;

        public MEAPIService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<IEnumerable<string>> GetValues()
        {
            var values = new List<string>();

            var token = await tokenService.GetToken();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("https://localhost:44355/values");
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                values = JsonConvert.DeserializeObject<List<string>>(json);
            }
            else
            {
                values = new List<string> { response.StatusCode.ToString(), response.ReasonPhrase };
            }
            return values;
        }
    }
}
