using Newtonsoft.Json;

namespace MedicalExaminer.API.Models.v1.Account
{
    /// <summary>
    ///     Post Validate Session Response
    /// </summary>
    public class PostValidateSessionResponse : ResponseBase
    {
        /// <summary>
        ///     User Id
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        /// <summary>
        ///     First Name
        /// </summary>
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        /// <summary>
        ///     Last Name
        /// </summary>
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        /// <summary>
        ///     Email Address
        /// </summary>
        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }
    }
}