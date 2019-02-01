using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiners_API.Models;

namespace Medical_Examiners_API.Persistence
{
    public interface IUserPersistence
    {
        Task SaveUserAsync(User user);
        Task<User> GetUserAsync(string Id);
        Task<IEnumerable<User>> GetUsersAsync();

    }
}
