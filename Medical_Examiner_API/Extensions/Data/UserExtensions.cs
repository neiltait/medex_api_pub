using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Users;
using Microsoft.Azure.Documents;

namespace Medical_Examiner_API.Extensions.Data
{
    /// <summary>
    /// User Extensions
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="UserItem"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A UserItem.</returns>
        public static UserItem ToUserItem(this MeUser user)
        {
            return new UserItem
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }

        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="GetUserResponse"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A GetUserResponse.</returns>
        public static GetUserResponse ToGetUserResponse(this MeUser user)
        {
            return new GetUserResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }
        
        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="GetUserResponse"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A GetUserResponse.</returns>
        public static PutUserResponse ToPutUserResponse(this MeUser user)
        {
            return new PutUserResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }
        
        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="PutUserResponse"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A GetUserResponse.</returns>
        public static PostUserResponse ToPostUserResponse(this MeUser user)
        {
            return new PostUserResponse
            {
                UserId = user.UserId,
            };
        }
        
        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="PutUserResponse"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A GetUserResponse.</returns>
        public static PostUserRequest ToPostUserRequest(this MeUser user)
        {
            return new PostUserRequest
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }

        /// <summary>
        /// Convert a <see cref="PostUserRequest"/>  to a <see cref="MeUser"/>.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A Models.User.</returns>
        public static MeUser ToUser(this PostUserRequest postUser)
        {
            return new MeUser
            {
                FirstName = postUser.FirstName,
                LastName = postUser.LastName,
                Email = postUser.Email
            };
        }

        /// <summary>
        /// Convert a <see cref="PutUserRequest"/>  to a <see cref="MeUser"/>.
        /// </summary>
        /// <param name="putUser">The PutUserRequest.</param>
        /// <returns>A Models.User.</returns>
        public static MeUser ToUser(this PutUserRequest putUser)
        {
            return new MeUser
            {
                UserId = putUser.UserId,
                FirstName = putUser.FirstName,
                LastName = putUser.LastName,
                Email = putUser.Email
            };
        }
    }
}
