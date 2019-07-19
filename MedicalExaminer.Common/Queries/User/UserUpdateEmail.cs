namespace MedicalExaminer.Common.Queries.User
{
    public class UserUpdateEmail : IUserUpdate
    {
        public string UserId { get; set; }

        public string Email { get; set; }
    }
}
