using AutoMapper;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Extensions.MeUser;
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
            CreateMap<MeUser, UserLookup>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FullName()));
            CreateMap<MeUser, GetUserResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<MeUser, PutUserResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<MeUser, PostUserResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<PostUserRequest, MeUser>()
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<PutUserRequest, MeUser>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName))
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<MEUserPermission, UserPermission>()
                .ForMember(x => x.UserRole, opt => opt.MapFrom(x => x.UserRole))
                .ForMember(x => x.LocationId, opt => opt.MapFrom(x => x.LocationId))
                .ForMember(x => x.PermissionId, opt => opt.MapFrom(x => x.PermissionId))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}