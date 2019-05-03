using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <inheritdoc />
    public class CreateUserQuery : IQuery<MeUser>
    {
        /// <inheritdoc />
        public CreateUserQuery(MeUser userToCreate, MeUser currentUser)
        {
            UserToCreate = userToCreate;
            CurrentUser = currentUser;
        }

        public MeUser UserToCreate { get; }

        public MeUser CurrentUser { get; }
    }
}