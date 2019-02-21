using AutoMapper;
using FluentAssertions;
using Medical_Examiner_API.Extensions.Data;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Examinations;
using Xunit;

namespace Medical_Examiner_API_Tests.Mapper
{
    /// <summary>
    /// Mapper Examination Profile Tests
    /// </summary>
    public class MapperExaminationProfileTests
    {
        /// <summary>
        /// Mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Setup
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
                ExaminationId = expectedExaminationId,
            };

            var response = _mapper.Map<GetExaminationResponse>(examination);

            response.ExaminationId.Should().Be(expectedExaminationId);
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
                ExaminationId = expectedExaminationId,
            };

            var response = _mapper.Map<ExaminationItem>(examination);

            response.ExaminationId.Should().Be(expectedExaminationId);
        }
    }
}
