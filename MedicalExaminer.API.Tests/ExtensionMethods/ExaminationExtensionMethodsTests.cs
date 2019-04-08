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
                UserId = "userOne"
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
        public void When_No_Urgency_Indicators_Are_Selected_Then_The_Urgency_Score_Is_Zero()
        {
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
            };
            var result = examination.UpdateCaseUrgencyScore();
            Assert.Equal(0, result.UrgencyScore);
        }

        [Fact]
        public void When_All_Urgency_Indicators_Are_Selected_Then_The_Urgency_Score_Is_Five()
        {
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
            };
            var result = examination.UpdateCaseUrgencyScore();
            Assert.Equal(5, result.UrgencyScore);
        }

        [Fact]
        public void When_No_Urgency_Indicators_Are_Selected_Then_The_Urgency_Score_Is_Five()
        {
            DateTime dateToCheck = new DateTime(2019, 4, 5);

            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
                CaseCreated = new DateTime(2019, 4, 1)
            };
            var result = examination.UpdateCaseUrgencyScore();
            Assert.Equal(1, result.UrgencyScore); // what score should we expect when 
        }
    }
}
