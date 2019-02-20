using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models.v1.Users
{
    /// <summary>
    /// Response object for a list of users.
    /// </summary>
    public class GetUsersResponse : ResponseBase
    {
        /// <summary>
        /// List of Users
        /// </summary>
        public IEnumerable<UserItem> Users;
    }
}
