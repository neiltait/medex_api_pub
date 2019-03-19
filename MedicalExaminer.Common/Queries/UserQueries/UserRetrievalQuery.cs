using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.UserQueries
{
    public class UserRetrievalQuery : IQuery<MeUser>
    {
        public readonly string Id;

        public UserRetrievalQuery(string id)
        {
            Id = id;
        }
    }
}
