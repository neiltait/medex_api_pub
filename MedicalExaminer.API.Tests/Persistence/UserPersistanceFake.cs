using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Faker;
using MedicalExaminer.Common;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Tests.Persistence
{
    public class UserPersistenceFake : IUserPersistence
    {
        private readonly List<MeUser> _users;

        public UserPersistenceFake()
        {
            // Populate the users fake vars 
            _users = new List<MeUser>();

            for (var count = 0; count < 10; count++)
            {
                var u = new MeUser
                {
                    Email = Internet.Email(),
                    UserId = "aaaaa" + count,
                    FirstName = Name.First(),
                    LastName = Name.Last(),
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };

                _users.Add(u);
            }
        }

        public async Task<MeUser> CreateUserAsync(MeUser meUser)
        {
            var createdUser = meUser;
            createdUser.UserId = "1";
            return await Task.FromResult(createdUser);
        }

        public async Task<MeUser> GetUserAsync(string UserId)
        {
            foreach (var user in _users)
                if (user.UserId == UserId)
                    return await Task.FromResult(user);

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

        public async Task<IEnumerable<MeUser>> GetMedicalExaminersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MeUser>> GetMedicalExaminerOfficerAsync()
        {
            throw new NotImplementedException();
        }
    }
}