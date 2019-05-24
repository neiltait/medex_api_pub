using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Models;
using MedicalExaminer.ToolBox.Common.Dtos;

namespace MedicalExaminer.ToolBox.Common.Services
{
    public class ImpersonateUserService
    {
        private readonly ICosmosStore<MeUser> _userStore;

        public ImpersonateUserService(ICosmosStore<MeUser> userStore)
        {
            _userStore = userStore;
        }

        public async Task<IEnumerable<MeUserItem>> GetUsers()
        {
            var users = await _userStore.Query().ToListAsync();

            return users.Select(u => new MeUserItem
            {
                Id = u.UserId,
                FullName = u.FullName(),
            });
        }

        public async Task Update(string selectedId, string email)
        {
            var users = await _userStore.Query().ToListAsync();

            foreach (var user in users)
            {
                if (user.UserId == selectedId)
                {
                    user.Email = email;
                }
                else
                {
                    user.Email = $"{user.UserId}@example.com";
                }
            }

            await _userStore.UpdateRangeAsync(users);
        }
    }
}
