using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models.V1.Users
{
    /// <summary>
    /// Response for Post User.
    /// </summary>
    public class PostUserResponse : ResponseBase
    {
        public string Id { get; set; }
    }
}
