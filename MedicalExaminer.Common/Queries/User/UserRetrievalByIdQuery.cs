namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByIdQuery : IQuery<Models.MeUser>
    {
        public UserRetrievalByIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}
