using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByEmailQuery : IQuery<MeUser>
    {
        public UserRetrievalByEmailQuery(string EmailAddress)
        {
            Email = EmailAddress;
        }

        public string Email { get; }
    }
}