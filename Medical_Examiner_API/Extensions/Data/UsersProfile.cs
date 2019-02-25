using AutoMapper;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Users;

namespace Medical_Examiner_API.Extensions.Data
{
    /// <summary>
    /// Users Profile for AutoMapper
    /// </summary>
    public class UsersProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of the Users AutoMapper Profile
        /// </summary>
        public UsersProfile()
        {
            CreateMap<MeUser, UserItem>();
            CreateMap<MeUser, GetUserResponse>();
            CreateMap<MeUser, PutUserResponse>();
            CreateMap<MeUser, PostUserResponse>();

            CreateMap<PostUserRequest, MeUser>();
            CreateMap<PutUserRequest, MeUser>();
        }
    }
}
