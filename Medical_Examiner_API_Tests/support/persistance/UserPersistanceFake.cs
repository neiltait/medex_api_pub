using Medical_Examiner_API;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Faker;

namespace ME_API_tests.Persistance
{
    public class UserPersistanceFake : IUserPersistence
    {
        private List<MEUser> users;

        public UserPersistanceFake()
        {

            // Populate the users fake vars 
            users = new List<MEUser>();

            for (int count = 0; count < 10; count++)
            {
                MEUser u = new MEUser();
                u.Email = Internet.Email();
                u.MEUserId = "aaaaa" + count.ToString();
                u.FirstName = Name.First();
                u.LastName = Name.Last();
                u.CreatedAt = DateTime.Now;
                u.ModifiedAt = DateTime.Now;

                users.Add(u);
            }
        }

        public async Task<MEUser> CreateUserAsync(MEUser meUser)
        {
            return await Task.FromResult(meUser);
        }

        public async Task<MEUser> GetUserAsync(string UserId)
        {
            foreach (MEUser user in users)
            {
                if (user.MEUserId == UserId)
                {
                    return await Task.FromResult(user);
                }
            }

            throw new ArgumentException("Invalid Argument");
        }

        public async Task<IEnumerable<MEUser>> GetUsersAsync()
        {
            return await Task.FromResult(users);
        }

        public async Task<MEUser> UpdateUserAsync(MEUser meUser)
        {
            return await Task.FromResult(meUser);
        }
    }
}
