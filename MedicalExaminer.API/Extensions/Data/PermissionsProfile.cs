using AutoMapper;
using MedicalExaminer.API.Models.v1.Permissions;
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
            CreateMap<MEUserPermission, PermissionItem>()
                .ForMember(permissionItem => permissionItem.LocationName, opt => opt.Ignore())
                .ForMember(permissionItem => permissionItem.UserId, opt => opt.Ignore());
            CreateMap<MEUserPermission, GetPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.Ignore())
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<MEUserPermission, PostPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.Ignore())
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<MEUserPermission, PutPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.Ignore())
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());

            CreateMap<PostPermissionRequest, MEUserPermission>()
                .ForMember(meUserPermission => meUserPermission.PermissionId, opt => opt.Ignore());
            CreateMap<PutPermissionRequest, MEUserPermission>()
                .ForMember(request => request.PermissionId, opt => opt.Ignore());
        }
    }
}