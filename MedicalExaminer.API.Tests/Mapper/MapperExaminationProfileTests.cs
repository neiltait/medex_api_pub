using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    /// <summary>
    /// Mapper Examination Profile Tests
    /// </summary>
    public class MapperExaminationProfileTests
    {
        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Setup.
        /// </summary>
        public MapperExaminationProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExaminationProfile>();
            });

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Test Mapping Examination to GetExaminationResponse.
        /// </summary>
        [Fact]
        public void TestGetExaminationResponse()
        {
            var expectedExaminationId = "expectedExaminationId";

            var examination = new Examination()
            {
                Id = expectedExaminationId,
            };

            var response = _mapper.Map<GetExaminationResponse>(examination);

            response.Id.Should().Be(expectedExaminationId);
        }

        /// <summary>
        /// Test Mapping Examination to ExaminationItem.
        /// </summary>
        [Fact]
        public void TestxaminationItem()
        {
            var expectedExaminationId = "expectedExaminationId";

            var examination = new Examination()
            {
                Id = expectedExaminationId,
            };

            var response = _mapper.Map<ExaminationItem>(examination);

            response.Id.Should().Be(expectedExaminationId);
        }
    }
}
