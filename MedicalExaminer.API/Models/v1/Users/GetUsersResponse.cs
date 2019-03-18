using System.Collections.Generic;

namespace MedicalExaminer.API.Models.v1.Users
{
    /// <inheritdoc />
    /// <summary>
    ///     Response object for a list of users.
    /// </summary>
    public class GetUsersResponse : ResponseBase
    {
        /// <summary>
        ///     List of Users        ///
        /// </summary>
        public IEnumerable<UserItem> Users { get; set; }
    }
}