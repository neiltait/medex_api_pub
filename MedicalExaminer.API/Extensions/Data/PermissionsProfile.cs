using AutoMapper;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.Extensions.DependencyInjection;

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
            //IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service;
            CreateMap<MEUserPermission, PermissionItem>()
                .ForMember(permissionItem => permissionItem.LocationName, opt => opt.Ignore())
                .ForMember(permissionItem => permissionItem.UserId, opt => opt.Ignore());
            CreateMap<MEUserPermission, GetPermissionResponse>()
                .ForMember(response => response.UserId, opt => opt.Ignore())
                .ForMember(response => response.LocationName, opt => opt.MapFrom(new LocationIdLocationNameResolver()))
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

    internal class LocationIdLocationNameResolver : IValueResolver<MEUserPermission, GetPermissionResponse, string>
    {
        IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> _service;
        public LocationIdLocationNameResolver()
        {
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> service;
            //_service = service;
        }
        public string Resolve(MEUserPermission source, GetPermissionResponse destination, string destMember, ResolutionContext context)
        {
            //var locationIdString = source.LocationId;
            //if (string.IsNullOrEmpty(locationIdString))
            //{
            //    return string.Empty;
            //}
            //ServiceProvider sp = new ServiceProvider();
            //var locationService = new LocationIdService()

            //var locationPersistence = (IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>)context.GetService(typeof(IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>));
            //if (locationPersistence == null)
            //{
            //    throw new NullReferenceException("locationPersistence is null");
            //}

            //var validatedLocation = locationPersistence.Handle(new LocationRetrievalByIdQuery(locationString)).Result;

            //return validatedLocation == null
            //    ? new ValidationResult("The location Id has not been found")
            //    : ValidationResult.Success;


            return "LOCATION_NAME";
        }
    }
}