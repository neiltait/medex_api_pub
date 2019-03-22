using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Configuration;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    /// <summary>
    /// Mapper Examination Profile Tests
    /// </summary>
    public class MapperExaminationProfileTests
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
        private const int UrgencyScore = 4;
        private DateTime CaseCreated = new DateTime(2019, 3, 15);
        private DateTime LastAdmission = new DateTime(2019, 1, 15);
        private TimeSpan TimeOfDeath = new TimeSpan(11, 30, 00);
        private const string CaseOfficer = "CaseOfficer";

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
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

       
        [Fact]
        public void Examination_To_PatientCard_NullAppointments()
        {
            var examination = GenerateExamination();

            var result = _mapper.Map<PatientCardItem>(examination);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.id.Should().Be(id);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(null);
            result.AppointmentTime.Should().Be(null);
            result.LastAdmission.Should().Be(LastAdmission);
            result.CaseCreatedDate.Should().Be(CaseCreated);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.CaseOfficer.Should().Be(CaseOfficer);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutstandingOutcomes.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);
        }

        [Fact]
        public void Examination_To_PatientCard_One_Representative_Null_Appointment_Details()
        {
            var representative = new Representative()
            {
                AppointmentDate = null,
                AppointmentTime = null,
                FullName = "bob",
                Informed = Informed.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = GenerateExamination();

            examination.Representatives = new[] { representative };
            
            var result = _mapper.Map<PatientCardItem>(examination);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.id.Should().Be(id);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(null);
            result.AppointmentTime.Should().Be(null);
            result.LastAdmission.Should().Be(LastAdmission);
            result.CaseCreatedDate.Should().Be(CaseCreated);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.CaseOfficer.Should().Be(CaseOfficer);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutstandingOutcomes.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);
        }

        [Fact]
        public void Examination_To_PatientCard_One_Representative_Appointment_Details()
        {
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representative = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = Informed.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = new Examination()
            {
                Id = id,
                AnyImplants = AnyImplants,
                AnyPersonalEffects = AnyPersonalEffects,
                ChildPriority = ChildPriority,
                Completed = Completed,
                CoronerPriority = CoronerPriority,
                CoronerStatus = CoronerStatus,
                County = County,
                Country = Country,
                CulturalPriority = CulturalPriority,
                DateOfBirth = DateOfBirth,
                DateOfDeath = DateOfDeath,
                FuneralDirectors = FuneralDirectors,
                FaithPriority = FaithPriority,
                GivenNames = GivenNames,
                Gender = Gender,
                GenderDetails = GenderDetails,
                HospitalNumber_1 = HospitalNumber_1,
                HospitalNumber_2 = HospitalNumber_2,
                HospitalNumber_3 = HospitalNumber_3,
                HouseNameNumber = HouseNameNumber,
                ImplantDetails = ImplantDetails,
                LastOccupation = LastOccupation,
                MedicalExaminerOfficeResponsible = MedicalExaminerOfficeResponsible,
                ModeOfDisposal = ModeOfDisposal,
                NhsNumber = NhsNumber,
                OrganisationCareBeforeDeathLocationId = OrganisationCareBeforeDeathLocationId,
                OtherPriority = OtherPriority,
                OutOfHours = OutOfHours,
                PersonalEffectDetails = PersonalEffectDetails,
                Postcode = Postcode,
                PlaceDeathOccured = PlaceDeathOccured,
                PriorityDetails = PriorityDetails,
                Representatives = new[] { representative },
                Surname = Surname,
                Street = Street,
                Town = Town,
                TimeOfDeath = TimeOfDeath,
                UrgencyScore = UrgencyScore,
                LastAdmission = LastAdmission,
                CaseCreated = CaseCreated
            };

            var result = _mapper.Map<PatientCardItem>(examination);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.id.Should().Be(id);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(appointmentDate);
            result.AppointmentTime.Should().Be(appointmentTime);
            result.LastAdmission.Should().Be(LastAdmission);
            result.CaseCreatedDate.Should().Be(CaseCreated);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.CaseOfficer.Should().Be(CaseOfficer);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutstandingOutcomes.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);
        }

        [Fact]
        public void Examination_To_PatientCard_Two_Representatives_One_Null_One_Complete_Appointment_Details()
        {
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representativeOne = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = Informed.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var representativeTwo = new Representative()
            {
                AppointmentDate = null,
                AppointmentTime = null,
                FullName = "bob",
                Informed = Informed.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };


            var examination = GenerateExamination();
            examination.Representatives = new[] { representativeTwo, representativeOne };

            var result = _mapper.Map<PatientCardItem>(examination);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.id.Should().Be(id);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(appointmentDate);
            result.AppointmentTime.Should().Be(appointmentTime);
            result.LastAdmission.Should().Be(LastAdmission);
            result.CaseCreatedDate.Should().Be(CaseCreated);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.CaseOfficer.Should().Be(CaseOfficer);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutstandingOutcomes.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);
        }

        [Fact]
        public void Examination_To_PatientCard_Two_Representatives_Both_Appointments_In_Past_Complete_Appointment_Details()
        {
            var appointmentDate1 = DateTime.Now.AddDays(-1);
            var appointmentDate2 = DateTime.Now.AddDays(-2);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representativeOne = new Representative()
            {
                AppointmentDate = appointmentDate1,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = Informed.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var representativeTwo = new Representative()
            {
                AppointmentDate = appointmentDate2,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = Informed.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };


            var examination = GenerateExamination();
            examination.Representatives = new[] { representativeTwo, representativeOne };
                
            var result = _mapper.Map<PatientCardItem>(examination);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.id.Should().Be(id);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(null);
            result.AppointmentTime.Should().Be(null);
            result.LastAdmission.Should().Be(LastAdmission);
            result.CaseCreatedDate.Should().Be(CaseCreated);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.CaseOfficer.Should().Be(CaseOfficer);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutstandingOutcomes.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);

        }

        [Fact]
        public void Examination_To_PatientCard_Two_Representatives_One_Appointment_In_Past_Complete_Appointment_Details()
        {
            var appointmentDate1 = DateTime.Now.AddDays(1);
            var appointmentDate2 = DateTime.Now.AddDays(-2);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representativeOne = new Representative()
            {
                AppointmentDate = appointmentDate1,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = Informed.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var representativeTwo = new Representative()
            {
                AppointmentDate = appointmentDate2,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = Informed.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };


            var examination = GenerateExamination();
            examination.Representatives = new[] { representativeTwo, representativeOne };

            var result = _mapper.Map<PatientCardItem>(examination);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.id.Should().Be(id);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(appointmentDate1);
            result.AppointmentTime.Should().Be(appointmentTime);
            result.LastAdmission.Should().Be(LastAdmission);
            result.CaseCreatedDate.Should().Be(CaseCreated);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.CaseOfficer.Should().Be(CaseOfficer);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutstandingOutcomes.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);
        }


        [Fact]
        public void Examination_To_GetExaminationResponse()
        {
            var examination = GenerateExamination();

            var result = _mapper.Map<GetExaminationResponse>(examination);

            result.AnyPersonalEffects.Should().Be(AnyPersonalEffects);
            result.CoronerPriority.Should().Be(CoronerPriority);
            result.ChildPriority.Should().Be(ChildPriority);
            result.Completed.Should().Be(Completed);
            result.CoronerStatus.Should().Be(CoronerStatus);
            result.Country.Should().Be(Country);
            result.County.Should().Be(County);
            result.CulturalPriority.Should().Be(CulturalPriority);
            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.FaithPriority.Should().Be(FaithPriority);
            result.FuneralDirectors.Should().Be(FuneralDirectors);
            result.GivenNames.Should().Be(GivenNames);
            result.Gender.Should().Be(Gender);
            result.GenderDetails.Should().Be(GenderDetails);
            result.HospitalNumber_1.Should().Be(HospitalNumber_1);
            result.HospitalNumber_2.Should().Be(HospitalNumber_2);
            result.HospitalNumber_3.Should().Be(HospitalNumber_3);
            result.HouseNameNumber.Should().Be(HouseNameNumber);
            result.LastOccupation.Should().Be(LastOccupation);
            result.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            result.ModeOfDisposal.Should().Be(ModeOfDisposal);
            result.NhsNumber.Should().Be(NhsNumber);
            result.OutOfHours.Should().Be(OutOfHours);
            result.OrganisationCareBeforeDeathLocationId.Should().Be(OrganisationCareBeforeDeathLocationId);
            result.OtherPriority.Should().Be(OtherPriority);
            result.Postcode.Should().Be(Postcode);
            result.PersonalEffectDetails.Should().Be(PersonalEffectDetails);
            result.PriorityDetails.Should().Be(PriorityDetails);
            result.Street.Should().Be(Street);
            result.Surname.Should().Be(Surname);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.Town.Should().Be(Town);
            Assert.True(result.Representatives.SequenceEqual(Representatives));
        }

        [Fact]
        public void PostNewCaseRequest_To_Examination()
        {
            var postNewCaseRequest = new PostNewCaseRequest()
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
                TimeOfDeath = TimeOfDeath,
            };

            var result = _mapper.Map<Examination>(postNewCaseRequest);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.GivenNames.Should().Be(GivenNames);
            result.Gender.Should().Be(Gender);
            result.GenderDetails.Should().Be(GenderDetails);
            result.HospitalNumber_1.Should().Be(HospitalNumber_1);
            result.HospitalNumber_2.Should().Be(HospitalNumber_2);
            result.HospitalNumber_3.Should().Be(HospitalNumber_3);
            result.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            result.NhsNumber.Should().Be(NhsNumber);
            result.OutOfHours.Should().Be(OutOfHours);
            result.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            result.Surname.Should().Be(Surname);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
        }

       

        private Examination GenerateExamination()
        {
            var examination = new Examination()
            {
                Id = id,
                AnyImplants = AnyImplants,
                AnyPersonalEffects = AnyPersonalEffects,
                ChildPriority = ChildPriority,
                Completed = Completed,
                CoronerPriority = CoronerPriority,
                CoronerStatus = CoronerStatus,
                County = County,
                Country = Country,
                CulturalPriority = CulturalPriority,
                DateOfBirth = DateOfBirth,
                DateOfDeath = DateOfDeath,
                FuneralDirectors = FuneralDirectors,
                FaithPriority = FaithPriority,
                GivenNames = GivenNames,
                Gender = Gender,
                GenderDetails = GenderDetails,
                HospitalNumber_1 = HospitalNumber_1,
                HospitalNumber_2 = HospitalNumber_2,
                HospitalNumber_3 = HospitalNumber_3,
                HouseNameNumber = HouseNameNumber,
                ImplantDetails = ImplantDetails,
                LastOccupation = LastOccupation,
                MedicalExaminerOfficeResponsible = MedicalExaminerOfficeResponsible,
                ModeOfDisposal = ModeOfDisposal,
                NhsNumber = NhsNumber,
                OrganisationCareBeforeDeathLocationId = OrganisationCareBeforeDeathLocationId,
                OtherPriority = OtherPriority,
                OutOfHours = OutOfHours,
                PersonalEffectDetails = PersonalEffectDetails,
                Postcode = Postcode,
                PlaceDeathOccured = PlaceDeathOccured,
                PriorityDetails = PriorityDetails,
                Representatives = null,
                Surname = Surname,
                Street = Street,
                Town = Town,
                TimeOfDeath = TimeOfDeath,
                UrgencyScore = UrgencyScore,
                LastAdmission = LastAdmission,
                CaseCreated = CaseCreated,
                PendingAdmissionNotes = true,
                PendingDiscussionWithQAP = true,
                PendingDiscussionWithRepresentative = true,
                AdmissionNotesHaveBeenAdded = true,
                HaveBeenScrutinisedByME = true,
                HaveFinalCaseOutstandingOutcomes = true,
                ReadyForMEScrutiny = true,
                Unassigned = true,
                CaseOfficer = CaseOfficer
            };

            return examination;
        }

        [Fact]
        public void Examination_To_ExaminationItem_Ensure_Source_Is_Mapped()
        {
            var mapType = _mapper.ConfigurationProvider.FindTypeMapFor<Examination, ExaminationItem>();
            AssertAllSourcePropertiesMappedForMap(mapType);
        }


        /// <summary>
        /// Test Mapping Examination to ExaminationItem.
        /// </summary>
        [Fact]
        public void Examination_To_ExaminationItem()
        {
            var expectedExaminationId = "expectedExaminationId";

            var examination = GenerateExamination();

            var response = _mapper.Map<ExaminationItem>(examination);
            response.GenderDetails.Should().Be(GenderDetails);
            response.Id.Should().Be(expectedExaminationId);
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

        private IEnumerable<Representative> GetRepresentatives(int numberToCreate)
        {
            var representatives = new List<Representative>(numberToCreate);
            for (var counter = 0; counter < numberToCreate; counter++)
            {
                representatives.Add(new Representative()
                {
                    AppointmentDate = new DateTime(2019, 2, 24),
                    AppointmentTime = new TimeSpan(11, 30, 0),
                    FullName = "fullName",
                    Informed = Informed.Yes,
                    PhoneNumber = "123456789",
                    PresentAtDeath = PresentAtDeath.Yes,
                    Relationship = "relationship"
                });
            }

            return representatives;
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

        private void AssertAllSourcePropertiesMappedForMap(TypeMap map)
        {
            
                // Here is hack, because source member mappings are not exposed
                Type t = typeof(TypeMap);
                var configs = t.GetField("_sourceMemberConfigs", BindingFlags.Instance | BindingFlags.NonPublic);
                var mappedSourceProperties = ((IEnumerable<SourceMemberConfig>)configs.GetValue(map)).Select(m => m.SourceMember);

                var mappedProperties = map.PropertyMaps.Select(m => m.SourceMember)
                    .Concat(mappedSourceProperties);

                var properties = map.SourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (var propertyInfo in properties)
                {
                    if (!mappedProperties.Contains(propertyInfo))
                        throw new Exception(String.Format("Property '{0}' of type '{1}' is not mapped",
                            propertyInfo, map.SourceType));
                }
            
        }

        private void AssertAllSourcePropertiesMapped()
        {
            foreach (var map in _mapper.ConfigurationProvider.GetAllTypeMaps())
            {
                // Here is hack, because source member mappings are not exposed
                Type t = typeof(TypeMap);
                var configs = t.GetField("_sourceMemberConfigs", BindingFlags.Instance | BindingFlags.NonPublic);
                var mappedSourceProperties = ((IEnumerable<SourceMemberConfig>)configs.GetValue(map)).Select(m => m.SourceMember);

                var mappedProperties = map.PropertyMaps.Select(m => m.SourceMember)
                    .Concat(mappedSourceProperties);

                var properties = map.SourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (var propertyInfo in properties)
                {
                    if (!mappedProperties.Contains(propertyInfo))
                        throw new Exception(String.Format("Property '{0}' of type '{1}' is not mapped",
                            propertyInfo, map.SourceType));
                }
            }
        }
    }
}
