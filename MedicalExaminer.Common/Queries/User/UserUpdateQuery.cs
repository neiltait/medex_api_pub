using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Update Query.
    /// </summary>
    public class UserUpdateQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserUpdateQuery"/>.
        /// </summary>
        /// <param name="userUpdate">User update</param>
        /// <param name="currentUser">Current user.</param>
        public UserUpdateQuery(IUserUpdate userUpdate, MeUser currentUser)
        {
            CurrentUser = currentUser;
            UserUpdate = userUpdate;
        }

        /// <summary>
        /// Current User.
        /// </summary>
        public MeUser CurrentUser { get; }

        /// <summary>
        /// User Update.
        /// </summary>
        public IUserUpdate UserUpdate { get; }
    }
}
