namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByIdQuery : IQuery<Models.MeUser>
    {
        public string UserId { get; }

        public UserRetrievalByIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
