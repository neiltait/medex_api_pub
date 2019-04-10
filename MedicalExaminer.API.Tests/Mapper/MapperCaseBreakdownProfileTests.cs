using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;
using System;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    public class MapperCaseBreakdownProfileTests
    {
        private readonly IMapper _mapper;
        public MapperCaseBreakdownProfileTests()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<CaseBreakdownProfile>();
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
            
            var result = _mapper.Map<CaseBreakDownItem>(caseBreakdown, opt=>opt.Items["myUser"] = myUser);

            Assert.Equal(latest.EventId, result.OtherEvents.Latest.EventId);
            result.OtherEvents.History.Should().BeEquivalentTo(history);
            Assert.Null(result.OtherEvents.UsersDraft);

            var otherResult = _mapper.Map<CaseBreakDownItem>(caseBreakdown, opt => opt.Items["myUser"] = otherUser);

            Assert.Equal(latest.EventId, result.OtherEvents.Latest.EventId);
            otherResult.OtherEvents.History.Should().BeEquivalentTo(history);
            Assert.Equal(otherUserDraft.EventId, otherResult.OtherEvents.UsersDraft.EventId);
        }
    }
}
