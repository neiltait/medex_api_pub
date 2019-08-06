using AutoMapper;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationItem>();
        }
    }
}
