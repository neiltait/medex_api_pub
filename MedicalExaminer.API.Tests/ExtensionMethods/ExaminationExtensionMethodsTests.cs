using System;
using System.Linq;
using FluentAssertions;
using MedicalExaminer.Models;
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
        public void No_AdmissionNotes_Case_Status_Pending_Admission_Notes_True()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.Equal(true, examination.PendingAdmissionNotes);
        }

        [Fact]
        public void Draft_AdmissionNotes_Case_Status_Pending_Admission_Notes_True()
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
            Assert.Equal(true, examination.PendingAdmissionNotes);
        }

        [Fact]
        public void Latest_AdmissionNotes_Case_Status_Pending_Admission_Notes_False()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            var admissionNotes = new AdmissionNotesEventContainer();
            admissionNotes.Latest = new AdmissionEvent();
            caseBreakDown.AdmissionNotes = admissionNotes;
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.Equal(true, examination.PendingAdmissionNotes);
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
            Assert.Equal(false, examination.AdmissionNotesHaveBeenAdded);
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
            Assert.Equal(false, examination.AdmissionNotesHaveBeenAdded);
        }

        [Fact]
        public void Latest_AdmissionNotes_No_Consultant_Case_Status_Admission_Notes_Have_Been_Added_False()
        {
            // Arrange
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            var admissionNotes = new AdmissionNotesEventContainer();
            admissionNotes.Latest = new AdmissionEvent();
            caseBreakDown.AdmissionNotes = admissionNotes;
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.Equal(false, examination.AdmissionNotesHaveBeenAdded);
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
            admissionNotes.Latest = new AdmissionEvent();
            caseBreakDown.AdmissionNotes = admissionNotes;
            examination.CaseBreakdown = caseBreakDown;

            // Act
            examination = examination.UpdateCaseStatus();

            // Assert
            Assert.Equal(true, examination.AdmissionNotesHaveBeenAdded);
        }

        [Fact]
        public void No_Me_Scrutiny_Case_Status_HaveBeenScrutinisedByME_False()
        {
            var examination = new Examination();
            var caseBreakDown = new CaseBreakDown();
            examination.CaseBreakdown = caseBreakDown;

            examination = examination.UpdateCaseStatus();

            Assert.Equal(false, examination.HaveBeenScrutinisedByME);
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
    }
}
