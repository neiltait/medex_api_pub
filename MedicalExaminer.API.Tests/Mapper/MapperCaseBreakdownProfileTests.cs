using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    public class MapperCaseBreakdownProfileTests
    {
        IMapper _mapper;
        public MapperCaseBreakdownProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<CaseBreakdownProfile>(); });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Mapper()
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
                EventStatus = MedicalExaminer.Models.Enums.EventStatus.Draft,
                EventText = "draft user one",
                UserId = "123"
            };

            var otherUserDraft = new OtherEvent()
            {
                EventId = "3",
                EventStatus = MedicalExaminer.Models.Enums.EventStatus.Draft,
                EventText = "draft user other",
                UserId = "456"
            };


            var latest = new OtherEvent()
            {
                EventId = "1",
                EventStatus = MedicalExaminer.Models.Enums.EventStatus.Final,
                EventText = "The others",
                UserId = "123"
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

            Assert.Equal(latest, result.OtherEvents.Latest);
            result.OtherEvents.History.Should().BeEquivalentTo(history);
            Assert.Null(result.OtherEvents.UsersDraft);

            var otherResult = _mapper.Map<CaseBreakDownItem>(caseBreakdown, opt => opt.Items["myUser"] = otherUser);

            Assert.Equal(latest, otherResult.OtherEvents.Latest);
            otherResult.OtherEvents.History.Should().BeEquivalentTo(history);
            Assert.Equal(otherUserDraft, otherResult.OtherEvents.UsersDraft);
        }
    }
}
