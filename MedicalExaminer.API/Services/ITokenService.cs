using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Services
{
    public interface ITokenService
    {
        Task<IntrospectResponse> IntrospectToken(string token);
    }
}
