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

            if (httpRequestAuthorization == null)
            {
                throw new ArgumentNullException();
            }

            if (httpRequestAuthorization.Length < httpRequestPrefix.Length)
            {
                throw new ArgumentException("httpRequestAuthorization insufficient length");
            }

            return httpRequestAuthorization.Substring(httpRequestPrefix.Length);
        }
    }
}
