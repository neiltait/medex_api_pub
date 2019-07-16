using AutoMapper;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
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
                .ForMember(response => response.Lookups, opt => opt.Ignore());
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
                .ForMember(meUser => meUser.UserId, opt => opt.MapFrom(request => request.UserId))
                .ForMember(meUser => meUser.FirstName, opt => opt.MapFrom(request => request.FirstName))
                .ForMember(meUser => meUser.LastName, opt => opt.MapFrom(request => request.LastName))
                .ForMember(meUser => meUser.Email, opt => opt.MapFrom(request => request.Email))
                .ForAllOtherMembers(request => request.Ignore());
            //CreateMap<MEUserPermission, UserPermission>()
            //    .ForMember(userPermission => userPermission.UserRole, opt => opt.MapFrom(meUserPermission => meUserPermission.UserRole))
            //    .ForMember(userPermission => userPermission.LocationId, opt => opt.MapFrom(meUserPermission => meUserPermission.LocationId))
            //    .ForMember(userPermission => userPermission.LocationName, opt => opt.MapFrom(new UserPermissionLocationIdLocationNameResolver(service)))
            //    .ForMember(userPermission => userPermission.PermissionId, opt => opt.MapFrom(meUserPermission => meUserPermission.PermissionId))
            //    .ForAllOtherMembers(meUserPermission => meUserPermission.Ignore());
        }
    }

    public class UserPermissionLocationIdLocationNameResolver : IValueResolver<MEUserPermission, UserPermission, string>
    {
        IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> _service;
        public UserPermissionLocationIdLocationNameResolver(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service)
        {
            _service = service;
        }
        public string Resolve(MEUserPermission source, UserPermission destination, string destMember, ResolutionContext context)
        {
            var locationIdString = source.LocationId;
            if (string.IsNullOrEmpty(locationIdString))
            {
                return string.Empty;
            }

            var validatedLocation = _service.Handle(new LocationRetrievalByIdQuery(locationIdString)).Result;

            return validatedLocation.Name == null
                ? string.Empty
                : validatedLocation.Name;
        }
    }
}