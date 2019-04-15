using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <inheritdoc />
    public class CreateUserQuery : IQuery<MeUser>
    {
        /// <inheritdoc />
        public CreateUserQuery(MeUser meUser)
        {
            MeUser = meUser;
        }

        public MeUser MeUser { get; }
    }
}