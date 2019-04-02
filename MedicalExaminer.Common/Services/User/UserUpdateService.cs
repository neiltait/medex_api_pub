using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents.SystemFunctions;

namespace MedicalExaminer.Common.Services.Examination
{
    public class UserUpdateService : IAsyncQueryHandler<UserUpdateQuery, Models.MeUser>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IMapper _mapper;

        public UserUpdateService(
            IDatabaseAccess databaseAccess,
            IUserConnectionSettings connectionSettings,
            IMapper mapper)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _mapper = mapper;
        }

        public async Task<Models.MeUser> Handle(UserUpdateQuery userUpdate)
        {
            if (userUpdate == null)
            {
                throw new ArgumentNullException(nameof(userUpdate));
            }
            
            var userToUpdate = await
                _databaseAccess
                    .GetItemAsync<Models.MeUser>(
                        _connectionSettings,
                        meUser => meUser.UserId == userUpdate.UserId);

            userToUpdate.Email = userUpdate.Email;
            
            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, userToUpdate);
            return result;
        }
    }
}