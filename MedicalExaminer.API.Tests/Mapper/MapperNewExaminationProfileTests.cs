using System;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    public class MapperNewExaminationProfileTests
    {
        public MapperNewExaminationProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<NewExaminationProfile>(); });

            _mapper = config.CreateMapper();
        }

        private CoronerStatus CoronerStatus = CoronerStatus.SentAwaitingConfirm;
        private readonly DateTime DateOfBirth = new DateTime(1990, 2, 24);
        private readonly DateTime DateOfDeath = new DateTime(2019, 2, 24);
        private const string GivenNames = "givenNames";
        private readonly ExaminationGender Gender = ExaminationGender.Male;
        private const string GenderDetails = "genderDetails";
        private const string HospitalNumber_1 = "hospitalNumber_1";
        private const string HospitalNumber_2 = "hospitalNumber_2";
        private const string HospitalNumber_3 = "hospitalNumber_3";
        private const string MedicalExaminerOfficeResponsible = "medicalExaminerOfficeResponsible";
        private ModeOfDisposal ModeOfDisposal = ModeOfDisposal.BuriedAtSea;
        private const string NhsNumber = "123456789";
        private const bool OutOfHours = true;
        private const string PlaceDeathOccured = "placeDeathOccured";
        private const string Surname = "surname";
        private readonly TimeSpan TimeOfDeath = new TimeSpan(11, 30, 00);
        private readonly IMapper _mapper;

        /// <summary>
        ///     Test Mapping Examination to ExaminationItem.
        /// </summary>
        [Fact]
        public void PostNewCaseRequest_To_ExaminationItem()
        {
            var postNewCaseRequest = new PostExaminationRequest
            {
                DateOfDeath = DateOfDeath,
                DateOfBirth = DateOfBirth,
                GivenNames = GivenNames,
                Gender = Gender,
                GenderDetails = GenderDetails,
                HospitalNumber_1 = HospitalNumber_1,
                HospitalNumber_2 = HospitalNumber_2,
                HospitalNumber_3 = HospitalNumber_3,
                MedicalExaminerOfficeResponsible = MedicalExaminerOfficeResponsible,
                NhsNumber = NhsNumber,
                OutOfHours = OutOfHours,
                PlaceDeathOccured = PlaceDeathOccured,
                Surname = Surname,
                TimeOfDeath = TimeOfDeath
            };


            var response = _mapper.Map<ExaminationItem>(postNewCaseRequest);
            response.GenderDetails.Should().Be(GenderDetails);
            response.GivenNames.Should().Be(GivenNames);
            response.DateOfBirth.Should().Be(DateOfBirth);
            response.DateOfDeath.Should().Be(DateOfDeath);
            response.Gender.Should().Be(Gender);
            response.GivenNames.Should().Be(GivenNames);
            response.HospitalNumber_1.Should().Be(HospitalNumber_1);
            response.HospitalNumber_2.Should().Be(HospitalNumber_2);
            response.HospitalNumber_3.Should().Be(HospitalNumber_3);
            response.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            response.NhsNumber.Should().Be(NhsNumber);
            response.OutOfHours.Should().Be(OutOfHours);
            response.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            response.Surname.Should().Be(Surname);
            response.TimeOfDeath.Should().Be(TimeOfDeath);
        }
    }
}