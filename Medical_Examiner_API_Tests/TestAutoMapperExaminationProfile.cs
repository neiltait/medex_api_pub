using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FluentAssertions;
using Medical_Examiner_API.Extensions.Data;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Examinations;
using Xunit;

namespace Medical_Examiner_API_Tests
{
    public class TestAutoMapperExaminationProfile
    {
        private readonly IMapper _mapper;

        public TestAutoMapperExaminationProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExaminationProfile>();
            });

            _mapper = config.CreateMapper();
        }

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
    }
}
