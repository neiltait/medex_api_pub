using System;
using System.Collections.Generic;
using MedicalExaminer.Models;
using System.Text;

namespace MedicalExaminer.Common.Queries.User
{
    public class UsersUpdateOktaTokenQuery : IQuery<MeUser>
    {
        public UsersUpdateOktaTokenQuery(MeUser user)
        {
            UserId = user.UserId;
            Email = user.Email;
            OktaToken = user.OktaToken;
            OktaTokenExpiry = user.OktaTokenExpiry;
        }

        public string OktaToken { get; }
        public DateTimeOffset OktaTokenExpiry { get; }
        public string UserId { get; }
        public string Email { get; }
    }
}
