using MedicalExaminer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByOktaTokenQuery : IQuery<MeUser>
    {
        public UserRetrievalByOktaTokenQuery(MeUser user)
        {
            OktaToken = user.OktaToken;
            OktaTokenExpiry = user.OktaTokenExpiry;
        }

        public string OktaToken { get; }
        public DateTimeOffset OktaTokenExpiry { get; }
    }
}
