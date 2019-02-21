using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Persistence
{
    public interface IUserPersistence
    {
        Task<MeUser> UpdateUserAsync(MeUser meUser);
        Task<MeUser> CreateUserAsync(MeUser meUser);
        Task<MeUser> GetUserAsync(string UserId);
        Task<IEnumerable<MeUser>> GetUsersAsync();
    }
}