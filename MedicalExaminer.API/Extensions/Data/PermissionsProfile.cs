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
                .ForMember(permissionItem => permissionItem.LocationName, opt => opt.MapFrom(x => x.Location.Name));
            CreateMap<PermissionLocation, GetPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.MapFrom(x => x.Location.Name))
                .ForMember(response => response.PermissionId, opt => opt.MapFrom(x => x.Permission.PermissionId))
                .ForMember(response => response.LocationId, opt => opt.MapFrom(x => x.Permission.LocationId))
                .ForMember(response => response.UserRole, opt => opt.MapFrom(x => x.Permission.UserRole));
            CreateMap<PermissionLocation, PutPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(response => response.LocationName, opt => opt.MapFrom(x => x.Location.Name))
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
                .ForMember(response => response.LocationName, opt => opt.MapFrom(x => x.Location.Name))
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<PostPermissionRequest, MEUserPermission>()
                .ForMember(meUserPermission => meUserPermission.PermissionId, opt => opt.Ignore());
            CreateMap<PutPermissionRequest, MEUserPermission>()
                .ForMember(request => request.PermissionId, opt => opt.Ignore());
        }
    }
}