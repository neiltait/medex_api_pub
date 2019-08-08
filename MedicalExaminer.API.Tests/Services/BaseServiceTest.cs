using AutoMapper;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using Moq;

namespace MedicalExaminer.API.Tests.Services
{
    public class BaseServiceTest
    {
        public BaseServiceTest()
        {
            var service = new Mock<IAsyncQueryHandler<LocationRetrievalByIdQuery, MedicalExaminer.Models.Location>>();
            var mapperConfiguration = new MapperConfiguration(config => { config.AddMedicalExaminerProfiles(); });

            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
