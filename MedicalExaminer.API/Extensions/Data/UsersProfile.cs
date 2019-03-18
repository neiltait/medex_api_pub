using AutoMapper;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Users Profile for AutoMapper
    /// </summary>
    public class UsersProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Users AutoMapper Profile
        /// </summary>
        public UsersProfile()
        {
            CreateMap<MeUser, UserItem>();
            CreateMap<MeUser, GetUserResponse>();
            CreateMap<MeUser, PutUserResponse>();
            CreateMap<MeUser, PostUserResponse>();
            CreateMap<PostUserRequest, MeUser>();
            CreateMap<PutUserRequest, MeUser>();
            CreateMap<UserItem, MeUser>();
        }
    }
}