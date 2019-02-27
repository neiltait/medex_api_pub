namespace MedicalExaminer.API.Services
{
    /// <summary>
    /// Example Authentication Service that just always gives out tokens
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Example Authentication that requires no input and generates a token
        /// </summary>
        /// <returns>A token.</returns>
        string Authenticate();
    }
}
