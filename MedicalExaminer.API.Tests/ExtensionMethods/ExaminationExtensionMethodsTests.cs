using System;
using System.Linq;
using FluentAssertions;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.ExtensionMethods
{
    public class ExaminationExtensionMethodsTests
    {
        [Fact]
        public void CreateDraftEventForUserReturnsDraft()
        {
            // Arrange
            var examination = new Examination();

            var caseBreakdown = new CaseBreakDown();
            caseBreakdown.OtherEvents = new OtherEventContainer();
            var myUser = new MeUser()
            {
                UserId = "userOne"
            };

            var newDraft = new OtherEvent()
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userOne",
                EventId = "a"
            };

            examination.CaseBreakdown = caseBreakdown;

            // Act
            examination.AddEvent(newDraft);

            // Assert
            Assert.Single(examination.CaseBreakdown.OtherEvents.Drafts);
            Assert.Equal(newDraft, examination.CaseBreakdown.OtherEvents.Drafts.First());
        }

        [Fact]
        public void CaseCompleted_UrgencyScoreIsZero()
        {
            // Arrange
            var examination = new Examination();
            examination.CaseCompleted = true;

            // Act
            examination.UpdateCaseUrgencyScore();

            // Assert
            Assert.Equal(0, examination.UrgencyScore);
        }


        [Fact]
        public void UpdateDraftEventForUserReturnsDraft()
        {
            // Arrange
            var examination = new Examination();

            var caseBreakdown = new CaseBreakDown();
            caseBreakdown.OtherEvents = new OtherEventContainer();
            var myUser = new MeUser()
            {
                UserId = "userOne"
            };

            var newDraft = new OtherEvent()
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userOne"
            };

            examination.CaseBreakdown = caseBreakdown;

            // Act
            examination.AddEvent(newDraft);

            var updateDraft = new OtherEvent()
            {
                EventId = newDraft.EventId,
                IsFinal = false,
                Text = "updated event",
                UserId = "userOne"
            };
            examination.AddEvent(updateDraft);

            // Assert
            Assert.Single(examination.CaseBreakdown.OtherEvents.Drafts);
            Assert.Equal(updateDraft, examination.CaseBreakdown.OtherEvents.Drafts.First());
        }

        [Fact]
        public void UpdateDraftEventForUserWithDifferentEventIdThrowsException()
        {
            // Arrange
            var examination = new Examination();

            var caseBreakdown = new CaseBreakDown();
            caseBreakdown.OtherEvents = new OtherEventContainer();
            var myUser = new MeUser()
            {
                UserId = "userOne"
            };

            var newDraft = new OtherEvent()
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userOne"
            };

            examination.CaseBreakdown = caseBreakdown;

            examination.AddEvent(newDraft);

            var updateDraft = new OtherEvent()
            {
                EventId = "bad id",
                IsFinal = false,
                Text = "updated event",
                UserId = "userOne"
            };

            // Act
            Action act = () => examination.AddEvent(updateDraft);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void DraftEventSetToFinalRemovesUsersDrafts()
        {
            // Arrange
            var examination = new Examination();

            var caseBreakdown = new CaseBreakDown();
            caseBreakdown.OtherEvents = new OtherEventContainer();
            var userOne = new MeUser()
            {
                UserId = "userOne"
            };

            var userTwo = new MeUser()
            {
                UserId = "userTwo"
            };

            var newDraft = new OtherEvent()
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userOne"
            };

            var newDraftTwo = new OtherEvent()
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userTwo"
            };

            examination.CaseBreakdown = caseBreakdown;

            // Act
            examination.AddEvent(newDraft);
            examination.AddEvent(newDraftTwo);

            var updateDraft = new OtherEvent()
            {
                EventId = newDraft.EventId,
                IsFinal = true,
                Text = "updated event",
                UserId = "userOne"
            };
            examination.AddEvent(updateDraft);

            // Assert
            Assert.Single(examination.CaseBreakdown.OtherEvents.Drafts);
            Assert.Equal(newDraftTwo, examination.CaseBreakdown.OtherEvents.Drafts.First());
            Assert.Equal(updateDraft, examination.CaseBreakdown.OtherEvents.Latest);
        }

        [Fact]
        public void When_Null_Examination_Is_Passed_Throws_Argument_Null_Exception()
        {
            Action act = () => ExaminationExtensionMethods.UpdateCaseUrgencyScore(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void No_Urgency_Indicators_Selected_And_Less_Than_Five_Days_Gone_Since_Case_Created_Then_The_Urgency_Score_Is_Zero()
        {
            // Arrange
            var examination = new Examination
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
                CreatedAt = DateTime.Now.AddDays(-3)
            };

            // Act
            var result = examination.UpdateCaseUrgencyScore();

            // Assert
            Assert.Equal(0, result.UrgencyScore);
        }

        [Fact]
        public void All_Urgency_Indicators_Selected_And_Less_Than_Five_Days_Gone_Since_Case_Created_Then_The_Urgency_Score_Is_500()
        {
            // Arrange
            var examination = new Examination
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CreatedAt = DateTime.Now.AddDays(-3)
            };

            // Act
            var result = examination.UpdateCaseUrgencyScore();

            // Assert
            Assert.Equal(500, result.UrgencyScore);
        }

        [Fact]
        public void No_Urgency_Indicators_Selected_And_Greater_Than_Five_Days_Gone_Since_Case_Created_Then_The_Urgency_Score_Is_1000()
        {
            // Arrange
            var examination = new Examination
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
                CreatedAt = DateTime.Now.AddDays(-6)
            };

            // Act
            var result = examination.UpdateCaseUrgencyScore();

            // Assert
            Assert.Equal(1000, result.UrgencyScore);
        }

        [Fact]
        public void No_AdmissionNotes_Case_Status_PendingAdmissionNotes_True()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.True(examination.PendingAdmissionNotes);
        }

        [Fact]
        public void Draft_AdmissionNotes_Case_Status_PendingAdmissionNotes_True()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            var admissionNotes = new AdmissionNotesEventContainer();
            admissionNotes.Drafts.Add(new AdmissionEvent());
            caseBreakDown.AdmissionNotes = admissionNotes;
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.True(examination.PendingAdmissionNotes);
        }

        [Fact]
        public void Latest_AdmissionNotes_Case_Status_PendingAdmissionNotes_False()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            var admissionNotes = new AdmissionNotesEventContainer();
            admissionNotes.Latest = new AdmissionEvent()
            {
                ImmediateCoronerReferral = false
            };
            caseBreakDown.AdmissionNotes = admissionNotes;
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.True(examination.PendingAdmissionNotes);
        }

        [Fact]
        public void No_AdmissionNotes_Case_Status_Admission_Notes_Have_Been_Added_False()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.False(examination.AdmissionNotesHaveBeenAdded);
        }

        [Fact]
        public void Draft_AdmissionNotes_Case_Status_Admission_Notes_Have_Been_Added_False()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            var admissionNotes = new AdmissionNotesEventContainer();
            admissionNotes.Drafts.Add(new AdmissionEvent());
            caseBreakDown.AdmissionNotes = admissionNotes;
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.False(examination.AdmissionNotesHaveBeenAdded);
        }

        [Fact]
        public void Latest_AdmissionNotes_No_Consultant_Case_Status_Admission_Notes_Have_Been_Added_False()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            var admissionNotes = new AdmissionNotesEventContainer();
            admissionNotes.Latest = new AdmissionEvent()
            {
                ImmediateCoronerReferral = false
            };
            caseBreakDown.AdmissionNotes = admissionNotes;
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.False(examination.AdmissionNotesHaveBeenAdded);
        }

        [Fact]
        public void Latest_AdmissionNotes_And_Consultant_Case_Status_Admission_Notes_Have_Been_Added_True()
        {
            // Arrange
            var examination = new Examination();
            var medicalTeam = new MedicalTeam();
            medicalTeam.ConsultantResponsible = new ClinicalProfessional();
            examination.MedicalTeam = medicalTeam;
            var caseBreakDown = new CaseBreakDown();
            var admissionNotes = new AdmissionNotesEventContainer();
            admissionNotes.Latest = new AdmissionEvent()
            {
                ImmediateCoronerReferral = false,
            };
            caseBreakDown.AdmissionNotes = admissionNotes;
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.True(examination.AdmissionNotesHaveBeenAdded);
        }

        [Fact]
        public void No_Me_Scrutiny_Case_Status_HaveBeenScrutinisedByME_False()
        {
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            examination.CaseBreakdown = caseBreakDown;

            examination = examination.UpdateCaseStatus();

            Assert.False(examination.HaveBeenScrutinisedByME);
        }

        [Fact]
        public void No_Me_Or_Meo_Unassigned_True()
        {
            var examination = new Examination();
            examination = examination.UpdateCaseStatus();

            Assert.True(examination.Unassigned);
        }

        [Fact]
        public void No_Me_Assigned_Meo_Unassigned_True()
        {
            var examination = new Examination();
            examination.MedicalTeam.MedicalExaminerOfficerUserId = "a";
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.Unassigned);
        }

        [Fact]
        public void Assigned_Me_Assigned_Meo_Unassigned_False()
        {
            var examination = new Examination();
            examination.MedicalTeam.MedicalExaminerOfficerUserId = "a";
            examination.MedicalTeam.MedicalExaminerUserId = "a";
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.Unassigned);
        }

        [Fact]
        public void Assigned_Me_No_Meo_Unassigned_True()
        {
            var examination = new Examination();
            examination.MedicalTeam.MedicalExaminerUserId = "a";
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.Unassigned);
        }

        [Fact]
        public void All_Urgency_Indicators_Selected_And_Greater_Than_Five_Days_Gone_Since_Case_Created_Then_The_Urgency_Score_Is_1500()
        {
            // Arrange
            var examination = new Examination
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
                CreatedAt = DateTime.Now.AddDays(-6)
            };

            // Act
            var result = examination.UpdateCaseUrgencyScore();

            // Assert
            Assert.Equal(1500, result.UrgencyScore);
        }

        [Fact]
        public void NewExamination_PendingQapDiscussion_True()
        {
            var examination = new Examination();
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void ReadyForMeScrutiny_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination.CaseBreakdown.MeoSummary.Latest = new MeoSummaryEvent();
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_DraftAdmissionNotes_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Drafts.Add(new AdmissionEvent());
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_FinalAdmissionNotes_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination.ReadyForMEScrutiny = false;
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() {
                ImmediateCoronerReferral = false
            };
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_FinalAdmissionNotes_ImmediateCoronerTrue_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination.ReadyForMEScrutiny = false;
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() { ImmediateCoronerReferral = true };
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_FinalAdmissionNotes_ImmediateCoronerFalse_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() { ImmediateCoronerReferral = false };
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_FinalAdmissionNotes_ImmediateCoronerFalse_QAPDiscussionOccured_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() { ImmediateCoronerReferral = false };
            examination.CaseBreakdown.QapDiscussion.Latest = new QapDiscussionEvent() { DiscussionUnableHappen = false };
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_FinalAdmissionNotes_ImmediateCoronerFalse_QAPDiscussionNotOccured_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() { ImmediateCoronerReferral = false };
            examination.CaseBreakdown.QapDiscussion.Latest = new QapDiscussionEvent() { DiscussionUnableHappen = true };
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NewExamination_PendingRepresentativeDiscussion_True()
        {
            var examination = new Examination();
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_ReadyForMeScrutiny_False()
        {
            var examination = new Examination();
            examination.CaseBreakdown.MeoSummary.Latest = new MeoSummaryEvent();
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_NoLatestAdmissionNotes_True()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = null;
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_LatestAdmissionNotes_True()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent()
            {
                ImmediateCoronerReferral = false
            };
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_LatestAdmissionNotes_ImmediateCoronerReferral_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() { ImmediateCoronerReferral = true };
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_LatestAdmissionNotes_NoImmediateCoronerReferral_True()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() { ImmediateCoronerReferral = false };
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_LatestAdmissionNotes_NoImmediateCoronerReferral_LatestBereavedDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() { ImmediateCoronerReferral = false };
            examination.CaseBreakdown.BereavedDiscussion.Latest = new BereavedDiscussionEvent();
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_LatestAdmissionNotes_NoImmediateCoronerReferral_LatestBereavedDiscussionUnableToHappen_True()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent() { ImmediateCoronerReferral = false };
            examination.CaseBreakdown.BereavedDiscussion.Latest = new BereavedDiscussionEvent() { DiscussionUnableHappen = true };
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void NewExamination_PendingScrutinyNotes_True()
        {
            var examination = new Examination();
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingScrutinyNotes);
        }

        [Fact]
        public void PendingScrutinyNotes_MEScrutiny_False()
        {
            var examination = new Examination();
            examination.CaseBreakdown.PreScrutiny.Latest = new PreScrutinyEvent();
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingScrutinyNotes);
        }

        [Fact]
        public void NewExamination_ScrutinyComplete_False()
        {
            var examination = new Examination();
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.ScrutinyConfirmed);
        }

        [Fact]
        public void ScrutinyComplete_Unassigned_False()
        {
            var examination = new Examination();
            examination = SetUnassigned(examination);
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.ScrutinyConfirmed);
        }

        [Fact]
        public void ScrutinyComplete_ReadyForScrutiny_Unassigned_False()
        {
            var examination = new Examination();
            examination.CaseBreakdown.MeoSummary.Latest = new MeoSummaryEvent();
            examination = SetUnassigned(examination);
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.ScrutinyConfirmed);
        }

        [Fact]
        public void ScrutinyComplete_ReadyForScrutiny_AdmissionNotes_Assigned_False()
        {
            var examination = new Examination();
            examination = SetAssigned(examination);
            examination = SetReadyForMeScrutinyAdmissionNotes(examination);
            examination = SetPreScrutinyDone(examination);
            examination = SetQapDiscussionDone(examination);
            examination.MedicalTeam.ConsultantResponsible = new ClinicalProfessional();
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.ScrutinyConfirmed);
        }

        [Fact]
        public void ScrutinyComplete_ReadyForScrutiny_MeoSummary_Assigned_False()
        {
            var examination = new Examination();
            examination = SetAssigned(examination);
            examination = SetReadyForMeScrutinyMeoSummary(examination);
            examination = SetPreScrutinyDone(examination);
            examination.MedicalTeam.ConsultantResponsible = new ClinicalProfessional();
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.ScrutinyConfirmed);
        }

        private Examination SetAssigned(Examination examination)
        {
            examination.MedicalTeam.MedicalExaminerOfficerUserId = "medicalExaminerOfficer";
            examination.MedicalTeam.MedicalExaminerUserId = "medicalExaminer";

            return examination;
        }

        private Examination SetUnassigned(Examination examination)
        {
            examination.MedicalTeam.MedicalExaminerOfficerUserId = null;
            examination.MedicalTeam.MedicalExaminerUserId = null;

            return examination;
        }

        private Examination SetReadyForMeScrutinyAdmissionNotes(Examination examination)
        {
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent()
            {
                ImmediateCoronerReferral = true
            };

            return examination;
        }

        private Examination SetReadyForMeScrutinyMeoSummary(Examination examination)
        {
            examination.CaseBreakdown.MeoSummary.Latest = new MeoSummaryEvent();
            return examination;
        }

        private Examination SetNotReadyForMeScrutiny(Examination examination)
        {
            examination.CaseBreakdown.AdmissionNotes.Drafts.Add(new AdmissionEvent());
            examination.CaseBreakdown.MeoSummary.Latest = null;
            return examination;
        }

        private Examination SetAdmissionNotesAdded(Examination examination)
        {
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent();
            examination.MedicalTeam.ConsultantResponsible = new ClinicalProfessional();
            return examination;
        }

        private Examination SetPendingQapDiscussion(Examination examination)
        {
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent()
            {
                ImmediateCoronerReferral = false
            };
            examination.CaseBreakdown.QapDiscussion.Latest = new QapDiscussionEvent()
            {
                DiscussionUnableHappen = false
            };

            return examination;
        }

        private Examination SetPreScrutinyDone(Examination examination)
        {
            examination.CaseBreakdown.PreScrutiny.Latest = new PreScrutinyEvent();
            return examination;
        }

        private Examination SetQapDiscussionDone(Examination examination)
        {
            examination.CaseBreakdown.QapDiscussion.Latest = new QapDiscussionEvent();
            return examination;
        }

        private Examination SetRepresentativeDiscussionDone_ReadyForMeScrutiny_AdmissionNotes(Examination examination)
        {
            examination = SetReadyForMeScrutinyAdmissionNotes(examination);
            return examination;
        }

        private Examination SetRepresentativeDiscussionDone_ReadyForMeScrutiny_MeoSummary(Examination examination)
        {
            examination = SetReadyForMeScrutinyMeoSummary(examination);
            return examination;
        }

        private Examination SetRepresentativeDiscussionDone_AdmissionNotes_ImmediateReferral(Examination examination)
        {
            if (examination.CaseBreakdown.AdmissionNotes.Latest != null)
            {
                examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral = true;
            }
            else
            {
                examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent()
                {
                    ImmediateCoronerReferral = true
                };
            }

            return examination;
        }

        private Examination SetRepresentativeDiscussionDone_RepresentativeDiscussion(Examination examination)
        {
            if (examination.CaseBreakdown.BereavedDiscussion.Latest != null)
            {
                examination.CaseBreakdown.BereavedDiscussion.Latest.DiscussionUnableHappen = false;
            }
            else
            {
                examination.CaseBreakdown.BereavedDiscussion.Latest = new BereavedDiscussionEvent()
                {
                    DiscussionUnableHappen = false
                };
            }

            return examination;
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_CompleteRequirements_Returns_True()
        {
            var examination = new Examination()
            {
                MedicalTeam = new MedicalTeam()
                {
                    MedicalExaminerOfficerUserId = "MedicalExaminerOfficerUserId",
                    MedicalExaminerUserId = "MedicalExaminerUserId",
                    ConsultantResponsible = new ClinicalProfessional
                    {
                        Name = "Name",
                        Role = "Role",
                        Organisation = "Organisation",
                        Phone = "011249837843",
                        Notes = "Notes",
                        GMCNumber = "GMCNumber"
                    }
                },
                CaseBreakdown = new CaseBreakDown()
                {
                    PreScrutiny = new PreScrutinyEventContainer()
                    {
                        Latest = new PreScrutinyEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = MedicalExaminer.Models.Enums.OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = MedicalExaminer.Models.Enums.ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = MedicalExaminer.Models.Enums.OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer()
                    {
                        Latest = new AdmissionEvent()
                        {
                            AdmittedDate = DateTime.Now,
                            AdmittedTime = new TimeSpan(12, 12, 12),
                            Created = DateTime.Now,
                            EventId = "2",
                            ImmediateCoronerReferral = false,
                            IsFinal = true,
                            Notes = "Notes",
                            UserId = "userId",
                            UsersRole = "usersRole",
                            UserFullName = "usersFullName"
                        }
                    },
                    MeoSummary = new MeoSummaryEventContainer
                    {
                        Latest = new MeoSummaryEvent
                        {
                            UserFullName = "UserFullName",
                            UsersRole = "UsersRole",
                            EventId = "EventId",
                            UserId = "UserId",
                            IsFinal = true,
                            SummaryDetails = null,
                            Created = DateTime.Now
                        }
                    },
                    QapDiscussion = new QapDiscussionEventContainer()
                    {
                        Latest = new QapDiscussionEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion Details",
                            DiscussionUnableHappen = false,
                            ParticipantName = "ParticipantName",
                            ParticipantOrganisation = "ParticipantOrganisation",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRole = "ParticipantRole",
                            EventId = "3",
                            IsFinal = true,
                            Created = DateTime.Now,
                            QapDiscussionOutcome = MedicalExaminer.Models.Enums.QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer()
                    {
                        Latest = new BereavedDiscussionEvent()
                        {
                            BereavedDiscussionOutcome = MedicalExaminer.Models.Enums.BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = MedicalExaminer.Models.Enums.InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = MedicalExaminer.Models.Enums.PresentAtDeath.No
                        }
                    }
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.True(examination.CalculateCanCompleteScrutiny());
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_MEAndMEOUnassigned_Returns_False()
        {
            var examination = new Examination()
            {
                MedicalTeam = new MedicalTeam()
                {
                    MedicalExaminerOfficerUserId = null,
                    MedicalExaminerUserId = null
                },
                CaseBreakdown = new CaseBreakDown()
                {
                    PreScrutiny = new PreScrutinyEventContainer()
                    {
                        Latest = new PreScrutinyEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = MedicalExaminer.Models.Enums.OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = MedicalExaminer.Models.Enums.ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = MedicalExaminer.Models.Enums.OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer()
                    {
                        Latest = new AdmissionEvent()
                        {
                            AdmittedDate = DateTime.Now,
                            AdmittedTime = new TimeSpan(12, 12, 12),
                            Created = DateTime.Now,
                            EventId = "2",
                            ImmediateCoronerReferral = false,
                            IsFinal = true,
                            Notes = "Notes",
                            UserId = "userId",
                            UsersRole = "usersRole",
                            UserFullName = "usersFullName"
                        }
                    },
                    MeoSummary = new MeoSummaryEventContainer
                    {
                        Latest = new MeoSummaryEvent
                        {
                            UserFullName = "UserFullName",
                            UsersRole = "UsersRole",
                            EventId = "EventId",
                            UserId = "UserId",
                            IsFinal = true,
                            SummaryDetails = null,
                            Created = DateTime.Now
                        }
                    },
                    QapDiscussion = new QapDiscussionEventContainer()
                    {
                        Latest = new QapDiscussionEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion Details",
                            DiscussionUnableHappen = false,
                            ParticipantName = "ParticipantName",
                            ParticipantOrganisation = "ParticipantOrganisation",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRole = "ParticipantRole",
                            EventId = "3",
                            IsFinal = true,
                            Created = DateTime.Now,
                            QapDiscussionOutcome = MedicalExaminer.Models.Enums.QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer()
                    {
                        Latest = new BereavedDiscussionEvent()
                        {
                            BereavedDiscussionOutcome = MedicalExaminer.Models.Enums.BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = MedicalExaminer.Models.Enums.InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = MedicalExaminer.Models.Enums.PresentAtDeath.No
                        }
                    }
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.False(examination.CalculateCanCompleteScrutiny());
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_HasQAPDiscussion_NoBereavedDiscussion_Returns_True()
        {
            var examination = new Examination()
            {
                MedicalTeam = new MedicalTeam()
                {
                    MedicalExaminerOfficerUserId = "MedicalExaminerOfficerUserId",
                    MedicalExaminerUserId = "MedicalExaminerUserId",
                    ConsultantResponsible = new ClinicalProfessional
                    {
                        Name = "Name",
                        Role = "Role",
                        Organisation = "Organisation",
                        Phone = "011249837843",
                        Notes = "Notes",
                        GMCNumber = "GMCNumber"
                    }
                },
                CaseBreakdown = new CaseBreakDown()
                {
                    PreScrutiny = new PreScrutinyEventContainer()
                    {
                        Latest = new PreScrutinyEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = MedicalExaminer.Models.Enums.OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = MedicalExaminer.Models.Enums.ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = MedicalExaminer.Models.Enums.OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer()
                    {
                        Latest = new AdmissionEvent()
                        {
                            AdmittedDate = DateTime.Now,
                            AdmittedTime = new TimeSpan(12, 12, 12),
                            Created = DateTime.Now,
                            EventId = "2",
                            ImmediateCoronerReferral = false,
                            IsFinal = true,
                            Notes = "Notes",
                            UserId = "userId",
                            UsersRole = "usersRole",
                            UserFullName = "usersFullName"
                        }
                    },
                    MeoSummary = new MeoSummaryEventContainer
                    {
                        Latest = new MeoSummaryEvent
                        {
                            UserFullName = "UserFullName",
                            UsersRole = "UsersRole",
                            EventId = "EventId",
                            UserId = "UserId",
                            IsFinal = true,
                            SummaryDetails = null,
                            Created = DateTime.Now
                        }
                    },
                    QapDiscussion = new QapDiscussionEventContainer()
                    {
                        Latest = new QapDiscussionEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion Details",
                            DiscussionUnableHappen = false,
                            ParticipantName = "ParticipantName",
                            ParticipantOrganisation = "ParticipantOrganisation",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRole = "ParticipantRole",
                            EventId = "3",
                            IsFinal = true,
                            Created = DateTime.Now,
                            QapDiscussionOutcome = MedicalExaminer.Models.Enums.QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.True(examination.CalculateCanCompleteScrutiny());
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_NoQapDiscussion_HasBereavedDiscussion_Returns_True()
        {
            var examination = new Examination()
            {
                MedicalTeam = new MedicalTeam()
                {
                    MedicalExaminerOfficerUserId = "MedicalExaminerOfficerUserId",
                    MedicalExaminerUserId = "MedicalExaminerUserId",
                    ConsultantResponsible = new ClinicalProfessional
                    {
                        Name = "Name",
                        Role = "Role",
                        Organisation = "Organisation",
                        Phone = "011249837843",
                        Notes = "Notes",
                        GMCNumber = "GMCNumber"
                    }
                },
                CaseBreakdown = new CaseBreakDown()
                {
                    PreScrutiny = new PreScrutinyEventContainer()
                    {
                        Latest = new PreScrutinyEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = MedicalExaminer.Models.Enums.OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = MedicalExaminer.Models.Enums.ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = MedicalExaminer.Models.Enums.OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer()
                    {
                        Latest = new AdmissionEvent()
                        {
                            AdmittedDate = DateTime.Now,
                            AdmittedTime = new TimeSpan(12, 12, 12),
                            Created = DateTime.Now,
                            EventId = "2",
                            ImmediateCoronerReferral = false,
                            IsFinal = true,
                            Notes = "Notes",
                            UserId = "userId",
                            UsersRole = "usersRole",
                            UserFullName = "usersFullName"
                        }
                    },
                    MeoSummary = new MeoSummaryEventContainer
                    {
                        Latest = new MeoSummaryEvent
                        {
                            UserFullName = "UserFullName",
                            UsersRole = "UsersRole",
                            EventId = "EventId",
                            UserId = "UserId",
                            IsFinal = true,
                            SummaryDetails = null,
                            Created = DateTime.Now
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer()
                    {
                        Latest = new BereavedDiscussionEvent()
                        {
                            BereavedDiscussionOutcome = MedicalExaminer.Models.Enums.BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = MedicalExaminer.Models.Enums.InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = MedicalExaminer.Models.Enums.PresentAtDeath.No
                        }
                    }
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.True(examination.CalculateCanCompleteScrutiny());
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_NoAdmissionNotes_Returns_False()
        {
            var examination = new Examination()
            {
                MedicalTeam = new MedicalTeam()
                {
                    MedicalExaminerOfficerUserId = "MedicalExaminerOfficerUserId",
                    MedicalExaminerUserId = "MedicalExaminerUserId",
                    ConsultantResponsible = new ClinicalProfessional
                    {
                        Name = "Name",
                        Role = "Role",
                        Organisation = "Organisation",
                        Phone = "011249837843",
                        Notes = "Notes",
                        GMCNumber = "GMCNumber"
                    }
                },
                CaseBreakdown = new CaseBreakDown()
                {
                    PreScrutiny = new PreScrutinyEventContainer()
                    {
                        Latest = new PreScrutinyEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = MedicalExaminer.Models.Enums.OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = MedicalExaminer.Models.Enums.ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = MedicalExaminer.Models.Enums.OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    MeoSummary = new MeoSummaryEventContainer
                    {
                        Latest = new MeoSummaryEvent
                        {
                            UserFullName = "UserFullName",
                            UsersRole = "UsersRole",
                            EventId = "EventId",
                            UserId = "UserId",
                            IsFinal = true,
                            SummaryDetails = null,
                            Created = DateTime.Now
                        }
                    },
                    QapDiscussion = new QapDiscussionEventContainer()
                    {
                        Latest = new QapDiscussionEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion Details",
                            DiscussionUnableHappen = false,
                            ParticipantName = "ParticipantName",
                            ParticipantOrganisation = "ParticipantOrganisation",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRole = "ParticipantRole",
                            EventId = "3",
                            IsFinal = true,
                            Created = DateTime.Now,
                            QapDiscussionOutcome = MedicalExaminer.Models.Enums.QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer()
                    {
                        Latest = new BereavedDiscussionEvent()
                        {
                            BereavedDiscussionOutcome = MedicalExaminer.Models.Enums.BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = MedicalExaminer.Models.Enums.InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = MedicalExaminer.Models.Enums.PresentAtDeath.No
                        }
                    }
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.False(examination.CalculateCanCompleteScrutiny());
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_NoPreScrutiny_Returns_False()
        {
            var examination = new Examination()
            {
                MedicalTeam = new MedicalTeam()
                {
                    MedicalExaminerOfficerUserId = "MedicalExaminerOfficerUserId",
                    MedicalExaminerUserId = "MedicalExaminerUserId",
                    ConsultantResponsible = new ClinicalProfessional
                    {
                        Name = "Name",
                        Role = "Role",
                        Organisation = "Organisation",
                        Phone = "011249837843",
                        Notes = "Notes",
                        GMCNumber = "GMCNumber"
                    }
                },
                CaseBreakdown = new CaseBreakDown()
                {
                    AdmissionNotes = new AdmissionNotesEventContainer()
                    {
                        Latest = new AdmissionEvent()
                        {
                            AdmittedDate = DateTime.Now,
                            AdmittedTime = new TimeSpan(12, 12, 12),
                            Created = DateTime.Now,
                            EventId = "2",
                            ImmediateCoronerReferral = false,
                            IsFinal = true,
                            Notes = "Notes",
                            UserId = "userId",
                            UsersRole = "usersRole",
                            UserFullName = "usersFullName"
                        }
                    },
                    MeoSummary = new MeoSummaryEventContainer
                    {
                        Latest = new MeoSummaryEvent
                        {
                            UserFullName = "UserFullName",
                            UsersRole = "UsersRole",
                            EventId = "EventId",
                            UserId = "UserId",
                            IsFinal = true,
                            SummaryDetails = null,
                            Created = DateTime.Now
                        }
                    },
                    QapDiscussion = new QapDiscussionEventContainer()
                    {
                        Latest = new QapDiscussionEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion Details",
                            DiscussionUnableHappen = false,
                            ParticipantName = "ParticipantName",
                            ParticipantOrganisation = "ParticipantOrganisation",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRole = "ParticipantRole",
                            EventId = "3",
                            IsFinal = true,
                            Created = DateTime.Now,
                            QapDiscussionOutcome = MedicalExaminer.Models.Enums.QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer()
                    {
                        Latest = new BereavedDiscussionEvent()
                        {
                            BereavedDiscussionOutcome = MedicalExaminer.Models.Enums.BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = MedicalExaminer.Models.Enums.InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = MedicalExaminer.Models.Enums.PresentAtDeath.No
                        }
                    }
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.False(examination.CalculateCanCompleteScrutiny());
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_NoMEOSummary_Returns_False()
        {
            var examination = new Examination()
            {
                MedicalTeam = new MedicalTeam()
                {
                    MedicalExaminerOfficerUserId = "MedicalExaminerOfficerUserId",
                    MedicalExaminerUserId = "MedicalExaminerUserId",
                },
                CaseBreakdown = new CaseBreakDown()
                {
                    PreScrutiny = new PreScrutinyEventContainer()
                    {
                        Latest = new PreScrutinyEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = MedicalExaminer.Models.Enums.OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = MedicalExaminer.Models.Enums.ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = MedicalExaminer.Models.Enums.OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer()
                    {
                        Latest = new AdmissionEvent()
                        {
                            AdmittedDate = DateTime.Now,
                            AdmittedTime = new TimeSpan(12, 12, 12),
                            Created = DateTime.Now,
                            EventId = "2",
                            ImmediateCoronerReferral = false,
                            IsFinal = true,
                            Notes = "Notes",
                            UserId = "userId",
                            UsersRole = "usersRole",
                            UserFullName = "usersFullName"
                        }
                    },
                    QapDiscussion = new QapDiscussionEventContainer()
                    {
                        Latest = new QapDiscussionEvent()
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion Details",
                            DiscussionUnableHappen = false,
                            ParticipantName = "ParticipantName",
                            ParticipantOrganisation = "ParticipantOrganisation",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRole = "ParticipantRole",
                            EventId = "3",
                            IsFinal = true,
                            Created = DateTime.Now,
                            QapDiscussionOutcome = MedicalExaminer.Models.Enums.QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer()
                    {
                        Latest = new BereavedDiscussionEvent()
                        {
                            BereavedDiscussionOutcome = MedicalExaminer.Models.Enums.BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = MedicalExaminer.Models.Enums.InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = MedicalExaminer.Models.Enums.PresentAtDeath.No
                        }
                    }
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.False(examination.CalculateCanCompleteScrutiny());
        }
    }
}
