using System.Collections.Generic;

namespace Medical_Examiner_API.Models.V1.Users
{
    /// <inheritdoc />
    /// <summary>
    /// Response object for a list of users.
    /// </summary>
    public class GetUsersResponse : ResponseBase
    {
        /// <summary>
        /// List of Users.
        /// </summary>
        public IEnumerable<UserItem> Users { get; set; }
    }
}
