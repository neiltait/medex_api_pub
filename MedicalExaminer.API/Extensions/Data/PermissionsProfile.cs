using AutoMapper;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
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
        /// 

        public PermissionsProfile(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service)
        {
            CreateMap<MEUserPermission, PermissionItem>()
                .ForMember(permissionItem => permissionItem.LocationName, opt => opt.MapFrom(new PermissionItemLocationIdLocationNameResolver(service)))
                .ForMember(permissionItem => permissionItem.UserId, opt => opt.Ignore());
            CreateMap<MEUserPermission, GetPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.MapFrom(new GetPermissionResponseLocationIdLocationNameResolver(service)))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<MEUserPermission, PostPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.MapFrom(new PostPermissionResponseLocationIdLocationNameResolver(service)))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<MEUserPermission, PutPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.MapFrom(new PutPermissionResponseLocationIdLocationNameResolver(service)))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());

            CreateMap<PostPermissionRequest, MEUserPermission>()
                .ForMember(meUserPermission => meUserPermission.PermissionId, opt => opt.Ignore());
            CreateMap<PutPermissionRequest, MEUserPermission>()
                .ForMember(request => request.PermissionId, opt => opt.Ignore());
        }
    }

    public class GetPermissionResponseLocationIdLocationNameResolver : IValueResolver<MEUserPermission, GetPermissionResponse, string>
    {
        IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> _service;

        public GetPermissionResponseLocationIdLocationNameResolver(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service)
        {
            _service = service;
        }

        public string Resolve(MEUserPermission source, GetPermissionResponse destination, string destMember, ResolutionContext context)
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

    public class PermissionItemLocationIdLocationNameResolver : IValueResolver<MEUserPermission, PermissionItem, string>
    {
        IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> _service;

        public PermissionItemLocationIdLocationNameResolver(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service)
        {
            _service = service;
        }

        public string Resolve(MEUserPermission source, PermissionItem destination, string destMember, ResolutionContext context)
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

    public class PostPermissionResponseLocationIdLocationNameResolver : IValueResolver<MEUserPermission, PostPermissionResponse, string>
    {
        IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> _service;

        public PostPermissionResponseLocationIdLocationNameResolver(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service)
        {
            _service = service;
        }

        public string Resolve(MEUserPermission source, PostPermissionResponse destination, string destMember, ResolutionContext context)
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

    public class PutPermissionResponseLocationIdLocationNameResolver : IValueResolver<MEUserPermission, PutPermissionResponse, string>
    {
        IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> _service;

        public PutPermissionResponseLocationIdLocationNameResolver(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service)
        {
            _service = service;
        }

        public string Resolve(MEUserPermission source, PutPermissionResponse destination, string destMember, ResolutionContext context)
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