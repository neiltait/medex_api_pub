using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Services
{
    /// <summary>
    /// Token Service.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Introspect Token to determine whether it's active and valid.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>Introspect Response.</returns>
        Task<IntrospectResponse> IntrospectToken(string token);
    }
}
