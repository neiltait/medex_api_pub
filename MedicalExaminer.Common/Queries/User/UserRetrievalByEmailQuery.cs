using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByEmailQuery : IQuery<MeUser>
    {
        public UserRetrievalByEmailQuery(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
