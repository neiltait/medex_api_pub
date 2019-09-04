using System;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    public class MapperCaseBreakdownProfileTests
    {
        private readonly IMapper _mapper;

        public MapperCaseBreakdownProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExaminationProfile>();
                cfg.AddProfile<OtherEventProfile>();
                cfg.AddProfile<AdmissionEventProfile>();
                cfg.AddProfile<BereavedDiscussionEventProfile>();
                cfg.AddProfile<MedicalHistoryEventProfile>();
                cfg.AddProfile<MeoSummaryEventProfile>();
                cfg.AddProfile<PreScrutinyEventProfile>();
                cfg.AddProfile<QapDiscussionEventProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void UserDraftsReturned()
        {
            var myUser = new MeUser()
            {
                UserId = "123"
            };

            var otherUser = new MeUser()
            {
                UserId = "456"
            };

            var myUserDraft = new OtherEvent()
            {
                EventId = "2",
                IsFinal = false,
                Text = "draft user one",
                UserId = "123",
                Created = DateTime.Now
            };

            var otherUserDraft = new OtherEvent()
            {
                EventId = "3",
                IsFinal = false,
                Text = "draft user other",
                UserId = "456",
                Created = DateTime.Now
            };

            var latest = new OtherEvent()
            {
                EventId = "1",
                IsFinal = true,
                Text = "The others",
                UserId = "123",
                Created = DateTime.Now
            };
            var drafts = new[] { otherUserDraft };
            var history = new[] { latest };

            var caseBreakdown = new CaseBreakDown()
            {
                OtherEvents = new OtherEventContainer()
                {
                    Drafts = drafts,
                    Latest = latest,
                    History = history
                }
            };

            var examination = new Examination()
            {
                CaseBreakdown = caseBreakdown
            };

            var result = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = myUser);

            Assert.Equal(latest.EventId, result.OtherEvents.Latest.EventId);
            result.OtherEvents.History.Should().BeEquivalentTo(history);
            Assert.Null(result.OtherEvents.UsersDraft);

            var otherResult = _mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = otherUser);

            Assert.Equal(latest.EventId, result.OtherEvents.Latest.EventId);
            otherResult.OtherEvents.History.Should().BeEquivalentTo(history);
            Assert.Equal(otherUserDraft.EventId, otherResult.OtherEvents.UsersDraft.EventId);
        }

        [Fact]
        public void Bereaved_Discussion_Outcome_When_Not_Marked_As_Unable_To_Happen()
        {
            // Arrange
            var request = new PutBereavedDiscussionEventRequest
            {
                EventId = "EventId",
                IsFinal = true,
                ParticipantFullName = "ParticipantFullName",
                ParticipantRelationship = "ParticipantRelationship",
                ParticipantPhoneNumber = "ParticipantPhoneNumber",
                PresentAtDeath = PresentAtDeath.Yes,
                InformedAtDeath = InformedAtDeath.Yes,
                DateOfConversation = DateTime.Now,
                TimeOfConversation = new TimeSpan(11, 00, 00),
                DiscussionUnableHappen = false,
                DiscussionUnableHappenDetails = null,
                DiscussionDetails = "DiscussionDetails",
                BereavedDiscussionOutcome = BereavedDiscussionOutcome.CauseOfDeathAccepted
            };

            // Act
            var theEvent = _mapper.Map<BereavedDiscussionEvent>(request);

            // Assert
            theEvent.BereavedDiscussionOutcome.Should().Be(request.BereavedDiscussionOutcome);
        }

        [Fact]
        public void Bereaved_Discussion_Outcome_When_Marked_As_Unable_To_Happen()
        {
            // Arrange
            var request = new PutBereavedDiscussionEventRequest
            {
                EventId = null,
                IsFinal = true,
                ParticipantFullName = null,
                ParticipantRelationship = null,
                ParticipantPhoneNumber = null,
                PresentAtDeath = null,
                InformedAtDeath = null,
                DateOfConversation = null,
                TimeOfConversation = null,
                DiscussionUnableHappen = true,
                DiscussionUnableHappenDetails = null,
                DiscussionDetails = null,
                BereavedDiscussionOutcome = null
            };

            // Act
            var theEvent = _mapper.Map<BereavedDiscussionEvent>(request);

            // Assert
            theEvent.BereavedDiscussionOutcome.Should().Be(BereavedDiscussionOutcome.DiscussionUnableToHappen);
        }

        [Fact]
        public void QAP_Discussion_Outcome_When_Not_Marked_As_Unable_To_Happen()
        {
            // Arrange
            var request = new PutQapDiscussionEventRequest
            {
                EventId = "EventId",
                IsFinal = true,
                ParticipantRole = "ParticipantRole",
                ParticipantOrganisation = "ParticipantOrganisation",
                ParticipantPhoneNumber = "ParticipantPhoneNumber",
                DateOfConversation = DateTime.Now,
                TimeOfConversation = new TimeSpan(11, 00, 00),
                DiscussionUnableHappen = false,
                DiscussionDetails = "DiscussionDetails",
                QapDiscussionOutcome = QapDiscussionOutcome.MccdCauseOfDeathProvidedByME,
                ParticipantName = "ParticipantName",
                CauseOfDeath1a = "CauseOfDeath1a",
                CauseOfDeath1b = "CauseOfDeath1b",
                CauseOfDeath1c = "CauseOfDeath1c",
                CauseOfDeath2 = "CauseOfDeath2"
            };

            // Act
            var theEvent = _mapper.Map<QapDiscussionEvent>(request);

            // Assert
            theEvent.QapDiscussionOutcome.Should().Be(request.QapDiscussionOutcome);
        }

        [Fact]
        public void QAP_Discussion_Outcome_When_Marked_As_Unable_To_Happen()
        {
            // Arrange
            var request = new PutQapDiscussionEventRequest
            {
                EventId = null,
                IsFinal = true,
                ParticipantRole = null,
                ParticipantOrganisation = null,
                ParticipantPhoneNumber = null,
                DateOfConversation = null,
                TimeOfConversation = null,
                DiscussionUnableHappen = true,
                DiscussionDetails = null,
                QapDiscussionOutcome = null,
                ParticipantName = null,
                CauseOfDeath1a = null,
                CauseOfDeath1b = null,
                CauseOfDeath1c = null,
                CauseOfDeath2 = null
            };

            // Act
            var theEvent = _mapper.Map<QapDiscussionEvent>(request);

            // Assert
            theEvent.QapDiscussionOutcome.Should().Be(QapDiscussionOutcome.DiscussionUnableToHappen);
        }
    }
}
