using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalQuery : IQuery<MeUser>
    {
        public UserRetrievalQuery(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; }
    }
}