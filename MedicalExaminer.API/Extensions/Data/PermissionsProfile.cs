using System.Linq;
using AutoMapper;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <inheritdoc />
    /// <summary>
    ///     Permissions Profile for AutoMapper
    /// </summary>
    public class PermissionsProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Permissions AutoMapper Profile.
        /// </summary>
        public PermissionsProfile()
        {
            CreateMap<PermissionLocation, PermissionItem>()
                .ForMember(permissionItem => permissionItem.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(permissionItem => permissionItem.PermissionId, opt => opt.MapFrom(x => x.Permission.PermissionId))
                .ForMember(permissionItem => permissionItem.LocationId, opt => opt.MapFrom(x => x.Permission.LocationId))
                .ForMember(permissionItem => permissionItem.UserRole, opt => opt.MapFrom(x => x.Permission.UserRole))
                .ForMember(permissionItem => permissionItem.LocationName, opt => opt.MapFrom(new PermissionLocationPermissionItemResolver()));
            CreateMap<PermissionLocation, GetPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.MapFrom(new PermissionLocationGetPermissionResponseResolver()))
                .ForMember(response => response.PermissionId, opt => opt.MapFrom(x => x.Permission.PermissionId))
                .ForMember(response => response.LocationId, opt => opt.MapFrom(x => x.Permission.LocationId))
                .ForMember(response => response.UserRole, opt => opt.MapFrom(x => x.Permission.UserRole));
            CreateMap<PermissionLocation, PutPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(response => response.LocationName, opt => opt.MapFrom(new PermissionLocationPutPermissionResponseResolver()))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore())
                .ForMember(response => response.PermissionId, opt => opt.MapFrom(x => x.Permission.PermissionId))
                .ForMember(response => response.LocationId, opt => opt.MapFrom(x => x.Permission.LocationId))
                .ForMember(response => response.UserRole, opt => opt.MapFrom(x => x.Permission.UserRole));
            CreateMap<PermissionLocation, PostPermissionResponse>()
                .ForMember(response => response.PermissionId, opt => opt.MapFrom(x => x.Permission.PermissionId))
                .ForMember(response => response.LocationId, opt => opt.MapFrom(x => x.Permission.LocationId))
                .ForMember(response => response.UserRole, opt => opt.MapFrom(x => x.Permission.UserRole))
                .ForMember(response => response.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.MapFrom(new PermissionLocationPostPermissionResponseResolver()))
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<PostPermissionRequest, MEUserPermission>()
                .ForMember(meUserPermission => meUserPermission.PermissionId, opt => opt.Ignore());
            CreateMap<PutPermissionRequest, MEUserPermission>()
                .ForMember(request => request.PermissionId, opt => opt.Ignore());
        }
    }

    public class PermissionLocationPermissionItemResolver : IValueResolver<PermissionLocation, PermissionItem, string>
    {
        public string Resolve(PermissionLocation source, PermissionItem destination, string destMember, ResolutionContext context)
        {
            return source.Locations
                .SingleOrDefault(location => location.LocationId == source.Permission.LocationId).Name;
        }
    }

    public class PermissionLocationGetPermissionResponseResolver : IValueResolver<PermissionLocation, GetPermissionResponse, string>
    {
        public string Resolve(PermissionLocation source, GetPermissionResponse destination, string destMember, ResolutionContext context)
        {
            return source.Locations
                .SingleOrDefault(location => location.LocationId == source.Permission.LocationId).Name;
        }
    }

    public class PermissionLocationPutPermissionResponseResolver : IValueResolver<PermissionLocation, PutPermissionResponse, string>
    {
        public string Resolve(PermissionLocation source, PutPermissionResponse destination, string destMember, ResolutionContext context)
        {
            return source.Locations
                .SingleOrDefault(location => location.LocationId == source.Permission.LocationId).Name;
        }
    }

    public class PermissionLocationPostPermissionResponseResolver : IValueResolver<PermissionLocation, PostPermissionResponse, string>
    {
        public string Resolve(PermissionLocation source, PostPermissionResponse destination, string destMember, ResolutionContext context)
        {

            return source.Locations
                .SingleOrDefault(location => location.LocationId == source.Permission.LocationId).Name;
        }
    }
}