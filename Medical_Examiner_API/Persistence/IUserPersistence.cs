using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Persistence
{
    public interface IUserPersistence
    {
        Task<MEUser> UpdateUserAsync(MEUser meUser);
        Task<MEUser> CreateUserAsync(MEUser meUser);
        Task<MEUser> GetUserAsync(string UserId);
        Task<IEnumerable<MEUser>> GetUsersAsync();

    }
}
