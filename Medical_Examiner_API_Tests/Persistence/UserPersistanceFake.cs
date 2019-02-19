using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Faker;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;

namespace Medical_Examiner_API_Tests.Persistence
{
    public class UserPersistenceFake : IUserPersistence
    {
        private readonly List<MeUser> _users;

        public UserPersistenceFake()
        {

            // Populate the users fake vars 
            _users = new List<MeUser>();

            for (int count = 0; count < 10; count++)
            {
                MeUser u = new MeUser();
                u.Email = Internet.Email();
                u.UserId = "aaaaa" + count;
                u.FirstName = Name.First();
                u.LastName = Name.Last();
                u.CreatedAt = DateTime.Now;
                u.ModifiedAt = DateTime.Now;

                _users.Add(u);
            }
        }

        public async Task<MeUser> CreateUserAsync(MeUser meUser)
        {
            return await Task.FromResult(meUser);
        }

        public async Task<MeUser> GetUserAsync(string UserId)
        {
            foreach (MeUser user in _users)
            {
                if (user.UserId == UserId)
                {
                    return await Task.FromResult(user);
                }
            }

            throw new ArgumentException("Invalid Argument");
        }

        public async Task<IEnumerable<MeUser>> GetUsersAsync()
        {
            return await Task.FromResult(_users);
        }

        public async Task<MeUser> UpdateUserAsync(MeUser meUser)
        {
            return await Task.FromResult(meUser);
        }
    }
}
