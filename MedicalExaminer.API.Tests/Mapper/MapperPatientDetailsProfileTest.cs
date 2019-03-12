using System;
using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    public class MapperPatientDetailsProfileTest
    {
        private const string id = "expectedExaminationId";
        private const string AltLink = "altLink";
        private const bool AnyImplants = true;
        private const bool AnyPersonalEffects = true;
        private const bool ChildPriority = true;
        private const bool Completed = true;
        private const bool CoronerPriority = true;
        private CoronerStatus CoronerStatus = CoronerStatus.SentAwaitingConfirm;
        private const string County = "Cheshire";
        private const string Country = "England";
        private const bool CulturalPriority = true;
        private DateTime DateOfBirth = new DateTime(1990, 2, 24);
        private DateTime DateOfDeath = new DateTime(2019, 2, 24);
        private const string FuneralDirectors = "funeralDirectors";
        private const bool FaithPriority = true;
        private const string GivenNames = "givenNames";
        private ExaminationGender Gender = ExaminationGender.Male;
        private const string GenderDetails = "genderDetails";
        private const string HospitalNumber_1 = "hospitalNumber_1";
        private const string HospitalNumber_2 = "hospitalNumber_2";
        private const string HospitalNumber_3 = "hospitalNumber_3";
        private const string HouseNameNumber = "houseNameNumber";
        private const string ImplantDetails = "implantDetails";
        private const string LastOccupation = "lastOccupation";
        private const string MedicalExaminerOfficeResponsible = "medicalExaminerOfficeResponsible";
        private ModeOfDisposal ModeOfDisposal = ModeOfDisposal.BuriedAtSea;
        private const string NhsNumber = "123456789";
        private const string OrganisationCareBeforeDeathLocationId = "organisationCareBeforeDeathLocationId";
        private const bool OtherPriority = true;
        private const bool OutOfHours = true;
        private const string PersonalEffectDetails = "personalEffectDetails";
        private const string Postcode = "postcode";
        private const string PlaceDeathOccured = "placeDeathOccured";
        private const string PriorityDetails = "priorityDetails";
        private const string Surname = "surname";
        private const string Street = "street";
        private const string Town = "town";
        private TimeSpan TimeOfDeath = new TimeSpan(11, 30, 00);

        private IEnumerable<Representative> Representatives = new List<Representative>()
        {
            new Representative()
            {
                AppointmentDate = new DateTime(2019, 2, 24),
                AppointmentTime = new TimeSpan(11, 30, 0),
                FullName = "fullName",
                Informed = Informed.Yes,
                PhoneNumber = "123456789",
                PresentAtDeath = PresentAtDeath.Yes,
                Relationship = "relationship"
            }
        };
        [Fact]
        public void PutPatientDetailsRequest_To_PatientDetails()
        {
            
        }

        [Fact]
        public void Examination_To_GetPatientDetailsResponse()
        {
            
        }

        [Fact]
        public void PatientDetails_To_Examination()
        {

        }
    }
}
