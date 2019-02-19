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
        private List<User> users;

        public UserPersistanceFake()
        {

            // Populate the users fake vars 
            users = new List<User>();

            for (int count = 0; count < 10; count++)
            {
                User u = new User();
                u.Email = Internet.Email();
                u.UserId = "aaaaa" + count.ToString();
                u.FirstName = Name.First();
                u.LastName = Name.Last();
                u.CreatedAt = DateTime.Now;
                u.ModifiedAt = DateTime.Now;

                users.Add(u);
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await Task.FromResult(user);
        }

        public async Task<User> GetUserAsync(string UserId)
        {
            foreach (User user in users)
            {
                if (user.UserId == UserId)
                {
                    return await Task.FromResult(user);
                }
            }

            throw new ArgumentException("Invalid Argument");
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await Task.FromResult(users);
        }

        public async Task<User> SaveUserAsync(User user)
        {
            return await Task.FromResult(user);
        }
    }
}
