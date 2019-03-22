using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByIdQuery : IQuery<MeUser>
    {
        public UserRetrievalByIdQuery(string userId)
        {
            userId = userId;
        }

        public string UserId { get; }
    }
}