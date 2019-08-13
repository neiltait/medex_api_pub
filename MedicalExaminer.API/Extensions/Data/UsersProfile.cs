using AutoMapper;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.User;
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
            CreateMap<MeUser, UserLookup>()
                .ForMember(userLookup => userLookup.FullName, opt => opt.MapFrom(meUser => meUser.FullName()));
            CreateMap<MeUser, GetUserResponse>()
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore())
                .ForMember(response => response.Permissions, opt => opt.Ignore());
            CreateMap<MeUser, PutUserResponse>()
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<MeUser, PostUserResponse>()
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<PostUserRequest, MeUser>()
                .ForMember(meUser => meUser.Email, opt => opt.MapFrom(request => request.Email))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<PutUserRequest, MeUser>()
                .ForMember(meUser => meUser.UserId, opt => opt.Ignore())
                .ForMember(meUser => meUser.UserId, opt => opt.Ignore())
                .ForMember(meUser => meUser.Email, opt => opt.MapFrom(request => request.Email))
                .ForAllOtherMembers(request => request.Ignore());
            CreateMap<UserUpdatePermissions, MeUser>()
                .ForMember(meUser => meUser.UserId, opt => opt.MapFrom(request => request.UserId))
                .ForMember(meUser => meUser.Permissions, opt => opt.MapFrom(request => request.Permissions))
                .ForAllOtherMembers(meUser => meUser.Ignore());
            CreateMap<UserUpdateEmail, MeUser>()
                .ForMember(meUser => meUser.UserId, opt => opt.MapFrom(request => request.UserId))
                .ForMember(meUser => meUser.Email, opt => opt.MapFrom(request => request.Email))
                .ForAllOtherMembers(meUser => meUser.Ignore());
            CreateMap<UserUpdateOktaQuery, MeUser>()
                .ForMember(meUser => meUser.UserId, opt => opt.MapFrom(request => request.UserId))
                .ForMember(meUser => meUser.OktaId, opt => opt.MapFrom(request => request.OktaId))
                .ForAllOtherMembers(meUser => meUser.Ignore());
            CreateMap<UserUpdateDetails, MeUser>()
                .ForMember(meUser => meUser.UserId, opt => opt.MapFrom(request => request.UserId))
                .ForMember(meUser => meUser.Email, opt => opt.MapFrom(request => request.Email))
                .ForMember(meUser => meUser.FirstName, opt => opt.MapFrom(request => request.FirstName))
                .ForMember(meUser => meUser.LastName, opt => opt.MapFrom(request => request.LastName))
                .ForAllOtherMembers(meUser => meUser.Ignore());
            CreateMap<UserUpdateFull, MeUser>()
                .ForMember(meUser => meUser.UserId, opt => opt.MapFrom(request => request.UserId))
                .ForMember(meUser => meUser.OktaId, opt => opt.MapFrom(request => request.OktaId))
                .ForMember(meUser => meUser.Email, opt => opt.MapFrom(request => request.Email))
                .ForMember(meUser => meUser.FirstName, opt => opt.MapFrom(request => request.FirstName))
                .ForMember(meUser => meUser.LastName, opt => opt.MapFrom(request => request.LastName))
                .ForAllOtherMembers(meUser => meUser.Ignore());
            CreateMap<PutUserRequest, UserUpdateEmail>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}