using Medical_Examiner_API;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ME_API_tests.Persistance
{
    public class UserPersistanceFake : IUserPersistence
    {


        public Task<User> GetUserAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveExaminationAsync(Examination examination)
        {
            return await Task.FromResult(true);
        }

        public Task SaveUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
