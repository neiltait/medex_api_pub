using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserUpdateQuery : IQuery<MeUser>
    {
        public UserUpdateQuery(MeUser user)
        {
            UserId = user.UserId;
            Email = user.Email;
        }

        public string UserId { get; }
        public string Email { get; }
    }
}