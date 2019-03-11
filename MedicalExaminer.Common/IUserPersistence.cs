using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common
{
    /// <summary>
    /// Interface for classes that manage persistence of user objects
    /// </summary>
    public interface IUserPersistence
    {
        /// <summary>
        /// Update one user
        /// </summary>
        /// <param name="meUser">User to update</param>
        /// <returns>Updated user</returns>
        Task<MeUser> UpdateUserAsync(MeUser meUser);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="meUser">User to create</param>
        /// <returns>Newly created user</returns>
        Task<MeUser> CreateUserAsync(MeUser meUser);

        /// <summary>
        /// Get user by UserId
        /// </summary>
        /// <param name="UserId">UserId of user to return</param>
        /// <returns>Found user</returns>
        Task<MeUser> GetUserAsync(string UserId);

        /// <summary>
        /// Get user by Email Address
        /// </summary>
        /// <param name="UserId">Email Address of user to return</param>
        /// <returns>Found user</returns>
        Task<MeUser> GetUserByEmailAddressAsync(string emailAddress);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users</returns>
        Task<IEnumerable<MeUser>> GetUsersAsync();

        /// <summary>
        /// Get all Medical Examiners
        /// </summary>
        /// <returns>List of Medical Examiners</returns>
        Task<IEnumerable<MeUser>> GetMedicalExaminersAsync();

        /// <summary>
        /// Get all Medical Examiner Officers
        /// </summary>
        /// <returns>List of all Medical Examiner Officers</returns>
        Task<IEnumerable<MeUser>> GetMedicalExaminerOfficerAsync();
    }
}