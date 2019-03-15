using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class CreateUserQuery : IQuery<MeUser>
    {
        public MeUser MeUser { get; }

        public CreateUserQuery(MeUser meUser)
        {
            MeUser = meUser;
        }
    }
}
