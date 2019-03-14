using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MedicalExaminer.API.Services
{
    /// <summary>
    /// Introspect Response
    /// </summary>
    public class IntrospectResponse
    {
        /// <summary>
        /// Active
        /// </summary>
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        /// <summary>
        /// Audeince
        /// </summary>
        [JsonProperty(PropertyName = "aud")]
        public string Audience { get; set; }

        /// <summary>
        /// Authority
        /// </summary>
        [JsonProperty(PropertyName = "iss")]
        public string Authority { get; set; }
    }
}
