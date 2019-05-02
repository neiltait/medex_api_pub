using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1
{
    /// <summary>
    /// User Request Interface.
    /// </summary>
    public interface IUserRequest
    {
        /// <summary>
        /// User Identifier.
        /// </summary>
        string UserId { get; set; }
    }
}
