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
        private const int UrgencyScore = 4;
        private DateTime CaseCreated = new DateTime(2019, 3, 15);
        private DateTime LastAdmission = new DateTime(2019, 1, 15);
        private TimeSpan TimeOfDeath = new TimeSpan(11, 30, 00);
        private const string CaseOfficer = "CaseOfficer";
        private const string AdmissionNotes = "admissionNotes";
        private TimeSpan AdmittedTime = new TimeSpan(12, 30, 01);
        private MeUser User0 = new MeUser()
        {
            UserId = "userId0",
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
                cfg.AddProfile<CaseBreakdownProfile>();
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
        public void Examination_To_GetCaseBreakdowResponse()
        {
            var examination = GenerateExamination();
            var result = _mapper.Map<CaseBreakDownItem>(examination.CaseBreakdown, opt => opt.Items["user"] = User0);

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

        private bool IsEqual(BaseEventContainter<BereavedDiscussionEvent> bereavedDiscussion1, EventContainerItem<BereavedDiscussionEventItem, NullPrepopulated> bereavedDiscussion2)
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

        public void Examination_To_PatientCard_One_Representative_Appointment_Details()
        {
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
            var result = _mapper.Map<PatientCardItem>(examination);

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

            var result = _mapper.Map<PatientCardItem>(examination);

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



        private Examination GenerateExamination()
        {
            var examination = new Examination
            {
                ExaminationId = ExaminationId,
                CaseBreakdown = GenerateCaseBreakdown(),
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
                Representatives = null,
                Surname = Surname,
                Street = Street,
                Town = Town,
                TimeOfDeath = TimeOfDeath,
                UrgencyScore = UrgencyScore,
                LastAdmission = LastAdmission,
                PendingAdmissionNotes = true,
                PendingDiscussionWithQAP = true,
                PendingDiscussionWithRepresentative = true,
                AdmissionNotesHaveBeenAdded = true,
                HaveBeenScrutinisedByME = true,
                HaveFinalCaseOutcomesOutstanding = true,
                ReadyForMEScrutiny = true,
                Unassigned = true,
                MedicalTeam = medicalTeam
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
                        UserId = User0.UserId
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
                            UserId = User0.UserId
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
                        ParticipantPhoneNumber = ParticipantPhoneNumber
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