using System;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    public class MapperNewEventTests
    {
        //public MapperNewEventTests()
        //{
        //    var config = new MapperConfiguration(cfg => { cfg.AddProfile<NewExaminationProfile>(); });

        //    _mapper = config.CreateMapper();
        //}

        private string eventText = "This is a text to test the Event";

        /// <summary>
        ///     Test Mapping Examination to ExaminationItem.
        /// </summary>
        [Fact]
        public void PostNewCaseRequest_To_ExaminationItem()
        {
            var postOtherEventRequest = new PostOtherEventRequest
            {
                EventText = eventText
            };


            //var response = _mapper.Map<ExaminationItem>(postNewCaseRequest);
            //response.GenderDetails.Should().Be(GenderDetails);
            //response.GivenNames.Should().Be(GivenNames);
            //response.DateOfBirth.Should().Be(DateOfBirth);
            //response.DateOfDeath.Should().Be(DateOfDeath);
            //response.Gender.Should().Be(Gender);
            //response.GivenNames.Should().Be(GivenNames);
            //response.HospitalNumber_1.Should().Be(HospitalNumber_1);
            //response.HospitalNumber_2.Should().Be(HospitalNumber_2);
            //response.HospitalNumber_3.Should().Be(HospitalNumber_3);
            //response.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            //response.NhsNumber.Should().Be(NhsNumber);
            //response.OutOfHours.Should().Be(OutOfHours);
            //response.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            //response.Surname.Should().Be(Surname);
            //response.TimeOfDeath.Should().Be(TimeOfDeath);
        }
    }
}