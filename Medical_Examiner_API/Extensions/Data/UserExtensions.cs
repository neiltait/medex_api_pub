using Medical_Examiner_API.Models.V1.Users;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Extensions.Data
{
    /// <summary>
    /// User Extensions
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Convert a <see cref="User"/>  to a <see cref="UserItem"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A UserItem.</returns>
        public static UserItem ToUserItem(this User user)
        {
            return new UserItem()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }

        /// <summary>
        /// Convert a <see cref="User"/>  to a <see cref="GetUserResponse"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A GetUserResponse.</returns>
        public static GetUserResponse ToGetUserResponse(this User user)
        {
            return new GetUserResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }

        /// <summary>
        /// Convert a <see cref="PostUserRequest"/>  to a <see cref="User"/>.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A Models.User.</returns>
        public static User ToUser(this PostUserRequest postUser)
        {
            return new User()
            {
                FirstName = postUser.FirstName,
                LastName = postUser.LastName,
                Email = postUser.Email,
            };
        }

        /// <summary>
        /// Convert a <see cref="PutUserRequest"/>  to a <see cref="User"/>.
        /// </summary>
        /// <param name="putUser">The PutUserRequest.</param>
        /// <returns>A Models.User.</returns>
        public static User ToUser(this PutUserRequest putUser)
        {
            return new User()
            {
                Id = putUser.Id,
                FirstName = putUser.FirstName,
                LastName = putUser.LastName,
                Email = putUser.Email,
            };
        }
    }
}
