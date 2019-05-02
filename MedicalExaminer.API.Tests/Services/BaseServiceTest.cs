using AutoMapper;
using MedicalExaminer.API.Extensions.Data;

namespace MedicalExaminer.API.Tests.Services
{
    public class BaseServiceTest
    {
        public BaseServiceTest()
        {
            var mapperConfiguration = new MapperConfiguration(config => { config.AddMedicalExaminerProfiles(); });

            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        public IMapper Mapper { get; }
    }
}
