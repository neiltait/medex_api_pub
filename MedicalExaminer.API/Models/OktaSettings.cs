namespace MedicalExaminer.API.Models
{
    /// <summary>
    /// Okta Settings.
    /// </summary>
    public class OktaSettings
    {
        /// <summary>
        /// The Authority.
        /// </summary>
        /// <remarks>Must be same as one issuing tokens.</remarks>
        public string Authority { get; set; }

        /// <summary>
        /// The Audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Introspect Endpoint.
        /// </summary>
        public string IntrospectUrl { get; set; }

        /// <summary>
        /// Client Id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client Secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
