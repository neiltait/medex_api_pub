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
        public string UserId { get; set; }

        /// <summary>
        ///     First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Email Address
        /// </summary>
        public string EmailAddress { get; set; }
    }
}