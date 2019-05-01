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
                .ForMember(x => x.UserId, opt => opt.Ignore());
            CreateMap<MEUserPermission, GetPermissionResponse>()
                .ForMember(x => x.UserId, opt => opt.Ignore())
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());

            CreateMap<Permission, PermissionItem>();
            CreateMap<Permission, GetPermissionResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<Permission, PostPermissionResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<Permission, PutPermissionResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());

            CreateMap<PostPermissionRequest, Permission>()
                .ForMember(x => x.PermissionId, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedAt, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore());
            CreateMap<PutPermissionRequest, Permission>()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedAt, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore());
        }
    }
}