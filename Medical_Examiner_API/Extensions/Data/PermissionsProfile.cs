using AutoMapper;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Examinations;
using Medical_Examiner_API.Models.V1.Users;

namespace Medical_Examiner_API.Extensions.Data
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
