using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Helpers
{
    /// <summary>
    /// Extracts token element from HttpRequest Authorization string
    /// </summary>
    public class OktaTokenParser
    {
        public static string ParseHttpRequestAuthorisation(string httpRequestAuthorization)
        {
            
            var httpRequestPrefix = "Bearer ";

            if (httpRequestAuthorization is null || httpRequestAuthorization.Length < httpRequestPrefix.Length)
            {
                throw new Exception("Cannot parse httpRequestAuthorization");
            }

            return httpRequestAuthorization.Substring(httpRequestPrefix.Length);
        }
    }
}
