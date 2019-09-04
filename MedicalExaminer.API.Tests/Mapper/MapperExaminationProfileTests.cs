using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Configuration;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.API.Models.v1.Report;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    /// <summary>
    ///     Mapper Examination Profile Tests
    /// </summary>
    public class MapperExaminationProfileTests
    {
        private const string ExaminationId = "expectedExaminationId";
        private const string AltLink = "altLink";
        private const bool CaseCompleted = true;
        private const bool AnyImplants = true;
        private const bool AnyPersonalEffects = true;
        private const bool ChildPriority = true;
        private const bool Completed = true;
        private const bool CoronerPriority = true;
        private readonly CoronerStatus CoronerStatus = CoronerStatus.SentAwaitingConfirm;
        private const string County = "Cheshire";
        private const string Country = "England";
        private const bool CulturalPriority = true;
        private readonly DateTime DateOfBirth = new DateTime(1990, 2, 24);
        private readonly DateTime DateOfDeath = new DateTime(2019, 2, 24);
        private const string FuneralDirectors = "funeralDirectors";
        private const string MedicalExaminerOfficeResponsibleName = "Medical Examiner Office Name";
        private const bool FaithPriority = true;
        private const string GivenNames = "givenNames";
        private readonly ExaminationGender Gender = ExaminationGender.Male;
        private const string GenderDetails = "genderDetails";
        private const string HospitalNumber_1 = "hospitalNumber_1";
        private const string HospitalNumber_2 = "hospitalNumber_2";
        private const string HospitalNumber_3 = "hospitalNumber_3";
        private const string HouseNameNumber = "houseNameNumber";
        private const string ImplantDetails = "implantDetails";
        private const string LastOccupation = "lastOccupation";
        private const string MedicalExaminerOfficeResponsible = "medicalExaminerOfficeResponsible";
        private readonly ModeOfDisposal ModeOfDisposal = ModeOfDisposal.BuriedAtSea;
        private const string NhsNumber = "123456789";
        private const string OrganisationCareBeforeDeathLocationId = "organisationCareBeforeDeathLocationId";
        private const bool OtherPriority = true;
        private const string PersonalEffectDetails = "personalEffectDetails";
        private const string Postcode = "postcode";
        private const string PlaceDeathOccured = "placeDeathOccured";
        private const string PriorityDetails = "priorityDetails";
        private const string Surname = "surname";
        private const string Street = "street";
        private const string Town = "town";
        private const int UrgencyScore = 1;
        private DateTime CaseCreated = new DateTime(2019, 3, 15);
        private DateTime LastAdmission = new DateTime(2019, 1, 15);
        private TimeSpan TimeOfDeath = new TimeSpan(11, 30, 00);
        private const string CaseOfficer = "CaseOfficer";
        private const string AdmissionNotes = "admissionNotes";
        private TimeSpan AdmittedTime = new TimeSpan(12, 30, 01);
        private MeUser User0 = new MeUser()
        {
            UserId = "userId0",
            FirstName = "Sucha",
            LastName = "Mann",
            Email = "user@email.com",
        };

        private MeUser User1 = new MeUser()
        {
            UserId = "userId1",
            Email = "user1@email.com",
        };

        private readonly MedicalTeam medicalTeam = new MedicalTeam
        {
            ConsultantResponsible = new ClinicalProfessional
            {
                GMCNumber = "ConsultantResponsibleGmcNumber",
                Name = "Consultant Name",
                Notes = "Notes",
                Organisation = "Organisation",
                Phone = "07911110000",
                Role = "Consultant"
            },
            ConsultantsOther = new ClinicalProfessional[]
            {
                new ClinicalProfessional
                {
                    GMCNumber = "gmcNumber",
                    Name = "Other Consultant Name",
                    Notes = "Notes",
                    Organisation = "Organisation",
                    Phone = "07911110000",
                    Role = "Consultant"
                }
            },
            GeneralPractitioner = new ClinicalProfessional
            {
                GMCNumber = "GPGmcNumber",
                Name = "GP Name",
                Notes = "Notes",
                Organisation = "Organisation",
                Phone = "07911110000",
                Role = "GP"
            },
            Qap = new ClinicalProfessional
            {
                GMCNumber = "QapGmcNumber",
                Name = "QAP Name",
                Notes = "Notes",
                Organisation = "Organisation",
                Phone = "07911110000",
                Role = "QAP"
            },
            NursingTeamInformation = "Nursing Team Information",
            MedicalExaminerUserId = "Medical Examiner User Id",
            MedicalExaminerFullName = "Medical Examiner Full Name",
            MedicalExaminerOfficerUserId = "Medical Examiner Officer UserId",
            MedicalExaminerOfficerFullName = "Medical Examiner Officer FullName"
        };

        private readonly IEnumerable<Representative> Representatives = new List<Representative>
        {
            new Representative
            {
                AppointmentDate = new DateTime(2019, 2, 24),
                AppointmentTime = new TimeSpan(11, 30, 0),
                FullName = "fullName",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "123456789",
                PresentAtDeath = PresentAtDeath.Yes,
                Relationship = "relationship"
            }
        };

        private readonly CaseOutcome caseOutcome = new CaseOutcome
        {
            ScrutinyConfirmedOn = new DateTime(2019, 5, 1),
            CaseCompleted = false,
            CaseMedicalExaminerFullName = "Medical Examiner Full Name",
            OutcomeQapDiscussion = QapDiscussionOutcome.MccdCauseOfDeathProvidedByME,
            CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD,
            OutcomeOfPrescrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
            OutcomeOfRepresentativeDiscussion = BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner,
            MccdIssued = true,
            GpNotifiedStatus = GPNotified.GPUnabledToBeNotified,
            CremationFormStatus = CremationFormStatus.Yes
        };

        /// <summary>
        ///     Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        private DateTime dateOfConversation = new DateTime(1984, 12, 24);
        private TimeSpan timeOfConversation = new TimeSpan(10, 30, 00);
        private string discussionDetails = "discussionDetails";

        private string ParticipantFullName = "participantFullName";
        private string ParticipantPhoneNumber = "participantPhoneNumber";
        private string ParticipantRelationship = "participantRelationship";

        private string MedicalHistoryEventText = "medicalHistoryEventText";
        private string SummaryDetails = "SummaryDetails";

        private string CauseOfDeath2 = "CauseOfDeath2";
        private string CauseOfDeath1c = "CauseOfDeath1c";
        private string CauseOfDeath1b = "CauseOfDeath1b";
        private string CauseOfDeath1a = "CauseOfDeath1a";

        private string ClinicalGovernanceReviewText = "clinicalGovernanceReviewText";
        private string MedicalExaminerThoughts = "medicalExaminerThoughts";

        private string ParticipantRoll = "participantRoll";
        private string ParticipantName = "participantName";
        private string ParticipantOrganisation = "participantOrganisation";


        /// <summary>
        ///     Initializes a new instance of the <see cref="MapperExaminationProfileTests" /> class.
        /// </summary>
        public MapperExaminationProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExaminationProfile>();
                // cfg.AddProfile<CaseBreakdownProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Examination_To_PutMedicalTeamResponse()
        {

            var examination = GenerateExamination();
            var response = _mapper.Map<PutMedicalTeamResponse>(examination);
            Assert.True(IsEqual(medicalTeam.ConsultantResponsible, response.ConsultantResponsible));
            Assert.True(IsEqual(medicalTeam.GeneralPractitioner, response.GeneralPractitioner));
            Assert.True(IsEqual(medicalTeam.Qap, response.Qap));
            foreach (var cons in response.ConsultantsOther)
            {
                Assert.True(IsEqual(medicalTeam.ConsultantsOther[0], cons));
            }

            response.NursingTeamInformation.Should().Be(medicalTeam.NursingTeamInformation);
            response.MedicalExaminerUserId.Should().Be(medicalTeam.MedicalExaminerUserId);
            response.MedicalExaminerOfficerUserId.Should().Be(medicalTeam.MedicalExaminerOfficerUserId);
        }

        private bool IsEqual(ClinicalProfessional cp, ClinicalProfessionalItem cpi)
        {
            return (cp.GMCNumber == cpi.GMCNumber &&
                    cp.Name == cpi.Name &&
                    cp.Notes == cpi.Notes &&
                    cp.Organisation == cpi.Organisation
                    && cp.Phone == cpi.Phone
                    && cp.Role == cpi.Role);
        }

        [Fact]
        public void Examination_To_GetMedicalTeamResponse()
        {
            var examination = GenerateExamination();

            var response = _mapper.Map<GetMedicalTeamResponse>(examination);

            Assert.True(IsEqual(examination, response));
        }

        [Fact]
        public void Examination_To_GetCaseBreakdowResponse_When_PreScrutiny_Is_Complete_Qap_Is_Complete()
        {
            var examination = GenerateExamination();

            var expectedPrepopulated = new BereavedDiscussionPrepopulated
            {
                Representatives = examination.Representatives,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                DateOfLatestPreScrutiny = examination.CaseBreakdown.PreScrutiny.Latest.Created,
                UserForLatestPrescrutiny = examination.CaseBreakdown.PreScrutiny.Latest.UserFullName,
                QAPDiscussionStatus = QAPDiscussionStatus.HappenedNoRevision,
                DateOfLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.DateOfConversation,
                UserForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.UserFullName,
                QAPNameForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.ParticipantName,
                CauseOfDeath1a = examination.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1a,
                CauseOfDeath1b = examination.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1b,
                CauseOfDeath1c = examination.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1c,
                CauseOfDeath2 = examination.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath2
            };

            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            IsEquivalent(expectedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void Examination_To_GetCaseBreakdowResponse_When_PreScrutiny_Is_Complete_And_Qap_Is_Unable_To_Happen()
        {
            var examination = GenerateExamination();
            examination.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen = true;

            var expectedPrepopulated = new BereavedDiscussionPrepopulated
            {
                Representatives = examination.Representatives,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                DateOfLatestPreScrutiny = examination.CaseBreakdown.PreScrutiny.Latest.Created,
                UserForLatestPrescrutiny = examination.CaseBreakdown.PreScrutiny.Latest.UserFullName,
                QAPDiscussionStatus = QAPDiscussionStatus.CouldNotHappen,
                DateOfLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.DateOfConversation,
                UserForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.UserFullName,
                QAPNameForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.ParticipantName,
                CauseOfDeath1a = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a,
                CauseOfDeath1b = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b,
                CauseOfDeath1c = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c,
                CauseOfDeath2 = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2
            };

            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            IsEquivalent(expectedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void Examination_To_GetCaseBreakdowResponse_When_PreScrutiny_Is_Complete_Qap_Is_Null()
        {
            var examination = GenerateExamination();
            examination.CaseBreakdown.QapDiscussion.Latest = null;

            var expectedPrepopulated = new BereavedDiscussionPrepopulated
            {
                Representatives = examination.Representatives,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                DateOfLatestPreScrutiny = examination.CaseBreakdown.PreScrutiny.Latest.Created,
                UserForLatestPrescrutiny = examination.CaseBreakdown.PreScrutiny.Latest.UserFullName,
                QAPDiscussionStatus = QAPDiscussionStatus.NoRecord,
                DateOfLatestQAPDiscussion = null,
                UserForLatestQAPDiscussion = null,
                QAPNameForLatestQAPDiscussion = null,
                CauseOfDeath1a = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a,
                CauseOfDeath1b = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b,
                CauseOfDeath1c = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c,
                CauseOfDeath2 = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2
            };

            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            IsEquivalent(expectedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void Examination_To_GetCaseBreakdowResponse_When_PreScrutiny_Is_Null()
        {
            var examination = GenerateExamination();
            examination.CaseBreakdown.PreScrutiny.Latest = null;

            var expectedPrepopulated = new BereavedDiscussionPrepopulated
            {
                Representatives = examination.Representatives,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyNotHappened,
                DateOfLatestPreScrutiny = null,
                UserForLatestPrescrutiny = null,
                QAPDiscussionStatus = QAPDiscussionStatus.HappenedNoRevision,
                DateOfLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.DateOfConversation,
                UserForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.UserFullName,
                QAPNameForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.ParticipantName,
                CauseOfDeath1a = examination.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1a,
                CauseOfDeath1b = examination.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1b,
                CauseOfDeath1c = examination.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1c,
                CauseOfDeath2 = examination.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath2
            };

            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            IsEquivalent(expectedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void Examination_To_GetCaseBreakdowResponse_When_PreScrutiny_Is_Null_And_Qap_Is_Unable_To_Happen()
        {
            var examination = GenerateExamination();
            examination.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen = true;
            examination.CaseBreakdown.PreScrutiny.Latest = null;

            var expectedPrepopulated = new BereavedDiscussionPrepopulated
            {
                Representatives = examination.Representatives,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyNotHappened,
                DateOfLatestPreScrutiny = null,
                UserForLatestPrescrutiny = null,
                QAPDiscussionStatus = QAPDiscussionStatus.CouldNotHappen,
                DateOfLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.DateOfConversation,
                UserForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.UserFullName,
                QAPNameForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.ParticipantName,
                CauseOfDeath1a = null,
                CauseOfDeath1b = null,
                CauseOfDeath1c = null,
                CauseOfDeath2 = null
            };

            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            IsEquivalent(expectedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void Examination_To_GetCaseBreakdowResponse_When_PreScrutiny_Is_Null_Qap_Is_Null()
        {
            var examination = GenerateExamination();
            examination.CaseBreakdown.PreScrutiny.Latest = null;
            examination.CaseBreakdown.QapDiscussion.Latest = null;

            var expectedPrepopulated = new BereavedDiscussionPrepopulated
            {
                Representatives = examination.Representatives,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyNotHappened,
                DateOfLatestPreScrutiny = null,
                UserForLatestPrescrutiny = null,
                QAPDiscussionStatus = QAPDiscussionStatus.NoRecord,
                DateOfLatestQAPDiscussion = null,
                UserForLatestQAPDiscussion = null,
                QAPNameForLatestQAPDiscussion = null,
                CauseOfDeath1a = null,
                CauseOfDeath1b = null,
                CauseOfDeath1c = null,
                CauseOfDeath2 = null
            };

            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            IsEquivalent(expectedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void Examination_To_GetCaseBreakdowResponse_When_PreScrutiny_Is_Complete_Qap_Accepts_COD_Provided_By_ME()
        {
            var examination = GenerateExamination();
            examination.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathProvidedByME;

            var expectedPrepopulated = new BereavedDiscussionPrepopulated
            {
                Representatives = examination.Representatives,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                DateOfLatestPreScrutiny = examination.CaseBreakdown.PreScrutiny.Latest.Created,
                UserForLatestPrescrutiny = examination.CaseBreakdown.PreScrutiny.Latest.UserFullName,
                QAPDiscussionStatus = QAPDiscussionStatus.HappenedNoRevision,
                DateOfLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.DateOfConversation,
                UserForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.UserFullName,
                QAPNameForLatestQAPDiscussion = examination.CaseBreakdown.QapDiscussion.Latest.ParticipantName,
                CauseOfDeath1a = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a,
                CauseOfDeath1b = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b,
                CauseOfDeath1c = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c,
                CauseOfDeath2 = examination.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2
            };

            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            IsEquivalent(expectedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        private void IsEquivalent(BereavedDiscussionPrepopulated expected, BereavedDiscussionPrepopulated actual)
        {
            actual.Representatives.Should().BeEquivalentTo(expected.Representatives);
            actual.CauseOfDeath1a.Should().Be(expected.CauseOfDeath1a);
            actual.CauseOfDeath1b.Should().Be(expected.CauseOfDeath1b);
            actual.CauseOfDeath1c.Should().Be(expected.CauseOfDeath1c);
            actual.CauseOfDeath2.Should().Be(expected.CauseOfDeath2);
            actual.DateOfLatestPreScrutiny.Should().Be(actual.DateOfLatestPreScrutiny);
            actual.DateOfLatestQAPDiscussion.Should().Be(actual.DateOfLatestQAPDiscussion);
            actual.MedicalExaminer.Should().Be(expected.MedicalExaminer);
            actual.PreScrutinyStatus.Should().Be(expected.PreScrutinyStatus);
            actual.QAPDiscussionStatus.Should().Be(expected.QAPDiscussionStatus);
            actual.QAPNameForLatestQAPDiscussion.Should().Be(expected.QAPNameForLatestQAPDiscussion);
            actual.UserForLatestPrescrutiny.Should().Be(expected.UserForLatestPrescrutiny);
            actual.UserForLatestQAPDiscussion.Should().Be(expected.UserForLatestQAPDiscussion);
        }

        [Fact]
        public void Examination_To_GetCaseBreakdowResponse()
        {
            var examination = GenerateExamination();
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            Assert.True(IsEqual(examination.CaseBreakdown, result));
        }

        private bool IsEqual(CaseBreakDown caseBreakdown, CaseBreakDownItem caseBreakdownItem)
        {
            return IsEqual(caseBreakdown.AdmissionNotes, caseBreakdownItem.AdmissionNotes) &&
                IsEqual(caseBreakdown.BereavedDiscussion, caseBreakdownItem.BereavedDiscussion) &&
                IsEqual(caseBreakdown.MedicalHistory, caseBreakdownItem.MedicalHistory) &&
                IsEqual(caseBreakdown.MeoSummary, caseBreakdownItem.MeoSummary) &&
                IsEqual(caseBreakdown.OtherEvents, caseBreakdownItem.OtherEvents) &&
                IsEqual(caseBreakdown.PreScrutiny, caseBreakdownItem.PreScrutiny) &&
                IsEqual(caseBreakdown.QapDiscussion, caseBreakdownItem.QapDiscussion);
        }

        private bool IsEqual(BaseEventContainter<QapDiscussionEvent> qapDiscussion1, EventContainerItem<QapDiscussionEventItem, QapDiscussionPrepopulated> qapDiscussion2)
        {
            var historyIsEqual = true;

            foreach (var history in qapDiscussion1.History)
            {
                var historyItem = qapDiscussion2.History.Single(x => x.EventId == history.EventId);
                historyIsEqual = IsEqual(history, historyItem);
                if (!historyIsEqual)
                {
                    break;
                }
            }

            return historyIsEqual && IsEqual(qapDiscussion1.Latest, qapDiscussion2.Latest);
        }

        private bool IsEqual(QapDiscussionEvent qap, QapDiscussionEventItem qapItem)
        {
            return qapItem.CauseOfDeath1a == qap.CauseOfDeath1a &&
                qapItem.CauseOfDeath1b == qap.CauseOfDeath1b &&
                qapItem.CauseOfDeath1c == qap.CauseOfDeath1c &&
                qapItem.CauseOfDeath2 == qap.CauseOfDeath2 &&
                qapItem.Created == qap.Created &&
                qapItem.DateOfConversation == qap.DateOfConversation &&
                qapItem.TimeOfConversation == qap.TimeOfConversation &&
                qapItem.DiscussionDetails == qap.DiscussionDetails &&
                qapItem.DiscussionUnableHappen == qap.DiscussionUnableHappen &&
                qapItem.IsFinal == qap.IsFinal &&
                qapItem.ParticipantName == qap.ParticipantName &&
                qapItem.ParticipantOrganisation == qap.ParticipantOrganisation &&
                qapItem.ParticipantPhoneNumber == qap.ParticipantPhoneNumber &&
                qapItem.ParticipantRole == qap.ParticipantRole &&
                qapItem.QapDiscussionOutcome == qap.QapDiscussionOutcome &&
                qapItem.UserId == qap.UserId;
        }

        private bool IsEqual(BaseEventContainter<PreScrutinyEvent> preScrutiny1, EventContainerItem<PreScrutinyEventItem, NullPrepopulated> preScrutiny2)
        {
            var historyIsEqual = true;

            foreach (var history in preScrutiny1.History)
            {
                var historyItem = preScrutiny2.History.Single(x => x.EventId == history.EventId);
                historyIsEqual = IsEqual(history, historyItem);
                if (!historyIsEqual)
                {
                    break;
                }
            }

            return historyIsEqual && IsEqual(preScrutiny1.Latest, preScrutiny2.Latest);
        }

        private bool IsEqual(PreScrutinyEvent history, PreScrutinyEventItem historyItem)
        {
            return historyItem.CauseOfDeath1a == history.CauseOfDeath1a &&
                historyItem.CauseOfDeath1b == history.CauseOfDeath1b &&
                historyItem.CauseOfDeath1c == history.CauseOfDeath1c &&
                historyItem.CauseOfDeath2 == history.CauseOfDeath2 &&
                historyItem.CircumstancesOfDeath == history.CircumstancesOfDeath &&
                historyItem.ClinicalGovernanceReview == history.ClinicalGovernanceReview &&
                historyItem.ClinicalGovernanceReviewText == history.ClinicalGovernanceReviewText &&
                historyItem.Created == history.Created &&
                historyItem.IsFinal == history.IsFinal &&
                historyItem.MedicalExaminerThoughts == history.MedicalExaminerThoughts &&
                historyItem.OutcomeOfPreScrutiny == history.OutcomeOfPreScrutiny &&
                historyItem.UserId == history.UserId;
        }

        private bool IsEqual(BaseEventContainter<OtherEvent> otherEvents1, EventContainerItem<OtherEventItem, NullPrepopulated> otherEvents2)
        {
            var historyIsEqual = true;

            foreach (var history in otherEvents1.History)
            {
                var historyItem = otherEvents2.History.Single(x => x.EventId == history.EventId);
                historyIsEqual = IsEqual(history, historyItem);
                if (!historyIsEqual)
                {
                    break;
                }
            }

            return historyIsEqual && IsEqual(otherEvents1.Latest, otherEvents2.Latest);
        }

        private bool IsEqual(OtherEvent history, OtherEventItem historyItem)
        {
            if (history == null && historyItem == null)
            {
                return true;
            }

            return historyItem.UserId == history.UserId &&
                historyItem.Text == history.Text &&
                historyItem.IsFinal == history.IsFinal;
        }

        private bool IsEqual(BaseEventContainter<MeoSummaryEvent> meoSummary1, EventContainerItem<MeoSummaryEventItem, NullPrepopulated> meoSummary2)
        {
            var historyIsEqual = true;

            foreach (var history in meoSummary1.History)
            {
                var historyItem = meoSummary2.History.Single(x => x.EventId == history.EventId);
                historyIsEqual = IsEqual(history, historyItem);
                if (!historyIsEqual)
                {
                    break;
                }
            }

            return historyIsEqual && IsEqual(meoSummary1.Latest, meoSummary2.Latest);
        }

        private bool IsEqual(MeoSummaryEvent history, MeoSummaryEventItem historyItem)
        {
            return historyItem.IsFinal == history.IsFinal &&
                historyItem.Created == history.Created &&
                historyItem.SummaryDetails == history.SummaryDetails &&
                historyItem.UserId == history.UserId;
        }

        private bool IsEqual(BaseEventContainter<MedicalHistoryEvent> medicalHistory1, EventContainerItem<MedicalHistoryEventItem, NullPrepopulated> medicalHistory2)
        {
            var historyIsEqual = true;

            foreach (var history in medicalHistory1.History)
            {
                var historyItem = medicalHistory2.History.Single(x => x.EventId == history.EventId);
                historyIsEqual = IsEqual(history, historyItem);
                if (!historyIsEqual)
                {
                    break;
                }
            }

            return historyIsEqual && IsEqual(medicalHistory1.Latest, medicalHistory2.Latest);
        }

        private bool IsEqual(MedicalHistoryEvent history, MedicalHistoryEventItem historyItem)
        {
            return historyItem.Created == history.Created &&
                historyItem.IsFinal == history.IsFinal &&
                historyItem.Text == history.Text &&
                historyItem.UserId == history.UserId;
        }

        private bool IsEqual(BaseEventContainter<BereavedDiscussionEvent> bereavedDiscussion1, EventContainerItem<BereavedDiscussionEventItem, BereavedDiscussionPrepopulated> bereavedDiscussion2)
        {
            var historyIsEqual = true;

            foreach (var history in bereavedDiscussion1.History)
            {
                var historyItem = bereavedDiscussion2.History.Single(x => x.EventId == history.EventId);
                historyIsEqual = IsEqual(history, historyItem);
                if (!historyIsEqual)
                {
                    break;
                }
            }

            return historyIsEqual && IsEqual(bereavedDiscussion1.Latest, bereavedDiscussion2.Latest);
        }

        private bool IsEqual(BereavedDiscussionEvent bereavedDiscussion, BereavedDiscussionEventItem bereavedDiscussionItem)
        {
            return bereavedDiscussionItem.BereavedDiscussionOutcome == bereavedDiscussion.BereavedDiscussionOutcome &&
                   bereavedDiscussionItem.Created == bereavedDiscussion.Created &&
                   bereavedDiscussionItem.DateOfConversation == bereavedDiscussion.DateOfConversation &&
                   bereavedDiscussionItem.TimeOfConversation == bereavedDiscussion.TimeOfConversation &&
                   bereavedDiscussionItem.DiscussionDetails == bereavedDiscussion.DiscussionDetails &&
                   bereavedDiscussionItem.DiscussionUnableHappen == bereavedDiscussion.DiscussionUnableHappen &&
                   bereavedDiscussionItem.InformedAtDeath == bereavedDiscussion.InformedAtDeath &&
                   bereavedDiscussionItem.IsFinal == bereavedDiscussion.IsFinal &&
                   bereavedDiscussionItem.ParticipantFullName == bereavedDiscussion.ParticipantFullName &&
                   bereavedDiscussionItem.ParticipantPhoneNumber == bereavedDiscussion.ParticipantPhoneNumber &&
                   bereavedDiscussionItem.ParticipantRelationship == bereavedDiscussion.ParticipantRelationship &&
                   bereavedDiscussionItem.PresentAtDeath == bereavedDiscussion.PresentAtDeath &&
                   bereavedDiscussionItem.UserId == bereavedDiscussion.UserId;
        }

        private bool IsEqual(BaseEventContainter<AdmissionEvent> admissionNotes1, EventContainerItem<AdmissionEventItem, NullPrepopulated> admissionNotes2)
        {
            var historyIsEqual = true;

            foreach (var history in admissionNotes1.History)
            {
                var historyItem = admissionNotes2.History.Single(x => x.EventId == history.EventId);
                historyIsEqual = IsEqual(history, historyItem);
                if (!historyIsEqual)
                {
                    break;
                }
            }

            var draftIsEqual = IsEqual(admissionNotes1.Drafts.Single(draft => draft.UserId == User0.UserId), admissionNotes2.UsersDraft);

            return draftIsEqual && historyIsEqual && IsEqual(admissionNotes1.Latest, admissionNotes2.Latest);
        }

        private bool IsEqual(AdmissionEvent history, AdmissionEventItem historyItem)
        {
            return historyItem.AdmittedDate == history.AdmittedDate &&
                historyItem.AdmittedTime == history.AdmittedTime &&
                historyItem.Created == history.Created &&
                historyItem.ImmediateCoronerReferral == history.ImmediateCoronerReferral &&
                historyItem.IsFinal == history.IsFinal &&
                historyItem.Notes == history.Notes &&
                historyItem.UserId == history.UserId &&
                historyItem.AdmittedDateUnknown == history.AdmittedDateUnknown &&
                historyItem.AdmittedTimeUnknown == history.AdmittedTimeUnknown;
        }

        private bool IsEqual(Examination examination, PatientDeathEventItem patientDeathEvent)
        {
            return patientDeathEvent.TimeOfDeath == examination.TimeOfDeath &&
                patientDeathEvent.DateOfDeath == examination.DateOfDeath;
        }

        private bool IsEqual(Examination examination, GetMedicalTeamResponse response)
        {
            return IsEqual(examination.MedicalTeam.ConsultantResponsible, response.ConsultantResponsible) &&
                IsEqual(examination.MedicalTeam.GeneralPractitioner, response.GeneralPractitioner) &&
                IsEqual(examination.MedicalTeam.Qap, response.Qap) &&
                response.NursingTeamInformation == examination.MedicalTeam.NursingTeamInformation &&
                response.MedicalExaminerUserId == examination.MedicalTeam.MedicalExaminerUserId &&
                response.MedicalExaminerOfficerUserId == examination.MedicalTeam.MedicalExaminerOfficerUserId &&
                IsEqual(examination.MedicalTeam.ConsultantsOther[0], response.ConsultantsOther[0]);
        }

        [Fact]
        public void Examination_To_GetPatientDetailsResponse()
        {
            var examination = GenerateExamination();

            var response = _mapper.Map<GetPatientDetailsResponse>(examination);

            response.CaseCompleted.Should().Be(Completed);
            response.AnyImplants.Should().Be(AnyImplants);
            response.AnyPersonalEffects.Should().Be(examination.AnyPersonalEffects);
            response.ChildPriority.Should().Be(examination.ChildPriority);
            response.CoronerPriority.Should().Be(CoronerPriority);
            response.CulturalPriority.Should().Be(CulturalPriority);
            response.FaithPriority.Should().Be(FaithPriority);
            response.OtherPriority.Should().Be(OtherPriority);
            response.PriorityDetails.Should().Be(PriorityDetails);
            response.CoronerStatus.Should().Be(CoronerStatus);
            response.Gender.Should().Be(Gender);
            response.County.Should().Be(County);
            response.Country.Should().Be(Country);
            response.UrgencyScore.Should().Be(UrgencyScore);
            response.GenderDetails.Should().Be(GenderDetails);
            response.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            response.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            response.MedicalExaminerOfficeResponsibleName.Should().Be(MedicalExaminerOfficeResponsibleName);
            response.DateOfBirth.Should().Be(DateOfBirth);
            response.HospitalNumber_1.Should().Be(HospitalNumber_1);
            response.HospitalNumber_2.Should().Be(HospitalNumber_2);
            response.HospitalNumber_3.Should().Be(HospitalNumber_3);
            response.TimeOfDeath.Should().Be(TimeOfDeath);
            response.GivenNames.Should().Be(GivenNames);
            response.Surname.Should().Be(Surname);
            response.PostCode.Should().Be(Postcode);
            response.HouseNameNumber.Should().Be(HouseNameNumber);
            response.Street.Should().Be(Street);
            response.Town.Should().Be(Town);
            response.LastOccupation.Should().Be(LastOccupation);
            response.OrganisationCareBeforeDeathLocationId.Should().Be(OrganisationCareBeforeDeathLocationId);
            response.ImplantDetails.Should().Be(ImplantDetails);
            response.FuneralDirectors.Should().Be(FuneralDirectors);
            response.PersonalEffectDetails.Should().Be(PersonalEffectDetails);
            response.Representatives.Should().AllBeEquivalentTo(Representatives);
        }

        [Fact]
        public void Examination_To_GetCaseOutcomeResponse()
        {
            var examination = GenerateExamination();

            var response = _mapper.Map<GetCaseOutcomeResponse>(examination);

            response.CaseCompleted.Should().Be(Completed);
            response.CaseMedicalExaminerFullName.Should().Be(caseOutcome.CaseMedicalExaminerFullName);
            response.CaseOutcomeSummary.Should().Be(caseOutcome.CaseOutcomeSummary);
            response.CremationFormStatus.Should().Be(caseOutcome.CremationFormStatus);
            response.GpNotifiedStatus.Should().Be(caseOutcome.GpNotifiedStatus);
            response.MccdIssued.Should().Be(caseOutcome.MccdIssued);
            response.OutcomeOfPrescrutiny.Should().Be(examination.CaseBreakdown.PreScrutiny.Latest.OutcomeOfPreScrutiny);
            response.OutcomeOfRepresentativeDiscussion.Should().Be(examination.CaseBreakdown.BereavedDiscussion.Latest.BereavedDiscussionOutcome);
            response.OutcomeQapDiscussion.Should().Be(examination.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome);
            response.ScrutinyConfirmedOn.Should().Be(caseOutcome.ScrutinyConfirmedOn);
        }

        [Fact]
        public void Examination_To_CaseOutcome()
        {
            var examination = GenerateExamination();

            var result = _mapper.Map<CaseOutcome>(examination);

            result.ScrutinyConfirmedOn.Should().Be(caseOutcome.ScrutinyConfirmedOn);
            result.CaseMedicalExaminerFullName.Should().Be(caseOutcome.CaseMedicalExaminerFullName);
            result.CaseOutcomeSummary.Should().Be(caseOutcome.CaseOutcomeSummary);
            result.CremationFormStatus.Should().Be(caseOutcome.CremationFormStatus);
            result.GpNotifiedStatus.Should().Be(caseOutcome.GpNotifiedStatus);
            result.MccdIssued.Should().Be(caseOutcome.MccdIssued);
            result.OutcomeOfPrescrutiny.Should().Be(caseOutcome.OutcomeOfPrescrutiny);
            result.OutcomeOfRepresentativeDiscussion.Should().Be(caseOutcome.OutcomeOfRepresentativeDiscussion);
            result.OutcomeQapDiscussion.Should().Be(caseOutcome.OutcomeQapDiscussion);
            result.CaseCompleted.Should().Be(Completed);

        }

        [Fact]
        public void Examination_To_PatientCard_NullAppointments()
        {
            var examination = GenerateExamination();

            var result = _mapper.Map<PatientCardItem>(examination);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.ExaminationId.Should().Be(ExaminationId);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(null);
            result.AppointmentTime.Should().Be(null);
            result.LastAdmission.Should().Be(LastAdmission);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutcomesOutstanding.Should().Be(true);
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
                Informed = InformedAtDeath.Yes,
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
            result.ExaminationId.Should().Be(ExaminationId);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(null);
            result.AppointmentTime.Should().Be(null);
            result.LastAdmission.Should().Be(LastAdmission);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutcomesOutstanding.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);
        }

        [Fact]
        public void Examination_To_PatientCard_Statuses_When_All_Required_Details_Entered()
        {
            // Arrange
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representative = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = GenerateExamination();
            examination.GivenNames = "GivenNames";
            examination.Surname = "Surname";
            examination.DateOfBirth = new DateTime(2000, 01, 12);
            examination.DateOfDeath = new DateTime(2019, 08, 12);
            examination.NhsNumber = "1234567890";

            examination.MedicalTeam.ConsultantResponsible = new ClinicalProfessional
            {
                Name = "ConsultantResponsible",
                Role = "Consultant",
                Organisation = "Organisation",
                Phone = "01148394748",
                Notes = "Notes",
                GMCNumber = "G12345"
            };
            examination.MedicalTeam.Qap.Name = "Qap Name";
            examination.Representatives = new[] { representative };
            examination.MedicalTeam.MedicalExaminerUserId = "MedicalExaminerUserId";

            examination.CaseOutcome.MccdIssued = true;
            examination.CaseOutcome.CremationFormStatus = CremationFormStatus.Yes;
            examination.CaseOutcome.GpNotifiedStatus = GPNotified.GPNotified;
            examination.CaseOutcome.CoronerReferralSent = true;
            examination.CaseCompleted = true;

            // Action
            var result = _mapper.Map<PatientCardItem>(examination);

            // Assert
            // Basic Details
            result.BasicDetailsEntered.Should().BeTrue();
            result.NameEntered.Should().BeTrue();
            result.DobEntered.Should().BeTrue();
            result.DodEntered.Should().BeTrue();
            result.NhsNumberEntered.Should().BeTrue();

            // Additional Details
            result.AdditionalDetailsEntered.Should().BeTrue();
            result.LatestAdmissionDetailsEntered.Should().BeTrue();
            result.DoctorInChargeEntered.Should().BeTrue();
            result.QapEntered.Should().BeTrue();
            result.BereavedInfoEntered.Should().BeTrue();
            result.MeAssigned.Should().BeTrue();

            // Is Scrutiny Complete?
            result.IsScrutinyCompleted.Should().BeTrue();
            result.PreScrutinyEventEntered.Should().BeTrue();
            result.QapDiscussionEventEntered.Should().BeTrue();
            result.BereavedDiscussionEventEntered.Should().BeTrue();

            // Is Case Items Complete?
            result.IsCaseItemsCompleted.Should().BeTrue();
            result.MccdIssued.Should().BeTrue();
            result.CremationFormInfoEntered.Should().BeTrue();
            result.GpNotified.Should().BeTrue();
            result.SentToCoroner.Should().BeTrue();
            result.CaseClosed.Should().BeTrue();
        }

        [Fact]
        public void Examination_To_PatientCard_Statuses_When_Basic_Details_Not_Entered()
        {
            // Arrange
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representative = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = GenerateExamination();
            examination.GivenNames = null;
            examination.Surname = null;
            examination.DateOfBirth = null;
            examination.DateOfDeath = null;
            examination.NhsNumber = null;

            examination.MedicalTeam.ConsultantResponsible = new ClinicalProfessional
            {
                Name = "ConsultantResponsible",
                Role = "Consultant",
                Organisation = "Organisation",
                Phone = "01148394748",
                Notes = "Notes",
                GMCNumber = "G12345"
            };
            examination.MedicalTeam.Qap.Name = "Qap Name";
            examination.Representatives = new[] { representative };
            examination.MedicalTeam.MedicalExaminerUserId = "MedicalExaminerUserId";

            examination.CaseOutcome.MccdIssued = true;
            examination.CaseOutcome.CremationFormStatus = CremationFormStatus.Yes;
            examination.CaseOutcome.GpNotifiedStatus = GPNotified.GPNotified;
            examination.CaseOutcome.CoronerReferralSent = true;

            // Action
            var result = _mapper.Map<PatientCardItem>(examination);

            // Assert
            // Basic Details
            result.BasicDetailsEntered.Should().BeFalse();
            result.NameEntered.Should().BeFalse();
            result.DobEntered.Should().BeFalse();
            result.DodEntered.Should().BeFalse();
            result.NhsNumberEntered.Should().BeFalse();
        }

        [Fact]
        public void Examination_To_PatientCard_Statuses_When_Additional_Details_Not_Entered()
        {
            // Arrange
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representative = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = GenerateExamination();
            examination.GivenNames = "GivenNames";
            examination.Surname = "Surname";
            examination.DateOfBirth = new DateTime(2000, 01, 12);
            examination.DateOfDeath = new DateTime(2019, 08, 12);
            examination.NhsNumber = "1234567890";

            examination.CaseBreakdown.AdmissionNotes.Latest = null;
            examination.MedicalTeam.ConsultantResponsible = null;
            examination.MedicalTeam.Qap.Name = null;
            examination.Representatives = null;
            examination.MedicalTeam.MedicalExaminerUserId = null;

            examination.CaseOutcome.MccdIssued = true;
            examination.CaseOutcome.CremationFormStatus = CremationFormStatus.Yes;
            examination.CaseOutcome.GpNotifiedStatus = GPNotified.GPNotified;
            examination.CaseOutcome.CoronerReferralSent = true;

            // Action
            var result = _mapper.Map<PatientCardItem>(examination);

            // Assert
            // Additional Details
            result.AdditionalDetailsEntered.Should().BeFalse();
            result.LatestAdmissionDetailsEntered.Should().BeFalse();
            result.DoctorInChargeEntered.Should().BeFalse();
            result.QapEntered.Should().BeFalse();
            result.BereavedInfoEntered.Should().BeFalse();
            result.MeAssigned.Should().BeFalse();
        }

        [Fact]
        public void Examination_To_PatientCard_Statuses_When_Events_Required_For_Scrutiny_Not_Entered()
        {
            // Arrange
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representative = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = GenerateExamination();
            examination.GivenNames = "GivenNames";
            examination.Surname = "Surname";
            examination.DateOfBirth = new DateTime(2000, 01, 12);
            examination.DateOfDeath = new DateTime(2019, 08, 12);
            examination.NhsNumber = "1234567890";

            examination.MedicalTeam.ConsultantResponsible = new ClinicalProfessional
            {
                Name = "ConsultantResponsible",
                Role = "Consultant",
                Organisation = "Organisation",
                Phone = "01148394748",
                Notes = "Notes",
                GMCNumber = "G12345"
            };
            examination.MedicalTeam.Qap.Name = "Qap Name";
            examination.Representatives = new[] { representative };
            examination.MedicalTeam.MedicalExaminerUserId = "MedicalExaminerUserId";

            examination.CaseBreakdown.PreScrutiny.Latest = null;
            examination.CaseBreakdown.QapDiscussion.Latest = null;
            examination.CaseBreakdown.BereavedDiscussion.Latest = null;

            examination.CaseOutcome.MccdIssued = true;
            examination.CaseOutcome.CremationFormStatus = CremationFormStatus.Yes;
            examination.CaseOutcome.GpNotifiedStatus = GPNotified.GPNotified;
            examination.CaseOutcome.CoronerReferralSent = true;

            // Action
            var result = _mapper.Map<PatientCardItem>(examination);

            // Assert
            // Is Scrutiny Complete?
            result.IsScrutinyCompleted.Should().BeFalse();
            result.PreScrutinyEventEntered.Should().BeFalse();
            result.QapDiscussionEventEntered.Should().BeFalse();
            result.BereavedDiscussionEventEntered.Should().BeFalse();
        }

        [Fact]
        public void Examination_To_PatientCard_Statuses_When_Case_Items_Not_Entered()
        {
            // Arrange
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representative = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = GenerateExamination();
            examination.GivenNames = "GivenNames";
            examination.Surname = "Surname";
            examination.DateOfBirth = new DateTime(2000, 01, 12);
            examination.DateOfDeath = new DateTime(2019, 08, 12);
            examination.NhsNumber = "1234567890";

            examination.MedicalTeam.ConsultantResponsible = new ClinicalProfessional
            {
                Name = "ConsultantResponsible",
                Role = "Consultant",
                Organisation = "Organisation",
                Phone = "01148394748",
                Notes = "Notes",
                GMCNumber = "G12345"
            };
            examination.MedicalTeam.Qap.Name = "Qap Name";
            examination.Representatives = new[] { representative };
            examination.MedicalTeam.MedicalExaminerUserId = "MedicalExaminerUserId";

            examination.CaseOutcome.MccdIssued = null;
            examination.CaseOutcome.CremationFormStatus = null;
            examination.CaseOutcome.GpNotifiedStatus = null;
            examination.CaseOutcome.CoronerReferralSent = false;
            examination.CaseCompleted = false;

            // Action
            var result = _mapper.Map<PatientCardItem>(examination);

            // Assert
            // Is Case Items Complete?
            result.IsCaseItemsCompleted.Should().BeFalse();
            result.MccdIssued.Should().BeFalse();
            result.CremationFormInfoEntered.Should().BeFalse();
            result.GpNotified.Should().BeFalse();
            result.SentToCoroner.Should().BeFalse();
            result.CaseClosed.Should().BeFalse();
        }

        [Fact]
        public void Examination_To_PatientCard_Statuses_When_Unknown_Case_Items_Entered()
        {
            // Arrange
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representative = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = GenerateExamination();
            examination.GivenNames = "GivenNames";
            examination.Surname = "Surname";
            examination.DateOfBirth = new DateTime(2000, 01, 12);
            examination.DateOfDeath = new DateTime(2019, 08, 12);
            examination.NhsNumber = "1234567890";

            examination.MedicalTeam.ConsultantResponsible = new ClinicalProfessional
            {
                Name = "ConsultantResponsible",
                Role = "Consultant",
                Organisation = "Organisation",
                Phone = "01148394748",
                Notes = "Notes",
                GMCNumber = "G12345"
            };
            examination.MedicalTeam.Qap.Name = "Qap Name";
            examination.Representatives = new[] { representative };
            examination.MedicalTeam.MedicalExaminerUserId = "MedicalExaminerUserId";

            examination.CaseOutcome.MccdIssued = null;
            examination.CaseOutcome.CremationFormStatus = CremationFormStatus.Unknown;
            examination.CaseOutcome.GpNotifiedStatus = null;
            examination.CaseOutcome.CoronerReferralSent = false;

            // Action
            var result = _mapper.Map<PatientCardItem>(examination);

            // Assert
            // Is Case Items Complete?
            result.IsCaseItemsCompleted.Should().BeNull();
            result.CremationFormInfoEntered.Should().BeNull();
        }

        [Fact]
        public void Examination_To_PatientCard_One_Representative_Appointment_Details()
        {
            // Arrange
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representative = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var examination = GenerateExamination();
            examination.Representatives = new[] { representative };

            // Action
            var result = _mapper.Map<PatientCardItem>(examination);

            // Assert
            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.ExaminationId.Should().Be(ExaminationId);
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
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutcomesOutstanding.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);
        }

        [Fact]
        public void Examination_To_PatientCard_Two_Representatives_One_Null_One_Complete_Appointment_Details()
        {
            // Arrange
            var appointmentDate = DateTime.Now.AddDays(1);
            var appointmentTime = new TimeSpan(10, 30, 00);
            var representativeOne = new Representative()
            {
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var representativeTwo = new Representative()
            {
                AppointmentDate = null,
                AppointmentTime = null,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "uncle"
            };

            var examination = GenerateExamination();
            examination.Representatives = new[] { representativeTwo, representativeOne };

            // Action
            var result = _mapper.Map<PatientCardItem>(examination);

            // Assert
            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.ExaminationId.Should().Be(ExaminationId);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(appointmentDate);
            result.AppointmentTime.Should().Be(appointmentTime);
            result.LastAdmission.Should().Be(LastAdmission);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutcomesOutstanding.Should().Be(true);
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
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "uncle"
            };

            var representativeTwo = new Representative()
            {
                AppointmentDate = appointmentDate2,
                AppointmentTime = appointmentTime,
                FullName = "barry",
                Informed = InformedAtDeath.Yes,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "granddad"
            };

            var examination = GenerateExamination();
            examination.Representatives = new[] { representativeTwo, representativeOne };

            var result = _mapper.Map<PatientCardItem>(examination);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.ExaminationId.Should().Be(ExaminationId);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(null);
            result.AppointmentTime.Should().Be(null);
            result.LastAdmission.Should().Be(LastAdmission);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutcomesOutstanding.Should().Be(true);
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
                Informed = InformedAtDeath.Unknown,
                PhoneNumber = "1234",
                PresentAtDeath = PresentAtDeath.Unknown,
                Relationship = "milk man"
            };

            var representativeTwo = new Representative()
            {
                AppointmentDate = appointmentDate2,
                AppointmentTime = appointmentTime,
                FullName = "bob",
                Informed = InformedAtDeath.Yes,
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
            result.ExaminationId.Should().Be(ExaminationId);
            result.NhsNumber.Should().Be(NhsNumber);
            result.GivenNames.Should().Be(GivenNames);
            result.Surname.Should().Be(Surname);
            result.UrgencyScore.Should().Be(UrgencyScore);
            result.AppointmentDate.Should().Be(appointmentDate1);
            result.AppointmentTime.Should().Be(appointmentTime);
            result.LastAdmission.Should().Be(LastAdmission);
            result.ReadyForMEScrutiny.Should().Be(true);
            result.AdmissionNotesHaveBeenAdded.Should().Be(true);
            result.HaveBeenScrutinisedByME.Should().Be(true);
            result.HaveFinalCaseOutcomesOutstanding.Should().Be(true);
            result.PendingAdmissionNotes.Should().Be(true);
            result.PendingDiscussionWithQAP.Should().Be(true);
            result.PendingDiscussionWithRepresentative.Should().Be(true);
            result.Unassigned.Should().Be(true);
        }

        [Fact]
        public void PostNewCaseRequest_To_Examination()
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
                PlaceDeathOccured = PlaceDeathOccured,
                Surname = Surname,
                TimeOfDeath = TimeOfDeath
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
            result.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            result.Surname.Should().Be(Surname);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
        }

        private Examination GenerateExamination(CaseBreakDown casebreakdown = null)
        {
            if (casebreakdown == null)
            {
                casebreakdown = GenerateCaseBreakdown();
            }

            var examination = new Examination
            {
                CreatedAt = CaseCreated,
                ExaminationId = ExaminationId,
                CaseBreakdown = casebreakdown,
                CaseOutcome = caseOutcome,
                AnyImplants = AnyImplants,
                AnyPersonalEffects = AnyPersonalEffects,
                ChildPriority = ChildPriority,
                CaseCompleted = Completed,
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
                MedicalExaminerOfficeResponsibleName = MedicalExaminerOfficeResponsibleName,
                OrganisationCareBeforeDeathLocationId = OrganisationCareBeforeDeathLocationId,
                OtherPriority = OtherPriority,
                PersonalEffectDetails = PersonalEffectDetails,
                Postcode = Postcode,
                PlaceDeathOccured = PlaceDeathOccured,
                PriorityDetails = PriorityDetails,
                Representatives = Enumerable.Empty<Representative>(),
                Surname = Surname,
                Street = Street,
                Town = Town,
                TimeOfDeath = TimeOfDeath,
                LastAdmission = LastAdmission,
                PendingAdmissionNotes = true,
                PendingDiscussionWithQAP = true,
                PendingDiscussionWithRepresentative = true,
                AdmissionNotesHaveBeenAdded = true,
                HaveBeenScrutinisedByME = true,
                HaveFinalCaseOutcomesOutstanding = true,
                ReadyForMEScrutiny = true,
                Unassigned = true,
                MedicalTeam = medicalTeam,
            };

            return examination;
        }

        private CaseBreakDown GenerateCaseBreakdown()
        {
            return new CaseBreakDown()
            {
                DeathEvent = new DeathEvent()
                {
                    DateOfDeath = DateOfDeath,
                    EventId = "deathEventId",
                    TimeOfDeath = TimeOfDeath,
                    UserId = User0.UserId
                },
                AdmissionNotes = new AdmissionNotesEventContainer()
                {
                    Latest = new AdmissionEvent()
                    {
                        AdmittedDate = LastAdmission,
                        AdmittedDateUnknown = false,
                        EventId = "admissionEventId",
                        ImmediateCoronerReferral = false,
                        IsFinal = true,
                        Notes = AdmissionNotes,
                        UserId = User0.UserId,
                        AdmittedTime = AdmittedTime,
                        AdmittedTimeUnknown = false
                    },
                    History = new[]
                    {
                        new AdmissionEvent()
                        {
                            AdmittedDate = LastAdmission,
                            AdmittedDateUnknown = false,
                            EventId = "admissionEventId",
                            ImmediateCoronerReferral = false,
                            IsFinal = true,
                            Notes = AdmissionNotes,
                            UserId = User0.UserId,
                            AdmittedTime = AdmittedTime,
                            AdmittedTimeUnknown = false
                        },
                        new AdmissionEvent()
                        {
                            AdmittedDate = LastAdmission,
                            AdmittedDateUnknown = false,
                            EventId = "admissionEventId2",
                            ImmediateCoronerReferral = false,
                            IsFinal = true,
                            Notes = AdmissionNotes,
                            UserId = User0.UserId,
                            AdmittedTime = AdmittedTime,
                            AdmittedTimeUnknown = false
                        }
                    },
                    Drafts = new[]
                    {
                        new AdmissionEvent()
                        {
                            AdmittedDate = LastAdmission,
                            AdmittedDateUnknown = false,
                            EventId = "admissionEventId2",
                            ImmediateCoronerReferral = false,
                            IsFinal = false,
                            Notes = AdmissionNotes,
                            UserId = User0.UserId,
                            AdmittedTime = AdmittedTime,
                            AdmittedTimeUnknown = false
                        },
                        new AdmissionEvent()
                        {
                            AdmittedDate = LastAdmission,
                            AdmittedDateUnknown = false,
                            EventId = "admissionEventId2",
                            ImmediateCoronerReferral = false,
                            IsFinal = false,
                            Notes = AdmissionNotes,
                            UserId = User1.UserId,
                            AdmittedTime = AdmittedTime,
                            AdmittedTimeUnknown = false
                        }
                    }
                },
                BereavedDiscussion = new BereavedDiscussionEventContainer()
                {
                    Latest = new BereavedDiscussionEvent()
                    {
                        BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                        DateOfConversation = dateOfConversation,
                        TimeOfConversation = timeOfConversation,
                        DiscussionDetails = discussionDetails,
                        DiscussionUnableHappen = false,
                        EventId = "bereavedDiscussionEventId",
                        InformedAtDeath = InformedAtDeath.Unknown,
                        IsFinal = true,
                        UserId = User0.UserId,
                        ParticipantFullName = ParticipantFullName,
                        ParticipantPhoneNumber = ParticipantPhoneNumber,
                        ParticipantRelationship = ParticipantRelationship,
                        PresentAtDeath = PresentAtDeath.No,
                    },
                    History = new[]
                    {
                        new BereavedDiscussionEvent()
                        {
                            BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            DateOfConversation = dateOfConversation,
                            TimeOfConversation = timeOfConversation,
                            DiscussionDetails = discussionDetails,
                            DiscussionUnableHappen = false,
                            EventId = "bereavedDiscussionEventId",
                            InformedAtDeath = InformedAtDeath.Unknown,
                            IsFinal = true,
                            UserId= User0.UserId,
                            ParticipantFullName = ParticipantFullName,
                            ParticipantPhoneNumber = ParticipantPhoneNumber,
                            ParticipantRelationship = ParticipantRelationship,
                            PresentAtDeath = PresentAtDeath.No,
                        }
                    }
                },
                MedicalHistory = new MedicalHistoryEventContainer()
                {
                    Latest = new MedicalHistoryEvent()
                    {
                        EventId = "MedicalHistoryEventId",
                        IsFinal = true,
                        UserId = User0.UserId,
                        Text = MedicalHistoryEventText,
                    },
                    History = new[]
                    {
                        new MedicalHistoryEvent()
                        {
                            EventId = "MedicalHistoryEventId",
                            IsFinal = true,
                            UserId = User0.UserId,
                            Text = MedicalHistoryEventText,
                        }
                    }
                },
                MeoSummary = new MeoSummaryEventContainer()
                {
                    Latest = new MeoSummaryEvent()
                    {
                        EventId = "MeoSummaryEventId",
                        IsFinal = true,
                        UserId = User0.UserId,
                        SummaryDetails = SummaryDetails
                    },
                    History = new[]
                    {
                        new MeoSummaryEvent()
                        {
                            EventId = "MeoSummaryEventId",
                            IsFinal = true,
                            UserId = User0.UserId,
                            SummaryDetails = SummaryDetails
                        }
                    }
                },
                OtherEvents = new OtherEventContainer
                {

                },
                PreScrutiny = new PreScrutinyEventContainer()
                {
                    Latest = new PreScrutinyEvent()
                    {
                        CauseOfDeath1a = CauseOfDeath1a,
                        CauseOfDeath1b = CauseOfDeath1b,
                        CauseOfDeath1c = CauseOfDeath1c,
                        CauseOfDeath2 = CauseOfDeath2,
                        CircumstancesOfDeath = OverallCircumstancesOfDeath.Unexpected,
                        ClinicalGovernanceReview = ClinicalGovernanceReview.Unknown,
                        ClinicalGovernanceReviewText = ClinicalGovernanceReviewText,
                        EventId = "preScrutinyEventId",
                        IsFinal = true,
                        MedicalExaminerThoughts = MedicalExaminerThoughts,
                        OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a,
                        UserId = User0.UserId,
                        UserFullName = User0.FullName()
                    },
                    History = new[]
                    {
                        new PreScrutinyEvent()
                        {
                            CauseOfDeath1a = CauseOfDeath1a,
                            CauseOfDeath1b = CauseOfDeath1b,
                            CauseOfDeath1c = CauseOfDeath1c,
                            CauseOfDeath2 = CauseOfDeath2,
                            CircumstancesOfDeath = OverallCircumstancesOfDeath.Unexpected,
                            ClinicalGovernanceReview = ClinicalGovernanceReview.Unknown,
                            ClinicalGovernanceReviewText = ClinicalGovernanceReviewText,
                            EventId = "preScrutinyEventId",
                            IsFinal = true,
                            MedicalExaminerThoughts = MedicalExaminerThoughts,
                            OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a,
                            UserId = User0.UserId,
                            UserFullName = User0.FullName()
                        }
                    }
                },
                QapDiscussion = new QapDiscussionEventContainer()
                {
                    Latest = new QapDiscussionEvent()
                    {
                        EventId = "QapDiscussionEventId",
                        IsFinal = true,
                        UserId = User0.UserId,
                        DiscussionDetails = discussionDetails,
                        CauseOfDeath1a = CauseOfDeath1a,
                        CauseOfDeath1b = CauseOfDeath1b,
                        CauseOfDeath1c = CauseOfDeath1c,
                        CauseOfDeath2 = CauseOfDeath2,
                        DiscussionUnableHappen = false,
                        QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                        ParticipantRole = ParticipantRoll,
                        DateOfConversation = dateOfConversation,
                        TimeOfConversation = timeOfConversation,
                        ParticipantName = ParticipantName,
                        ParticipantOrganisation = ParticipantOrganisation,
                        ParticipantPhoneNumber = ParticipantPhoneNumber,
                        UserFullName = User0.FullName()
                    },
                    History = new[]
                    {
                        new QapDiscussionEvent()
                    {
                        EventId = "QapDiscussionEventId",
                        IsFinal = true,
                        UserId = User0.UserId,
                        DiscussionDetails = discussionDetails,
                        CauseOfDeath1a = CauseOfDeath1a,
                        CauseOfDeath1b = CauseOfDeath1b,
                        CauseOfDeath1c = CauseOfDeath1c,
                        CauseOfDeath2 = CauseOfDeath2,
                        DiscussionUnableHappen = false,
                        QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                        ParticipantRole = ParticipantRoll,
                        DateOfConversation = dateOfConversation,
                        TimeOfConversation = timeOfConversation,
                        ParticipantName = ParticipantName,
                        ParticipantOrganisation = ParticipantOrganisation,
                        ParticipantPhoneNumber = ParticipantPhoneNumber
                    }
                    }
                }
            };
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
            response.ExaminationId.Should().Be(expectedExaminationId);
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
            response.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            response.Surname.Should().Be(Surname);
            response.TimeOfDeath.Should().Be(TimeOfDeath);
        }

        [Fact]
        public void Full_Examination_To_Odt_Download()
        {
            var examination = GetExaminationForOdtMapping(true, true, true);
            var expected = new GetCoronerReferralDownloadResponse()
            {
                AbleToIssueMCCD = true,
                AnyImplants = examination.AnyImplants,
                CauseOfDeath1a = "mustang",
                CauseOfDeath1b = "bathroom",
                CauseOfDeath1c = "novotel",
                CauseOfDeath2 = "airbnb",
                Consultant = new ClinicalProfessionalItem()
                {
                    GMCNumber = "122484abcd",
                    Name = "Clarisa Charmeress",
                    Notes = "TOP SECRET",
                    Organisation = "The World Health Organisation",
                    Phone = "999",
                    Role = "Consultant"
                },
                County = "county",
                DateOfBirth = new DateTime(2001, 9, 11),
                DateOfDeath = new DateTime(2019, 8, 5),
                DetailsAboutMedicalHistory = "Some History should never be thought of",
                Gender = ExaminationGender.Female,
                GivenNames = "givenName",
                GP = new ClinicalProfessionalItem()
                {
                    GMCNumber = "11111111111112",
                    Name = "Gary Numan",
                    Notes = "About to retire",
                    Organisation = "The Oaklands Practice",
                    Phone = "1111",
                    Role = "General Practitioner"
                },
                HouseNameNumber = "houseNameNumber",
                ImplantDetails = "implantDetails",
                LatestAdmissionDetails = new AdmissionEventItem()
                {
                    AdmittedDate = new DateTime(2019, 5, 5),
                    AdmittedDateUnknown = false,
                    AdmittedTime = new TimeSpan(11, 11, 00),
                    AdmittedTimeUnknown = false,
                    Created = new DateTime(),
                    EventId = "2",
                    ImmediateCoronerReferral = false,
                    IsFinal = true,
                    Notes = "the admission notes",
                    RouteOfAdmission = RouteOfAdmission.AccidentAndEmergency,
                    UserFullName = "userFullName",
                    UserId = "userId",
                    UsersRole = "userRole"
                },
                LatestBereavedDiscussion = new BereavedDiscussionEventItem()
                {
                    BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                    Created = new DateTime(2019, 8, 12),
                    DateOfConversation = new DateTime(2019, 8, 12),
                    DiscussionDetails = "discussionDetails",
                    DiscussionUnableHappen = false,
                    DiscussionUnableHappenDetails = "unableToHappenDetails",
                    EventId = "1",
                    InformedAtDeath = InformedAtDeath.Yes,
                    IsFinal = true,
                    ParticipantFullName = "Maraget Thatcher",
                    ParticipantPhoneNumber = "9876",
                    ParticipantRelationship = "Wet nurse",
                    PresentAtDeath = PresentAtDeath.Yes,
                    TimeOfConversation = new TimeSpan(13, 13, 0),
                    UserFullName = "userFullName",
                    UserId = "userId",
                    UsersRole = "usersRole"
                },
                NhsNumber = "nhsNumber",
                PlaceOfDeath = "placeDeathOccured",
                Postcode = "postCode",
                Qap = new ClinicalProfessionalItem()
                {
                    GMCNumber = null,
                    Name = "gary link",
                    Notes = "some discussion details",
                    Organisation = "gary links big examination",
                    Phone = "077123456",
                    Role = "SomeRole"
                },
                Street = "street",
                Surname = "surname",
                TimeOfDeath = new TimeSpan(11, 11, 00),
                Town = "town",
            };


            var result = _mapper.Map<GetCoronerReferralDownloadResponse>(examination);
            IsEquivalent(expected, result);
        }

        [Fact]
        public void Nullish_Examination_To_Odt_Download()
        {
            var examination = GetExaminationForOdtMapping(false, false, false);
            var expected = new GetCoronerReferralDownloadResponse()
            {
                AbleToIssueMCCD = false,
                AnyImplants = examination.AnyImplants,
                CauseOfDeath1a = null,
                CauseOfDeath1b = null,
                CauseOfDeath1c = null,
                CauseOfDeath2 = null,
                Consultant = null,
                County = null,
                DateOfBirth = null,
                DateOfDeath = null,
                DetailsAboutMedicalHistory = "",
                Gender = ExaminationGender.Female,
                GivenNames = null,
                GP = null,
                HouseNameNumber = null,
                ImplantDetails = null,
                LatestAdmissionDetails = null,
                LatestBereavedDiscussion = null,
                NhsNumber = null,
                PlaceOfDeath = null,
                Postcode = null,
                Qap = null,
                Street = null,
                Surname = null,
                TimeOfDeath = null,
                Town = null,
            };

            var result = _mapper.Map<GetCoronerReferralDownloadResponse>(examination);
            IsEquivalent(expected, result);
        }

        [Fact]
        public void OnlyPrescrutiny_Examination_To_Odt_Download()
        {
            var examination = GetExaminationForOdtMapping(true, false, true);
            var expected = new GetCoronerReferralDownloadResponse()
            {
                AbleToIssueMCCD = true,
                AnyImplants = examination.AnyImplants,
                CauseOfDeath1a = "apple",
                CauseOfDeath1b = "banana",
                CauseOfDeath1c = "Cucumber",
                CauseOfDeath2 = "doughnut",
                Consultant = new ClinicalProfessionalItem()
                {
                    GMCNumber = "122484abcd",
                    Name = "Clarisa Charmeress",
                    Notes = "TOP SECRET",
                    Organisation = "The World Health Organisation",
                    Phone = "999",
                    Role = "Consultant"
                },
                County = "county",
                DateOfBirth = new DateTime(2001, 9, 11),
                DateOfDeath = new DateTime(2019, 8, 5),
                DetailsAboutMedicalHistory = "Some History should never be thought of",
                Gender = ExaminationGender.Female,
                GivenNames = "givenName",
                GP = new ClinicalProfessionalItem()
                {
                    GMCNumber = "11111111111112",
                    Name = "Gary Numan",
                    Notes = "About to retire",
                    Organisation = "The Oaklands Practice",
                    Phone = "1111",
                    Role = "General Practitioner"
                },
                HouseNameNumber = "houseNameNumber",
                ImplantDetails = "implantDetails",
                LatestAdmissionDetails = new AdmissionEventItem()
                {
                    AdmittedDate = new DateTime(2019, 5, 5),
                    AdmittedDateUnknown = false,
                    AdmittedTime = new TimeSpan(11, 11, 00),
                    AdmittedTimeUnknown = false,
                    Created = new DateTime(),
                    EventId = "2",
                    ImmediateCoronerReferral = false,
                    IsFinal = true,
                    Notes = "the admission notes",
                    RouteOfAdmission = RouteOfAdmission.AccidentAndEmergency,
                    UserFullName = "userFullName",
                    UserId = "userId",
                    UsersRole = "userRole"
                },
                LatestBereavedDiscussion = new BereavedDiscussionEventItem()
                {
                    BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                    Created = new DateTime(2019, 8, 12),
                    DateOfConversation = new DateTime(2019, 8, 12),
                    DiscussionDetails = "discussionDetails",
                    DiscussionUnableHappen = false,
                    DiscussionUnableHappenDetails = "unableToHappenDetails",
                    EventId = "1",
                    InformedAtDeath = InformedAtDeath.Yes,
                    IsFinal = true,
                    ParticipantFullName = "Maraget Thatcher",
                    ParticipantPhoneNumber = "9876",
                    ParticipantRelationship = "Wet nurse",
                    PresentAtDeath = PresentAtDeath.Yes,
                    TimeOfConversation = new TimeSpan(13, 13, 0),
                    UserFullName = "userFullName",
                    UserId = "userId",
                    UsersRole = "usersRole"
                },
                NhsNumber = "nhsNumber",
                PlaceOfDeath = "placeDeathOccured",
                Postcode = "postCode",
                Qap = null,
                Street = "street",
                Surname = "surname",
                TimeOfDeath = new TimeSpan(11, 11, 00),
                Town = "town",
            };


            var result = _mapper.Map<GetCoronerReferralDownloadResponse>(examination);
            IsEquivalent(expected, result);

        }

        [Fact]
        public void OnlyQapDiscussion_Examination_To_Odt_Download()
        {
            var examination = GetExaminationForOdtMapping(false, true, true);
            var expected = new GetCoronerReferralDownloadResponse()
            {
                AbleToIssueMCCD = true,
                AnyImplants = examination.AnyImplants,
                CauseOfDeath1a = "mustang",
                CauseOfDeath1b = "bathroom",
                CauseOfDeath1c = "novotel",
                CauseOfDeath2 = "airbnb",
                Consultant = new ClinicalProfessionalItem()
                {
                    GMCNumber = "122484abcd",
                    Name = "Clarisa Charmeress",
                    Notes = "TOP SECRET",
                    Organisation = "The World Health Organisation",
                    Phone = "999",
                    Role = "Consultant"
                },
                County = "county",
                DateOfBirth = new DateTime(2001, 9, 11),
                DateOfDeath = new DateTime(2019, 8, 5),
                DetailsAboutMedicalHistory = "Some History should never be thought of",
                Gender = ExaminationGender.Female,
                GivenNames = "givenName",
                GP = new ClinicalProfessionalItem()
                {
                    GMCNumber = "11111111111112",
                    Name = "Gary Numan",
                    Notes = "About to retire",
                    Organisation = "The Oaklands Practice",
                    Phone = "1111",
                    Role = "General Practitioner"
                },
                HouseNameNumber = "houseNameNumber",
                ImplantDetails = "implantDetails",
                LatestAdmissionDetails = new AdmissionEventItem()
                {
                    AdmittedDate = new DateTime(2019, 5, 5),
                    AdmittedDateUnknown = false,
                    AdmittedTime = new TimeSpan(11, 11, 00),
                    AdmittedTimeUnknown = false,
                    Created = new DateTime(),
                    EventId = "2",
                    ImmediateCoronerReferral = false,
                    IsFinal = true,
                    Notes = "the admission notes",
                    RouteOfAdmission = RouteOfAdmission.AccidentAndEmergency,
                    UserFullName = "userFullName",
                    UserId = "userId",
                    UsersRole = "userRole"
                },
                LatestBereavedDiscussion = new BereavedDiscussionEventItem()
                {
                    BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                    Created = new DateTime(2019, 8, 12),
                    DateOfConversation = new DateTime(2019, 8, 12),
                    DiscussionDetails = "discussionDetails",
                    DiscussionUnableHappen = false,
                    DiscussionUnableHappenDetails = "unableToHappenDetails",
                    EventId = "1",
                    InformedAtDeath = InformedAtDeath.Yes,
                    IsFinal = true,
                    ParticipantFullName = "Maraget Thatcher",
                    ParticipantPhoneNumber = "9876",
                    ParticipantRelationship = "Wet nurse",
                    PresentAtDeath = PresentAtDeath.Yes,
                    TimeOfConversation = new TimeSpan(13, 13, 0),
                    UserFullName = "userFullName",
                    UserId = "userId",
                    UsersRole = "usersRole"
                },
                NhsNumber = "nhsNumber",
                PlaceOfDeath = "placeDeathOccured",
                Postcode = "postCode",
                Qap = new ClinicalProfessionalItem()
                {
                    GMCNumber = null,
                    Name = "gary link",
                    Notes = "some discussion details",
                    Organisation = "gary links big examination",
                    Phone = "077123456",
                    Role = "SomeRole"
                },
                Street = "street",
                Surname = "surname",
                TimeOfDeath = new TimeSpan(11, 11, 00),
                Town = "town",
            };

            var result = _mapper.Map<GetCoronerReferralDownloadResponse>(examination);
            IsEquivalent(expected, result);
        }

        private Examination GetExaminationForOdtMapping(bool hasPrescrutiny, bool hasQapDiscussion, bool generalDetails)
        {
            PreScrutinyEvent latestPrescrutiny = null;
            if (hasPrescrutiny)
            {
                latestPrescrutiny = new PreScrutinyEvent()
                {
                    CauseOfDeath1a = "apple",
                    CauseOfDeath1b = "banana",
                    CauseOfDeath1c = "Cucumber",
                    CauseOfDeath2 = "doughnut",
                    CircumstancesOfDeath = OverallCircumstancesOfDeath.Unexpected,
                    ClinicalGovernanceReview = ClinicalGovernanceReview.Yes,
                    ClinicalGovernanceReviewText = "So much for 5 a day",
                    Created = null,
                    EventId = "prescrutinyEventId",
                    IsFinal = true,
                    MedicalExaminerThoughts = "musings",
                    OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                    UserFullName = "bob marley",
                    UserId = "BobMarley",
                    UsersRole = "MedicalExaminer"
                };
            }

            QapDiscussionEvent latestQapDiscussionEvent = null;
            if (hasQapDiscussion)
            {
                latestQapDiscussionEvent = new QapDiscussionEvent()
                {
                    CauseOfDeath1a = "mustang",
                    CauseOfDeath1b = "bathroom",
                    CauseOfDeath1c = "novotel",
                    CauseOfDeath2 = "airbnb",
                    Created = null,
                    DateOfConversation = null,
                    DiscussionDetails = "some discussion details",
                    DiscussionUnableHappen = false,
                    EventId = "qapDiscussionEventId",
                    IsFinal = true,
                    ParticipantName = "gary link",
                    ParticipantOrganisation = "gary links big examination",
                    ParticipantPhoneNumber = "077123456",
                    ParticipantRole = "SomeRole",
                    QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP,
                    TimeOfConversation = null,
                    UserFullName = "The Tax Man",
                    UserId = "TaxMan",
                    UsersRole = "Tax Man"
                };
            }

            MedicalHistoryEvent medicalHistoryEvent = null;
            AdmissionEvent admissionEvent = null;
            BereavedDiscussionEvent bereavedDiscussion = null;
            CaseOutcome caseOutcome = null;
            Representative[] representatives = null;
            TimeSpan? timeOfDeath = null;
            DateTime? dateOfBirth = null;
            DateTime? dateOfDeath = null;
            DateTime? lastAdmission = null;
            ClinicalProfessional consultant = null;
            ClinicalProfessional qap = null;
            ClinicalProfessional gp = null;

            if (generalDetails)
            {
                timeOfDeath = new TimeSpan(11, 11, 00);
                dateOfBirth = new DateTime(2001, 9, 11);
                dateOfDeath = new DateTime(2019, 8, 5);
                lastAdmission = new DateTime(2019, 5, 5);

                medicalHistoryEvent = new MedicalHistoryEvent()
                {
                    Text = "Some History should never be thought of"
                };

                gp = new ClinicalProfessional()
                {
                    GMCNumber = "11111111111112",
                    Name = "Gary Numan",
                    Notes = "About to retire",
                    Organisation = "The Oaklands Practice",
                    Phone = "1111",
                    Role = "General Practitioner"
                };

                consultant = new ClinicalProfessional()
                {
                    GMCNumber = "122484abcd",
                    Name = "Clarisa Charmeress",
                    Notes = "TOP SECRET",
                    Organisation = "The World Health Organisation",
                    Phone = "999",
                    Role = "Consultant"
                };

                qap = new ClinicalProfessional()
                {
                    GMCNumber = "112484abcd",
                    Name = "Jermimer Puddleduck",
                    Notes = "Its great weather for ducks outside",
                    Organisation = "General Staffing Pool",
                    Phone = "111",
                    Role = "QAP"
                };

                admissionEvent = new AdmissionEvent()
                {
                    AdmittedDate = new DateTime(2019, 5, 5),
                    AdmittedDateUnknown = false,
                    AdmittedTime = new TimeSpan(11, 11, 00),
                    AdmittedTimeUnknown = false,
                    Created = new DateTime(),
                    EventId = "2",
                    ImmediateCoronerReferral = false,
                    IsFinal = true,
                    Notes = "the admission notes",
                    RouteOfAdmission = RouteOfAdmission.AccidentAndEmergency,
                    UserFullName = "userFullName",
                    UserId = "userId",
                    UsersRole = "userRole"
                };

                caseOutcome = new CaseOutcome()
                {
                    CaseCompleted = true,
                    CaseMedicalExaminerFullName = "caseMedicalExaminerFullName",
                    CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD,
                    CoronerReferralSent = true,
                    CremationFormStatus = CremationFormStatus.Yes,
                    GpNotifiedStatus = GPNotified.GPNotified,
                    MccdIssued = true,
                    OutcomeOfPrescrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                    OutcomeOfRepresentativeDiscussion = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                    OutcomeQapDiscussion = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                    ScrutinyConfirmedOn = new DateTime()
                };

                representatives = new Representative[]
                {
                    new Representative()
                    {
                        AppointmentDate = new DateTime(2019, 8, 12),
                        AppointmentTime = new TimeSpan(14, 45, 00),
                        FullName = "Alexander Boris de Pfeffel Johnson",
                        Informed = InformedAtDeath.No,
                        Notes = "Plonker",
                        PhoneNumber = "888",
                        PresentAtDeath = PresentAtDeath.Yes,
                        Relationship = "Embarrassing Uncle"
                    }
                };

                bereavedDiscussion = new BereavedDiscussionEvent()
                {
                    BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                    Created = new DateTime(2019, 8, 12),
                    DateOfConversation = new DateTime(2019, 8, 12),
                    DiscussionDetails = "discussionDetails",
                    DiscussionUnableHappen = false,
                    DiscussionUnableHappenDetails = "unableToHappenDetails",
                    EventId = "1",
                    InformedAtDeath = InformedAtDeath.Yes,
                    IsFinal = true,
                    ParticipantFullName = "Maraget Thatcher",
                    ParticipantPhoneNumber = "9876",
                    ParticipantRelationship = "Wet nurse",
                    PresentAtDeath = PresentAtDeath.Yes,
                    TimeOfConversation = new TimeSpan(13, 13, 0),
                    UserFullName = "userFullName",
                    UserId = "userId",
                    UsersRole = "usersRole"
                };
            }

            return new Examination()
            {
                AdmissionNotesHaveBeenAdded = generalDetails ? true : false,
                AnyImplants = generalDetails ? true : false,
                AnyPersonalEffects = generalDetails ? true : false,
                CaseBreakdown = new CaseBreakDown()
                {
                    AdmissionNotes = new AdmissionNotesEventContainer()
                    {
                        Latest = admissionEvent,
                        Drafts = null,
                        History = null
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer()
                    {
                        Latest = bereavedDiscussion,
                        Drafts = null,
                        History = null
                    },
                    CaseClosedEvent = null,
                    DeathEvent = null,
                    MedicalHistory = new MedicalHistoryEventContainer()
                    {
                        Latest = medicalHistoryEvent,
                        Drafts = null,
                        History = new[] { medicalHistoryEvent }
                    },
                    MeoSummary = new MeoSummaryEventContainer()
                    {
                        Latest = null,
                        Drafts = null,
                        History = null
                    },
                    OtherEvents = new OtherEventContainer()
                    {
                        Latest = null,
                        Drafts = null,
                        History = null
                    },
                    PreScrutiny = new PreScrutinyEventContainer()
                    {
                        Latest = latestPrescrutiny,
                        Drafts = null,
                        History = null
                    },
                    QapDiscussion = new QapDiscussionEventContainer()
                    {
                        Latest = latestQapDiscussionEvent,
                        Drafts = null,
                        History = null
                    }
                },
                CaseCompleted = generalDetails ? true : false,
                CaseOutcome = caseOutcome,
                ChildPriority = generalDetails ? true : false,
                ConfirmationOfScrutinyCompletedAt = null,
                ConfirmationOfScrutinyCompletedBy = generalDetails ? "Dan Ooka" : null,
                CoronerPriority = generalDetails ? true : false,
                CoronerReferralSent = generalDetails ? true : false,
                CoronerStatus = CoronerStatus.None,
                Country = generalDetails ? "country" : null,
                County = generalDetails ? "county" : null,
                CreatedAt = DateTime.Now,
                CreatedBy = generalDetails ? "Dan Ooka" : null,
                CulturalPriority = generalDetails ? true : false,
                DateOfBirth = dateOfBirth,
                DateOfDeath = dateOfDeath,
                DeletedAt = null,
                ExaminationId = "examinationId",
                FaithPriority = generalDetails ? true : false,
                FuneralDirectors = generalDetails ? "easyBurials" : null,
                Gender = ExaminationGender.Female,
                GenderDetails = generalDetails ? "genderDetails" : null,
                GivenNames = generalDetails ? "givenName" : null,
                HaveBeenScrutinisedByME = generalDetails ? true : false,
                HaveFinalCaseOutcomesOutstanding = generalDetails ? true : false,
                HospitalNumber_1 = generalDetails ? "hospitalNumber_1" : null,
                HospitalNumber_2 = generalDetails ? "hospitalNumber_2" : null,
                HospitalNumber_3 = generalDetails ? "hospitalNumber_3" : null,
                HouseNameNumber = generalDetails ? "houseNameNumber" : null,
                ImplantDetails = generalDetails ? "implantDetails" : null,
                LastAdmission = lastAdmission,
                LastModifiedBy = null,
                LastOccupation = generalDetails ? "lastOccupation" : null,
                MedicalExaminerOfficeResponsible = generalDetails ? "medicalExaminerOfficeId" : null,
                MedicalExaminerOfficeResponsibleName = generalDetails ? "medicalExaminerOfficeName" : null,
                MedicalTeam = new MedicalTeam()
                {
                    ConsultantResponsible = consultant,
                    ConsultantsOther = null,
                    GeneralPractitioner = gp,
                    MedicalExaminerFullName = generalDetails ? "medicalExaminerName" : null,
                    MedicalExaminerOfficerFullName = generalDetails ? "medicalExaminerOfficerName" : null,
                    MedicalExaminerOfficerUserId = generalDetails ? "medicalExaminerOfficerId" : null,
                    MedicalExaminerUserId = generalDetails ? "medicalExaminerId" : null,
                    NursingTeamInformation = null,
                    Qap = qap
                },
                ModeOfDisposal = ModeOfDisposal.Unknown,
                ModifiedAt = new DateTimeOffset(),
                NationalLocationId = generalDetails ? "nationalLocationId" : null,
                NhsNumber = generalDetails ? "nhsNumber" : null,
                OrganisationCareBeforeDeathLocationId = generalDetails ? "organisationCareBeforeDeathLocationId" : null,
                OtherPriority = generalDetails ? true : false,
                OutstandingCaseItemsCompleted = generalDetails ? true : false,
                PendingAdmissionNotes = generalDetails ? true : false,
                PendingDiscussionWithQAP = generalDetails ? true : false,
                PendingDiscussionWithRepresentative = generalDetails ? true : false,
                PendingScrutinyNotes = generalDetails ? true : false,
                PersonalEffectDetails = generalDetails ? "personalEffectDetails" : null,
                PlaceDeathOccured = generalDetails ? "placeDeathOccured" : null,
                Postcode = generalDetails ? "postCode" : null,
                PriorityDetails = generalDetails ? "priorityDetails" : null,
                ReadyForMEScrutiny = generalDetails ? true : false,
                RegionLocationId = generalDetails ? "regionLocationId" : null,
                Representatives = representatives,
                ScrutinyConfirmed = generalDetails ? true : false,
                SiteLocationId = generalDetails ? "siteLocationId" : null,
                Street = generalDetails ? "street" : null,
                Surname = generalDetails ? "surname" : null,
                TimeOfDeath = timeOfDeath,
                Town = generalDetails ? "town" : null,
                TrustLocationId = generalDetails ? "trustLocationId" : null,
                Unassigned = generalDetails ? true : false,
            };
        }

        private void IsEquivalent(GetCoronerReferralDownloadResponse expected, GetCoronerReferralDownloadResponse actual)
        {
            actual.AbleToIssueMCCD.Should().Be(expected.AbleToIssueMCCD);
            actual.AnyImplants.Should().Be(expected.AnyImplants);
            actual.CauseOfDeath1a.Should().Be(expected.CauseOfDeath1a);
            actual.CauseOfDeath1b.Should().Be(expected.CauseOfDeath1b);
            actual.CauseOfDeath1c.Should().Be(expected.CauseOfDeath1c);
            actual.CauseOfDeath2.Should().Be(expected.CauseOfDeath2);
            actual.Consultant.Should().BeEquivalentTo(expected.Consultant);
            actual.County.Should().Be(expected.County);
            actual.DateOfBirth.Should().Be(expected.DateOfBirth);
            actual.DateOfDeath.Should().Be(expected.DateOfDeath);
            actual.DetailsAboutMedicalHistory.Should().Be(expected.DetailsAboutMedicalHistory);
            actual.Gender.Should().Be(expected.Gender);
            actual.GivenNames.Should().Be(expected.GivenNames);
            actual.GP.Should().BeEquivalentTo(expected.GP);
            actual.HouseNameNumber.Should().Be(expected.HouseNameNumber);
            actual.ImplantDetails.Should().Be(expected.ImplantDetails);
            actual.LatestAdmissionDetails.Should().BeEquivalentTo(expected.LatestAdmissionDetails);
            actual.LatestBereavedDiscussion.Should().BeEquivalentTo(expected.LatestBereavedDiscussion);
            actual.NhsNumber.Should().Be(expected.NhsNumber);
            actual.PlaceOfDeath.Should().Be(expected.PlaceOfDeath);
            actual.Postcode.Should().Be(expected.Postcode);
            actual.Qap.Should().BeEquivalentTo(expected.Qap);
            actual.Street.Should().Be(expected.Street);
            actual.Surname.Should().Be(expected.Surname);
            actual.TimeOfDeath.Should().Be(expected.TimeOfDeath);
            actual.Town.Should().Be(expected.Town);
        }

        [Fact]
        public void BereavedDiscussionPrepopulated_Examination_To_Casebreakdown_No_PrescrutinyLatest()
        {
            // Arrange
            var casebreakdown = GenerateCaseBreakdown();
            var dateNow = DateTime.Now;

            casebreakdown.PreScrutiny.Latest = null;
            casebreakdown.PreScrutiny.History = new List<PreScrutinyEvent>();
            var examination = GenerateExamination(casebreakdown);
            examination.Representatives = Representatives;

            var expectedBereavedPrepopulated = new BereavedDiscussionPrepopulated()
            {
                Representatives = examination.Representatives,
                CauseOfDeath1a = casebreakdown.QapDiscussion.Latest.CauseOfDeath1a,
                CauseOfDeath1b = casebreakdown.QapDiscussion.Latest.CauseOfDeath1b,
                CauseOfDeath1c = casebreakdown.QapDiscussion.Latest.CauseOfDeath1c,
                CauseOfDeath2 = casebreakdown.QapDiscussion.Latest.CauseOfDeath2,
                DateOfLatestPreScrutiny = null,
                DateOfLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.DateOfConversation,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyNotHappened,
                QAPDiscussionStatus = QAPDiscussionStatus.HappenedNoRevision,
                QAPNameForLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.ParticipantName,
                UserForLatestPrescrutiny = null,
                UserForLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.UserFullName
            };

            // Act
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            // Assert
            AreEquivalent(expectedBereavedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void BereavedDiscussionPrepopulated_Examination_To_Casebreakdown_PrescrutinyLatest_Same_As_QapLatest()
        {
            // Arrange
            var casebreakdown = GenerateCaseBreakdown();
            var dateNow = DateTime.Now.Date;
            casebreakdown.PreScrutiny.Latest.Created = dateNow;
            var examination = GenerateExamination(casebreakdown);
            examination.Representatives = Representatives;

            var expectedBereavedPrepopulated = new BereavedDiscussionPrepopulated()
            {
                Representatives = examination.Representatives,
                CauseOfDeath1a = casebreakdown.QapDiscussion.Latest.CauseOfDeath1a,
                CauseOfDeath1b = casebreakdown.QapDiscussion.Latest.CauseOfDeath1b,
                CauseOfDeath1c = casebreakdown.QapDiscussion.Latest.CauseOfDeath1c,
                CauseOfDeath2 = casebreakdown.QapDiscussion.Latest.CauseOfDeath2,
                DateOfLatestPreScrutiny = casebreakdown.PreScrutiny.Latest.Created,
                DateOfLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.DateOfConversation,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                QAPDiscussionStatus = QAPDiscussionStatus.HappenedNoRevision,
                QAPNameForLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.ParticipantName,
                UserForLatestPrescrutiny = casebreakdown.PreScrutiny.Latest.UserFullName,
                UserForLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.UserFullName
            };

            // Act
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            // Assert
            AreEquivalent(expectedBereavedPrepopulated, result.BereavedDiscussion.Prepopulated);

        }

        [Fact]
        public void BereavedDiscussionPrepopulated_Examination_To_Casebreakdown_PrescrutinyLatest_Different_To_QapLatest()
        {
            // Arrange
            var casebreakdown = GenerateCaseBreakdown();
            casebreakdown.QapDiscussion.Latest.CauseOfDeath1a = "big banana";
            var dateNow = DateTime.Now.Date;
            casebreakdown.PreScrutiny.Latest.Created = dateNow;
            var examination = GenerateExamination(casebreakdown);
            examination.Representatives = Representatives;

            var expectedBereavedPrepopulated = new BereavedDiscussionPrepopulated()
            {
                Representatives = examination.Representatives,
                CauseOfDeath1a = casebreakdown.QapDiscussion.Latest.CauseOfDeath1a,
                CauseOfDeath1b = casebreakdown.QapDiscussion.Latest.CauseOfDeath1b,
                CauseOfDeath1c = casebreakdown.QapDiscussion.Latest.CauseOfDeath1c,
                CauseOfDeath2 = casebreakdown.QapDiscussion.Latest.CauseOfDeath2,
                DateOfLatestPreScrutiny = casebreakdown.PreScrutiny.Latest.Created,
                DateOfLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.DateOfConversation,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                QAPDiscussionStatus = QAPDiscussionStatus.HappenedWithRevisions,
                QAPNameForLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.ParticipantName,
                UserForLatestPrescrutiny = casebreakdown.PreScrutiny.Latest.UserFullName,
                UserForLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.UserFullName
            };

            // Act
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            // Assert
            AreEquivalent(expectedBereavedPrepopulated, result.BereavedDiscussion.Prepopulated);

        }

        [Fact]
        public void BereavedDiscussionPrepopulated_Examination_To_Casebreakdown_No_LatestQapDiscussion()
        {
            // Arrange
            var casebreakdown = GenerateCaseBreakdown();
            var dateNow = DateTime.Now.Date;
            casebreakdown.PreScrutiny.Latest.Created = dateNow;
            casebreakdown.QapDiscussion.Latest = null;
            casebreakdown.QapDiscussion.History = new List<QapDiscussionEvent>();
            var examination = GenerateExamination(casebreakdown);
            examination.Representatives = Representatives;

            var expectedBereavedPrepopulated = new BereavedDiscussionPrepopulated()
            {
                Representatives = examination.Representatives,
                CauseOfDeath1a = casebreakdown.PreScrutiny.Latest.CauseOfDeath1a,
                CauseOfDeath1b = casebreakdown.PreScrutiny.Latest.CauseOfDeath1b,
                CauseOfDeath1c = casebreakdown.PreScrutiny.Latest.CauseOfDeath1c,
                CauseOfDeath2 = casebreakdown.PreScrutiny.Latest.CauseOfDeath2,
                DateOfLatestPreScrutiny = casebreakdown.PreScrutiny.Latest.Created,
                DateOfLatestQAPDiscussion = null,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                QAPDiscussionStatus = QAPDiscussionStatus.NoRecord,
                QAPNameForLatestQAPDiscussion = null,
                UserForLatestPrescrutiny = casebreakdown.PreScrutiny.Latest.UserFullName,
                UserForLatestQAPDiscussion = null
            };

            // Act
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            // Assert
            AreEquivalent(expectedBereavedPrepopulated, result.BereavedDiscussion.Prepopulated);

        }

        [Fact]
        public void BereavedDiscussionPrepopulated_Examination_To_Casebreakdown_No_LatestQapDiscussionDidNotHappen()
        {
            // Arrange
            var casebreakdown = GenerateCaseBreakdown();
            var dateNow = DateTime.Now.Date;
            casebreakdown.PreScrutiny.Latest.Created = dateNow;
            casebreakdown.QapDiscussion.Latest.DiscussionUnableHappen = true;

            var examination = GenerateExamination(casebreakdown);
            examination.Representatives = Representatives;

            var expectedBereavedPrepopulated = new BereavedDiscussionPrepopulated()
            {
                Representatives = examination.Representatives,
                CauseOfDeath1a = casebreakdown.PreScrutiny.Latest.CauseOfDeath1a,
                CauseOfDeath1b = casebreakdown.PreScrutiny.Latest.CauseOfDeath1b,
                CauseOfDeath1c = casebreakdown.PreScrutiny.Latest.CauseOfDeath1c,
                CauseOfDeath2 = casebreakdown.PreScrutiny.Latest.CauseOfDeath2,
                DateOfLatestPreScrutiny = casebreakdown.PreScrutiny.Latest.Created,
                DateOfLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.DateOfConversation,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                QAPDiscussionStatus = QAPDiscussionStatus.CouldNotHappen,
                QAPNameForLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.ParticipantName,
                UserForLatestPrescrutiny = casebreakdown.PreScrutiny.Latest.UserFullName,
                UserForLatestQAPDiscussion = casebreakdown.QapDiscussion.Latest.UserFullName
            };

            // Act
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            // Assert
            AreEquivalent(expectedBereavedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void BereavedDiscussionPrepopulated_Examination_To_Casebreakdown_No_LatestQapDiscussion_No_LatestPrescrutiny()
        {
            // Arrange
            var casebreakdown = GenerateCaseBreakdown();
            var dateNow = DateTime.Now.Date;
            casebreakdown.PreScrutiny.Latest.Created = dateNow;
            casebreakdown.QapDiscussion.Latest = null;
            casebreakdown.QapDiscussion.History = new List<QapDiscussionEvent>();
            casebreakdown.PreScrutiny.Latest = null;
            casebreakdown.PreScrutiny.History = new List<PreScrutinyEvent>();
            var examination = GenerateExamination(casebreakdown);
            examination.Representatives = Representatives;

            var expectedBereavedPrepopulated = new BereavedDiscussionPrepopulated()
            {
                Representatives = examination.Representatives,
                CauseOfDeath1a = null,
                CauseOfDeath1b = null,
                CauseOfDeath1c = null,
                CauseOfDeath2 = null,
                DateOfLatestPreScrutiny = null,
                DateOfLatestQAPDiscussion = null,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyNotHappened,
                QAPDiscussionStatus = QAPDiscussionStatus.NoRecord,
                QAPNameForLatestQAPDiscussion = null,
                UserForLatestPrescrutiny = null,
                UserForLatestQAPDiscussion = null
            };

            // Act
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            // Assert
            AreEquivalent(expectedBereavedPrepopulated, result.BereavedDiscussion.Prepopulated);
        }

        [Fact]
        public void QapDiscussionPrepopulated_Examination_To_Casebreakdown_LatestPreScrutinyDiscussion()
        {
            // Arrange
            var casebreakdown = GenerateCaseBreakdown();
            var dateNow = DateTime.Now.Date;

            var examination = GenerateExamination(casebreakdown);
            examination.MedicalTeam = medicalTeam;

            var expectedQapPrepopulated = new QapDiscussionPrepopulated()
            {
                Qap = examination.MedicalTeam.Qap,
                CauseOfDeath1a = casebreakdown.PreScrutiny.Latest.CauseOfDeath1a,
                CauseOfDeath1b = casebreakdown.PreScrutiny.Latest.CauseOfDeath1b,
                CauseOfDeath1c = casebreakdown.PreScrutiny.Latest.CauseOfDeath1c,
                CauseOfDeath2 = casebreakdown.PreScrutiny.Latest.CauseOfDeath2,
                DateOfLatestPreScrutiny = casebreakdown.PreScrutiny.Latest.Created,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyHappened,
                UserForLatestPrescrutiny = casebreakdown.PreScrutiny.Latest.UserFullName,
            };

            // Act
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            // Assert
            AreEquivalent(expectedQapPrepopulated, result.QapDiscussion.Prepopulated);
        }

        [Fact]
        public void QapDiscussionPrepopulated_Examination_To_Casebreakdown_No_LatestPreScrutinyDiscussion()
        {
            // Arrange
            var casebreakdown = GenerateCaseBreakdown();
            casebreakdown.PreScrutiny.Latest = null;
            casebreakdown.PreScrutiny.History = new List<PreScrutinyEvent>();
            var dateNow = DateTime.Now.Date;

            var examination = GenerateExamination(casebreakdown);
            examination.MedicalTeam = medicalTeam;

            var expectedQapPrepopulated = new QapDiscussionPrepopulated()
            {
                Qap = examination.MedicalTeam.Qap,
                CauseOfDeath1a = null,
                CauseOfDeath1b = null,
                CauseOfDeath1c = null,
                CauseOfDeath2 = null,
                DateOfLatestPreScrutiny = null,
                MedicalExaminer = examination.MedicalTeam.MedicalExaminerFullName,
                PreScrutinyStatus = PreScrutinyStatus.PrescrutinyNotHappened,
                UserForLatestPrescrutiny = null,
            };

            // Act
            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = User0);

            // Assert
            AreEquivalent(expectedQapPrepopulated, result.QapDiscussion.Prepopulated);
        }

        private void AreEquivalent(BereavedDiscussionPrepopulated expectedBereavedPrepopulated, BereavedDiscussionPrepopulated prepopulated)
        {
            prepopulated.CauseOfDeath1a.Should().Be(expectedBereavedPrepopulated.CauseOfDeath1a);
            prepopulated.CauseOfDeath1b.Should().Be(expectedBereavedPrepopulated.CauseOfDeath1b);
            prepopulated.CauseOfDeath1c.Should().Be(expectedBereavedPrepopulated.CauseOfDeath1c);
            prepopulated.CauseOfDeath2.Should().Be(expectedBereavedPrepopulated.CauseOfDeath2);
            prepopulated.DateOfLatestPreScrutiny.Should().Be(expectedBereavedPrepopulated.DateOfLatestPreScrutiny);
            prepopulated.DateOfLatestQAPDiscussion.Should().Be(expectedBereavedPrepopulated.DateOfLatestQAPDiscussion);
            prepopulated.MedicalExaminer.Should().Be(expectedBereavedPrepopulated.MedicalExaminer);
            prepopulated.PreScrutinyStatus.Should().Be(expectedBereavedPrepopulated.PreScrutinyStatus);
            prepopulated.QAPDiscussionStatus.Should().Be(expectedBereavedPrepopulated.QAPDiscussionStatus);
            prepopulated.QAPNameForLatestQAPDiscussion.Should().Be(expectedBereavedPrepopulated.QAPNameForLatestQAPDiscussion);
            prepopulated.UserForLatestPrescrutiny.Should().Be(expectedBereavedPrepopulated.UserForLatestPrescrutiny);
            prepopulated.UserForLatestQAPDiscussion.Should().Be(expectedBereavedPrepopulated.UserForLatestQAPDiscussion);
        }

        private void AreEquivalent(QapDiscussionPrepopulated expectedQapPrepopulated, QapDiscussionPrepopulated prepopulated)
        {
            prepopulated.CauseOfDeath1a.Should().Be(expectedQapPrepopulated.CauseOfDeath1a);
            prepopulated.CauseOfDeath1b.Should().Be(expectedQapPrepopulated.CauseOfDeath1b);
            prepopulated.CauseOfDeath1c.Should().Be(expectedQapPrepopulated.CauseOfDeath1c);
            prepopulated.CauseOfDeath2.Should().Be(expectedQapPrepopulated.CauseOfDeath2);
            prepopulated.DateOfLatestPreScrutiny.Should().Be(expectedQapPrepopulated.DateOfLatestPreScrutiny);
            prepopulated.MedicalExaminer.Should().Be(expectedQapPrepopulated.MedicalExaminer);
            prepopulated.PreScrutinyStatus.Should().Be(expectedQapPrepopulated.PreScrutinyStatus);
            prepopulated.UserForLatestPrescrutiny.Should().Be(expectedQapPrepopulated.UserForLatestPrescrutiny);
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
