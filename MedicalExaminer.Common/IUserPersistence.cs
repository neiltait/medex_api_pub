using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common
{
    public interface IUserPersistence
    {
        Task<MeUser> UpdateUserAsync(MeUser meUser);
        Task<MeUser> CreateUserAsync(MeUser meUser);
        Task<MeUser> GetUserAsync(string UserId);
        Task<IEnumerable<MeUser>> GetUsersAsync();
        Task<IEnumerable<MeUser>> GetMedicalExaminersAsync();
    }
}