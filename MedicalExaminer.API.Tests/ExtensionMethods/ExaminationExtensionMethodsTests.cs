using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentAssertions;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
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
            var myUser = new MeUser
            {
                UserId = "userOne"
            };

            var newDraft = new OtherEvent
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
            var myUser = new MeUser
            {
                UserId = "userOne"
            };

            var newDraft = new OtherEvent
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userOne"
            };

            examination.CaseBreakdown = caseBreakdown;

            // Act
            examination.AddEvent(newDraft);

            var updateDraft = new OtherEvent
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
            var myUser = new MeUser
            {
                UserId = "userOne"
            };

            var newDraft = new OtherEvent
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userOne"
            };

            examination.CaseBreakdown = caseBreakdown;

            examination.AddEvent(newDraft);

            var updateDraft = new OtherEvent
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
            var userOne = new MeUser
            {
                UserId = "userOne"
            };

            var userTwo = new MeUser
            {
                UserId = "userTwo"
            };

            var newDraft = new OtherEvent
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userOne"
            };

            var newDraftTwo = new OtherEvent
            {
                IsFinal = false,
                Text = "other event one",
                UserId = "userTwo"
            };

            examination.CaseBreakdown = caseBreakdown;

            // Act
            examination.AddEvent(newDraft);
            examination.AddEvent(newDraftTwo);

            var updateDraft = new OtherEvent
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
            admissionNotes.Latest = new AdmissionEvent
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
            admissionNotes.Latest = new AdmissionEvent
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
            admissionNotes.Latest = new AdmissionEvent
            {
                ImmediateCoronerReferral = false
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
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent
            {
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
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent { ImmediateCoronerReferral = true };
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_FinalAdmissionNotes_ImmediateCoronerFalse_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent { ImmediateCoronerReferral = false };
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_FinalAdmissionNotes_ImmediateCoronerFalse_QAPDiscussionOccured_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent { ImmediateCoronerReferral = false };
            examination.CaseBreakdown.QapDiscussion.Latest = new QapDiscussionEvent { DiscussionUnableHappen = false };
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithQAP);
        }

        [Fact]
        public void NotReadyForMeScrutiny_FinalAdmissionNotes_ImmediateCoronerFalse_QAPDiscussionNotOccured_PendingQapDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent { ImmediateCoronerReferral = false };
            examination.CaseBreakdown.QapDiscussion.Latest = new QapDiscussionEvent { DiscussionUnableHappen = true };
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
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent
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
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent { ImmediateCoronerReferral = true };
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_LatestAdmissionNotes_NoImmediateCoronerReferral_True()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent { ImmediateCoronerReferral = false };
            examination = examination.UpdateCaseStatus();
            Assert.True(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_LatestAdmissionNotes_NoImmediateCoronerReferral_LatestBereavedDiscussion_False()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent { ImmediateCoronerReferral = false };
            examination.CaseBreakdown.BereavedDiscussion.Latest = new BereavedDiscussionEvent();
            examination = examination.UpdateCaseStatus();
            Assert.False(examination.PendingDiscussionWithRepresentative);
        }

        [Fact]
        public void PendingRepresentativeDiscussion_NotReadyForMeScrutiny_LatestAdmissionNotes_NoImmediateCoronerReferral_LatestBereavedDiscussionUnableToHappen_True()
        {
            var examination = new Examination();
            examination = SetNotReadyForMeScrutiny(examination);
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent { ImmediateCoronerReferral = false };
            examination.CaseBreakdown.BereavedDiscussion.Latest = new BereavedDiscussionEvent { DiscussionUnableHappen = true };
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
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent
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
            examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent
            {
                ImmediateCoronerReferral = false
            };
            examination.CaseBreakdown.QapDiscussion.Latest = new QapDiscussionEvent
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
                examination.CaseBreakdown.AdmissionNotes.Latest = new AdmissionEvent
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
                examination.CaseBreakdown.BereavedDiscussion.Latest = new BereavedDiscussionEvent
                {
                    DiscussionUnableHappen = false
                };
            }

            return examination;
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_CompleteRequirements_Returns_True()
        {
            var examination = new Examination
            {
                MedicalTeam = new MedicalTeam
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
                CaseBreakdown = new CaseBreakDown
                {
                    PreScrutiny = new PreScrutinyEventContainer
                    {
                        Latest = new PreScrutinyEvent
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer
                    {
                        Latest = new AdmissionEvent
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
                    QapDiscussion = new QapDiscussionEventContainer
                    {
                        Latest = new QapDiscussionEvent
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
                            QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer
                    {
                        Latest = new BereavedDiscussionEvent
                        {
                            BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = PresentAtDeath.No
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
            var examination = new Examination
            {
                MedicalTeam = new MedicalTeam
                {
                    MedicalExaminerOfficerUserId = null,
                    MedicalExaminerUserId = null
                },
                CaseBreakdown = new CaseBreakDown
                {
                    PreScrutiny = new PreScrutinyEventContainer
                    {
                        Latest = new PreScrutinyEvent
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer
                    {
                        Latest = new AdmissionEvent
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
                    QapDiscussion = new QapDiscussionEventContainer
                    {
                        Latest = new QapDiscussionEvent
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
                            QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer
                    {
                        Latest = new BereavedDiscussionEvent
                        {
                            BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = PresentAtDeath.No
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
            var examination = new Examination
            {
                MedicalTeam = new MedicalTeam
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
                CaseBreakdown = new CaseBreakDown
                {
                    PreScrutiny = new PreScrutinyEventContainer
                    {
                        Latest = new PreScrutinyEvent
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer
                    {
                        Latest = new AdmissionEvent
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
                    QapDiscussion = new QapDiscussionEventContainer
                    {
                        Latest = new QapDiscussionEvent
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
                            QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    }
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.True(examination.CalculateCanCompleteScrutiny());
        }

        [Fact]
        private void CalculateScrutinyCanBeConfirmed_NoQapDiscussion_HasBereavedDiscussion_Returns_True()
        {
            var examination = new Examination
            {
                MedicalTeam = new MedicalTeam
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
                CaseBreakdown = new CaseBreakDown
                {
                    PreScrutiny = new PreScrutinyEventContainer
                    {
                        Latest = new PreScrutinyEvent
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer
                    {
                        Latest = new AdmissionEvent
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
                    BereavedDiscussion = new BereavedDiscussionEventContainer
                    {
                        Latest = new BereavedDiscussionEvent
                        {
                            BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = PresentAtDeath.No
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
            var examination = new Examination
            {
                MedicalTeam = new MedicalTeam
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
                CaseBreakdown = new CaseBreakDown
                {
                    PreScrutiny = new PreScrutinyEventContainer
                    {
                        Latest = new PreScrutinyEvent
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
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
                    QapDiscussion = new QapDiscussionEventContainer
                    {
                        Latest = new QapDiscussionEvent
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
                            QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer
                    {
                        Latest = new BereavedDiscussionEvent
                        {
                            BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = PresentAtDeath.No
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
            var examination = new Examination
            {
                MedicalTeam = new MedicalTeam
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
                CaseBreakdown = new CaseBreakDown
                {
                    AdmissionNotes = new AdmissionNotesEventContainer
                    {
                        Latest = new AdmissionEvent
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
                    QapDiscussion = new QapDiscussionEventContainer
                    {
                        Latest = new QapDiscussionEvent
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
                            QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer
                    {
                        Latest = new BereavedDiscussionEvent
                        {
                            BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = PresentAtDeath.No
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
            var examination = new Examination
            {
                MedicalTeam = new MedicalTeam
                {
                    MedicalExaminerOfficerUserId = "MedicalExaminerOfficerUserId",
                    MedicalExaminerUserId = "MedicalExaminerUserId"
                },
                CaseBreakdown = new CaseBreakDown
                {
                    PreScrutiny = new PreScrutinyEventContainer
                    {
                        Latest = new PreScrutinyEvent
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole"
                        }
                    },
                    AdmissionNotes = new AdmissionNotesEventContainer
                    {
                        Latest = new AdmissionEvent
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
                    QapDiscussion = new QapDiscussionEventContainer
                    {
                        Latest = new QapDiscussionEvent
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
                            QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role"
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer
                    {
                        Latest = new BereavedDiscussionEvent
                        {
                            BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = PresentAtDeath.No
                        }
                    }
                }
            };

            examination = examination.UpdateCaseStatus();

            Assert.False(examination.CalculateCanCompleteScrutiny());
        }

        [Fact]
        private void CalculateOutstandingCaseOutcomesCompleted_With_ReferToCoroner_Returns_True()
        {
            var caseOutcome = new CaseOutcome
            {
                CaseOutcomeSummary = CaseOutcomeSummary.ReferToCoroner
            };

            var examination = new Examination
            {
                CaseOutcome = caseOutcome
            };

            Assert.True(examination.CalculateOutstandingCaseOutcomesCompleted());
        }

        [Fact]
        private void CalculateOutstandingCaseOutcomesCompleted_With_IssueMCCD_Returns_False()
        {
            var caseOutcome = new CaseOutcome
            {
                CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD
            };

            var examination = new Examination
            {
                CaseOutcome = caseOutcome
            };

            Assert.False(examination.CalculateOutstandingCaseOutcomesCompleted());
        }

        [Fact]
        private void CalculateOutstandingCaseOutcomesCompleted_With_No_CremationFormStatus_Returns_True()
        {
            var caseOutcome = new CaseOutcome
            {
                CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD,
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.No
            };

            var examination = new Examination
            {
                CaseOutcome = caseOutcome
            };

            Assert.True(examination.CalculateOutstandingCaseOutcomesCompleted());
        }

        [Fact]
        private void CalculateOutstandingCaseOutcomesCompleted_With_Unknown_CremationFormStatus_Returns_True()
        {
            var caseOutcome = new CaseOutcome
            {
                CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD,
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Unknown
            };

            var examination = new Examination
            {
                CaseOutcome = caseOutcome
            };

            Assert.True(examination.CalculateOutstandingCaseOutcomesCompleted());
        }

        [Fact]
        private void CalculateOutstandingCaseOutcomesCompleted_With_CremationFormStatus_But_GP_Not_Notified_Returns_False()
        {
            var caseOutcome = new CaseOutcome
            {
                CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD,
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.GPUnabledToBeNotified
            };

            var examination = new Examination
            {
                CaseOutcome = caseOutcome
            };

            Assert.False(examination.CalculateOutstandingCaseOutcomesCompleted());
        }

        [Fact]
        private void CalculateOutstandingCaseOutcomesCompleted_With_CremationFormStatus_But_GP_Notified_NA_Returns_True()
        {
            var caseOutcome = new CaseOutcome
            {
                CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD,
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.NA
            };

            var examination = new Examination
            {
                CaseOutcome = caseOutcome
            };

            Assert.True(examination.CalculateOutstandingCaseOutcomesCompleted());
        }

        [Theory]
        [InlineData("Scenario1", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario2", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario3", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario4", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario5", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario6", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario7", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario8", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario9", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario10", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario11", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario12", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario13", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario14", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario15", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario16", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario17", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario18", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario19", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario20", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerFor100a, true, null, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario21", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario22", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario23", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario24", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario25", OverallOutcomeOfPreScrutiny.IssueAnMccd, false, QapDiscussionOutcome.ReferToCoronerInvestigation, true, null, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario26", OverallOutcomeOfPreScrutiny.IssueAnMccd, true, null, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario27", OverallOutcomeOfPreScrutiny.IssueAnMccd, true, null, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario28", OverallOutcomeOfPreScrutiny.IssueAnMccd, true, null, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario29", OverallOutcomeOfPreScrutiny.IssueAnMccd, true, null, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario30", OverallOutcomeOfPreScrutiny.IssueAnMccd, true, null, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario31", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario32", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario33", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario34", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario35", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario36", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario37", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario38", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario39", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario40", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario41", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario42", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario43", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario44", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario45", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario46", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario47", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario48", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario49", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario50", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerFor100a, true, null, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario51", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario52", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario53", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario54", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario55", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, false, QapDiscussionOutcome.ReferToCoronerInvestigation, true, null, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario56", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, true, null, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario57", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, true, null, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario58", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, true, null, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario59", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, true, null, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario60", OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a, true, null, true, null, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario61", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario62", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario63", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario64", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario65", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario66", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario67", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario68", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario69", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario70", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathProvidedByME, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario71", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario72", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario73", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario74", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario75", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME, true, null, CaseOutcomeSummary.IssueMCCD)]
        [InlineData("Scenario76", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario77", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario78", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario79", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerFor100a, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario80", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerFor100a, true, null, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario81", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario82", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario83", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario84", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerInvestigation, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario85", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, false, QapDiscussionOutcome.ReferToCoronerInvestigation, true, null, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario86", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, true, null, false, BereavedDiscussionOutcome.CauseOfDeathAccepted, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario87", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, true, null, false, BereavedDiscussionOutcome.ConcernsCoronerInvestigation, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario88", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, true, null, false, BereavedDiscussionOutcome.ConcernsRequires100a, CaseOutcomeSummary.IssueMCCDWith100a)]
        [InlineData("Scenario89", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, true, null, false, BereavedDiscussionOutcome.ConcernsAddressedWithoutCoroner, CaseOutcomeSummary.ReferToCoroner)]
        [InlineData("Scenario90", OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation, true, null, true, null, CaseOutcomeSummary.ReferToCoroner)]
        public void TestCaseOutcome(
            string scenario,
            OverallOutcomeOfPreScrutiny? overallOutcomeOfPreScrutiny,
            bool qapDiscussionUnableToHappen,
            QapDiscussionOutcome? qapDiscussionOutcome,
            bool bereavedDiscussionUnableToHappen,
            BereavedDiscussionOutcome? bereavedDiscussionOutcome,
            CaseOutcomeSummary caseOutcomeSummary)
        {
            var examination = new Examination();

            examination.ReadyForMEScrutiny = true;
            examination.Unassigned = false;
            examination.PendingScrutinyNotes = false;
            examination.PendingAdmissionNotes = false;
            examination.PendingDiscussionWithQAP = false;

            examination.CaseBreakdown.PreScrutiny.Add(new PreScrutinyEvent
            {
                IsFinal = true,
                OutcomeOfPreScrutiny = overallOutcomeOfPreScrutiny
            });

            examination.CaseBreakdown.QapDiscussion.Add(new QapDiscussionEvent
            {
                IsFinal = true,
                QapDiscussionOutcome = qapDiscussionOutcome,
                DiscussionUnableHappen = qapDiscussionUnableToHappen
            });

            examination.CaseBreakdown.BereavedDiscussion.Add(new BereavedDiscussionEvent
            {
                IsFinal = true,
                BereavedDiscussionOutcome = bereavedDiscussionOutcome,
                DiscussionUnableHappen = bereavedDiscussionUnableToHappen
            });

            var actualCaseOutcomeSummary = examination.CalculateScrutinyOutcome();

            actualCaseOutcomeSummary.Should().Be(caseOutcomeSummary);
        }

        [Fact]
        public void CalculateBasicDetailsEnteredStatus_When_All_The_Details_Are_Entered_Returns_True()
        {
            // Arrange
            var examination = new Examination
            {
                GivenNames = "GivenNames",
                Surname = "Surname",
                DateOfBirth = DateTime.Today,
                DateOfDeath = DateTime.Today,
                NhsNumber = "1234567890",
            };

            // Act
            var haveUnknownBasicDetails = examination.CalculateBasicDetailsEnteredStatus();

            // Assert
            haveUnknownBasicDetails.Should().BeTrue();
        }

        [Fact]
        public void CalculateBasicDetailsEnteredStatus_When_No_Basic_Details_Returns_False()
        {
            // Arrange
            var examination = new Examination
            {
                GivenNames = null,
                Surname = null,
                DateOfBirth = null,
                DateOfDeath = null,
                NhsNumber = null,
            };

            // Act
            var haveUnknownBasicDetails = examination.CalculateBasicDetailsEnteredStatus();

            // Assert
            haveUnknownBasicDetails.Should().BeFalse();
        }

        [Fact]
        public void CalculateAdditionalDetailsEnteredStatus_When_All_Additional_Details_Returns_True()
        {
            // Arrange
            var examination = new Examination
            {
                Representatives = new List<Representative>
                {
                    new Representative
                    {
                        AppointmentDate = new DateTime(2019, 2, 24),
                        AppointmentTime = new TimeSpan(11, 30, 0),
                        FullName = "fullName",
                        Informed = InformedAtDeath.Yes,
                        PhoneNumber = "123456789",
                        PresentAtDeath = PresentAtDeath.Yes,
                        Relationship = "relationship",
                    }
                },

                MedicalTeam = new MedicalTeam
                {
                    MedicalExaminerUserId = "MedicalExaminerUserId",
                    MedicalExaminerFullName = "MedicalExaminerFullName",
                    ConsultantResponsible = new ClinicalProfessional
                    {
                        Name = "ConsultantResponsibleName",
                        Role = "ConsultantResponsible",
                        Organisation = "Organisation",
                        Phone = "12345678",
                        Notes = "Notes",
                        GMCNumber = "GMCNumber",
                    },
                    Qap = new ClinicalProfessional
                    {
                        Name = "QapName",
                        Role = "Qap",
                        Organisation = "Organisation",
                        Phone = "12345678",
                        Notes = "Notes",
                        GMCNumber = "GMCNumber",
                    }
                },

                CaseBreakdown = new CaseBreakDown
                {
                    AdmissionNotes = new AdmissionNotesEventContainer
                    {
                        Latest = new AdmissionEvent
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
                            UserFullName = "usersFullName",
                        }
                    }
                }
            };

            // Act
            var additionalDetailsEntered = examination.CalculateAdditionalDetailsEnteredStatus();

            // Assert
            additionalDetailsEntered.Should().BeTrue();
        }

        [Fact]
        public void CalculateAdditionalDetailsEnteredStatus_When_No_Additional_Details_Returns_False()
        {
            // Arrange
            var examination = new Examination
            {
                Representatives = null,

                MedicalTeam = new MedicalTeam
                {
                    MedicalExaminerUserId = null,
                    MedicalExaminerFullName = null,
                    ConsultantResponsible = null,
                    Qap = null,
                },

                CaseBreakdown = new CaseBreakDown
                {
                    AdmissionNotes = new AdmissionNotesEventContainer
                    {
                        Latest = null
                    }
                }
            };

            // Act
            var additionalDetailsEntered = examination.CalculateAdditionalDetailsEnteredStatus();

            // Assert
            additionalDetailsEntered.Should().BeFalse();
        }

        [Fact]
        public void CalculateScrutinyCompleteStatus_When_No_Required_Events_Entered_Returns_False()
        {
            // Arrange
            var examination = new Examination
            {
                CaseBreakdown = new CaseBreakDown
                {
                    PreScrutiny = new PreScrutinyEventContainer
                    {
                        Latest = null
                    },
                    QapDiscussion = new QapDiscussionEventContainer
                    {
                        Latest = null
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer
                    {
                        Latest = null
                    }
                }
            };

            // Act
            var additionalDetailsEntered = examination.CalculateScrutinyCompleteStatus();

            // Assert
            additionalDetailsEntered.Should().BeFalse();
        }

        [Fact]
        public void CalculateScrutinyCompleteStatus_When_All_Required_Events_Entered_Returns_True()
        {
            // Arrange
            var examination = new Examination
            {
                CaseBreakdown = new CaseBreakDown
                {
                    PreScrutiny = new PreScrutinyEventContainer
                    {
                        Latest = new PreScrutinyEvent
                        {
                            CauseOfDeath1a = "CauseOfDeath1a",
                            CauseOfDeath1b = "CauseOfDeath1b",
                            CauseOfDeath1c = "CauseOfDeath1c",
                            CauseOfDeath2 = "CauseOfDeath2",
                            CircumstancesOfDeath = OverallCircumstancesOfDeath.Expected,
                            ClinicalGovernanceReview = ClinicalGovernanceReview.No,
                            ClinicalGovernanceReviewText = "ClinicalGovernanceReviewText",
                            Created = DateTime.Now,
                            EventId = "1",
                            IsFinal = true,
                            MedicalExaminerThoughts = "MedicalExaminerThoughts",
                            OutcomeOfPreScrutiny = OverallOutcomeOfPreScrutiny.IssueAnMccd,
                            UserFullName = "UserFullName",
                            UserId = "userId",
                            UsersRole = "UsersRole",
                        }
                    },
                    QapDiscussion = new QapDiscussionEventContainer
                    {
                        Latest = new QapDiscussionEvent
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
                            QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME,
                            UserFullName = "user full name",
                            UserId = "userId",
                            UsersRole = "user role",
                        }
                    },
                    BereavedDiscussion = new BereavedDiscussionEventContainer
                    {
                        Latest = new BereavedDiscussionEvent
                        {
                            BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted,
                            Created = DateTime.Now,
                            DateOfConversation = DateTime.Now,
                            TimeOfConversation = new TimeSpan(10, 00, 00),
                            DiscussionDetails = "Discussion details",
                            DiscussionUnableHappen = false,
                            EventId = "4",
                            InformedAtDeath = InformedAtDeath.Yes,
                            IsFinal = true,
                            UserId = "userId",
                            UserFullName = "user full name",
                            UsersRole = "users role",
                            ParticipantFullName = "ParticipantFullName",
                            ParticipantPhoneNumber = "ParticipantPhoneNumber",
                            ParticipantRelationship = "ParticipantRelationship",
                            PresentAtDeath = PresentAtDeath.No,
                        }
                    }
                }
            };

            // Act
            var additionalDetailsEntered = examination.CalculateScrutinyCompleteStatus();

            // Assert
            additionalDetailsEntered.Should().BeTrue();
        }

        [Fact]
        public void CalculateCaseItemsCompleteStatus_When_No_Case_Items_Entered_Returns_False()
        {
            // Arrange
            var examination = new Examination
            {
                CaseOutcome = new CaseOutcome
                {
                    MccdIssued = null,
                    CremationFormStatus = null,
                    GpNotifiedStatus = null,
                    CoronerReferralSent = false,
                },
                CaseCompleted = true
            };

            // Act
            var additionalDetailsEntered = examination.CalculateCaseItemsCompleteStatus();

            // Assert
            additionalDetailsEntered.Should().BeFalse();
        }

        [Fact]
        public void CalculateCaseItemsCompleteStatus_When_All_Case_Items_Entered_Returns_True()
        {
            // Arrange
            var examination = new Examination
            {
                CaseOutcome = new CaseOutcome
                {
                    MccdIssued = true,
                    CremationFormStatus = CremationFormStatus.Yes,
                    GpNotifiedStatus = GPNotified.GPNotified,
                    CoronerReferralSent = true,
                },
                CaseCompleted = true
            };

            // Act
            var additionalDetailsEntered = examination.CalculateCaseItemsCompleteStatus();

            // Assert
            additionalDetailsEntered.Should().BeTrue();
        }

        [Fact]
        public void CalculateCaseItemsCompleteStatus_When_Any_Unknown_Case_Items_Entered_Returns_True()
        {
            // Arrange
            var examination = new Examination
            {
                CaseOutcome = new CaseOutcome
                {
                    MccdIssued = true,
                    CremationFormStatus = CremationFormStatus.Unknown,
                    GpNotifiedStatus = GPNotified.GPNotified,
                    CoronerReferralSent = true,
                },
                CaseCompleted = true
            };

            // Act
            var additionalDetailsEntered = examination.CalculateCaseItemsCompleteStatus();

            // Assert
            additionalDetailsEntered.Should().BeNull();
        }

        [Fact]
        public void CalculateCaseItemsCompleteStatus_When_Refer_To_Coroner_And_CoronerReferralSent_Is_False_Entered_Returns_False()
        {
            // Arrange
            var examination = new Examination
            {
                CaseOutcome = new CaseOutcome
                {
                    CaseOutcomeSummary = CaseOutcomeSummary.ReferToCoroner,
                    MccdIssued = true,
                    CremationFormStatus = CremationFormStatus.Yes,
                    GpNotifiedStatus = GPNotified.GPNotified,
                    CoronerReferralSent = false,
                },
                CaseCompleted = true
            };

            // Act
            var additionalDetailsEntered = examination.CalculateCaseItemsCompleteStatus();

            // Assert
            additionalDetailsEntered.Should().BeFalse();
        }

        [Fact]
        public void CalculateCaseItemsCompleteStatus_When_Case_Not_Closed_Returns_False()
        {
            // Arrange
            var examination = new Examination
            {
                CaseOutcome = new CaseOutcome
                {
                    CaseOutcomeSummary = CaseOutcomeSummary.ReferToCoroner,
                    MccdIssued = true,
                    CremationFormStatus = CremationFormStatus.Yes,
                    GpNotifiedStatus = GPNotified.GPNotified,
                    CoronerReferralSent = true,
                },
                CaseCompleted = false
            };

            // Act
            var additionalDetailsEntered = examination.CalculateCaseItemsCompleteStatus();

            // Assert
            additionalDetailsEntered.Should().BeFalse();
        }
    }
}
