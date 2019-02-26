using AutoMapper;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Permissions Profile for AutoMapper
    /// </summary>
    public class PermissionsProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of the Permissions AutoMapper Profile.
        /// </summary>
        public PermissionsProfile()
        {
            CreateMap<Permission, PermissionItem>();
            CreateMap<Permission, GetPermissionResponse>();
            CreateMap<Permission, PostPermissionResponse>();
            CreateMap<Permission, PutPermissionResponse>();

            CreateMap<PostPermissionRequest, Permission>();
            CreateMap<PutPermissionRequest, Permission>();
        }
    }
}
