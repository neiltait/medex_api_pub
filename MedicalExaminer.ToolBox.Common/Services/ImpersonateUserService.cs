using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.ToolBox.Common.Dtos;

namespace MedicalExaminer.ToolBox.Common.Services
{
    public class ImpersonateUserService
    {
        private IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userRetrievalByIdService;
        private IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> _usersRetrievalService;
        private IAsyncQueryHandler<UserUpdateQuery, MeUser> _userUpdateService;

        public ImpersonateUserService(
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalByIdService,
            IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> usersRetrievalService,
            IAsyncQueryHandler<UserUpdateQuery, MeUser> userUpdateService)
        {
            _userRetrievalByIdService = userRetrievalByIdService;
            _usersRetrievalService = usersRetrievalService;
            _userUpdateService = userUpdateService;
        }

        public async Task<IEnumerable<MeUserItem>> GetUsers()
        {
            var users = await _usersRetrievalService.Handle(new UsersRetrievalQuery(null));

            return users.Select(u => new MeUserItem
            {
                Id = u.UserId,
                FullName = u.FullName(),
            });
        }

        public async Task Update(string selectedId, string email)
        {
            var users = await _usersRetrievalService.Handle(new UsersRetrievalQuery(null));

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

                await _userUpdateService.Handle(new UserUpdateQuery(user));
            }
        }
    }
}
