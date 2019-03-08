using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MedicalExaminer.API.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MedicalExaminer.API.Services.Implementations
{
    /// <summary>
    /// Okta Token Service.
    /// </summary>
    /// <inheritdoc/>
    public class OktaTokenService : ITokenService
    {
        /// <summary>
        /// Okta Settings.
        /// </summary>
        private readonly OktaSettings _oktaSettings;

        /// <summary>
        /// Initialise a new instance of the Okta Token Service.
        /// </summary>
        /// <param name="oktaSettings">Okta settings.</param>
        public OktaTokenService(OktaSettings oktaSettings)
        {
            _oktaSettings = oktaSettings;
        }

        /// <summary>
        /// Introspect.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Introspect Response.</returns>
        public async Task<IntrospectResponse> IntrospectToken(string token)
        {
            var client = new HttpClient();
            var clientId = _oktaSettings.ClientId;
            var clientSecret = _oktaSettings.ClientSecret;
            var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

            var request = new HttpRequestMessage(HttpMethod.Post, _oktaSettings.IntrospectUrl)
            {
                Content = new FormUrlEncodedContent(
                    new Dictionary<string, string> { { "token", token } })
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var introspectResponse = JsonConvert.DeserializeObject<IntrospectResponse>(json);

                return introspectResponse;
            }

            throw new ApplicationException("Unable to verify token from Okta");
        }
    }
}
