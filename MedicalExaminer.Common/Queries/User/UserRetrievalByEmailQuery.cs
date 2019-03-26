using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByEmailQuery : IQuery<Models.MeUser>
    {
        public UserRetrievalByEmailQuery(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; }
    }
}