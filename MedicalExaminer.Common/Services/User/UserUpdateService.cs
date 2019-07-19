using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Update Service.
    /// </summary>
    public class UserUpdateService : QueryHandler<UserUpdateQuery, MeUser>
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialise a new instance of <see cref="UserUpdateService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">User Connection Settings.</param>
        /// <param name="mapper">Mapper</param>
        public UserUpdateService(
            IDatabaseAccess databaseAccess, 
            IUserConnectionSettings connectionSettings, 
            IMapper mapper)
            : base(databaseAccess, connectionSettings)
        {
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public override async Task<MeUser> Handle(UserUpdateQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var userToUpdate = await GetItemByIdAsync(param.UserUpdate.UserId);

            if (userToUpdate == null)
            {
                throw new InvalidOperationException($"User with id `{param.UserUpdate.UserId}` not found.");
            }

            _mapper.Map(
                param.UserUpdate,
                userToUpdate,
                param.UserUpdate.GetType(),
                typeof(MeUser));

            userToUpdate.LastModifiedBy = param.CurrentUser.UserId;
            userToUpdate.ModifiedAt = DateTime.Now;

            var result = await UpdateItemAsync(userToUpdate);
            return result;
        }
    }
}