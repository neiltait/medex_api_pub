using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByIdQuery : IQuery<MeUser>
    {
        public UserRetrievalByIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}
