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
            CreateMap<MeUser, UserItem>()
                .ForMember(x => x.UserRole, opt => opt.Ignore());
            CreateMap<MeUser, GetUserResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore());
            CreateMap<MeUser, PutUserResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore());
            CreateMap<MeUser, PostUserResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore());
            CreateMap<PostUserRequest, MeUser>();
            CreateMap<PutUserRequest, MeUser>();
            CreateMap<UserItem, MeUser>();

            CreateMap<MEUserPermission, UserPermission>();
        }
    }
}