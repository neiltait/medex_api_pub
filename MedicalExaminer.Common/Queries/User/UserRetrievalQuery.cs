namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalQuery : IQuery<Models.MeUser>
    {
        public string UserEmail { get; }

        public UserRetrievalQuery(string userEmail)
        {
            UserEmail = userEmail;
        }
    }
}
