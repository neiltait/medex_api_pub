using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserUpdateQuery : IQuery<MeUser>
    {
        public UserUpdateQuery(string userId, string userEmail)
        {
            UserId = userId;
            UserEmail = userEmail;
        }

        public string UserId { get; }
        public string UserEmail { get; }
    }
}