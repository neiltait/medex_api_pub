using System.Linq;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.ExtensionMethods
{
    public class ExaminationExtensionMethodsTests
    {
        [Fact]
        public void MyUsersView_OtherEvents_Filtered()
        {
            var myUserId = "456";
            var amendmentUser1 = new OtherEvent()
            {
                EventId = "1",
                EventStatus = EventStatus.Draft,
                EventText = "Hello Earth",
                UserId = "456"
            };

            var amendmentUser2 = new OtherEvent()
            {
                EventId = "2",
                EventStatus = EventStatus.Draft,
                EventText = "Hello Earth",
                UserId = "123"
            };

            var amendmentUser3 = new OtherEvent()
            {
                EventId = "3",
                EventStatus = EventStatus.Final,
                EventText = "Hello Earth",
                UserId = "123"
            };

            var amendmentUser4 = new OtherEvent()
            {
                EventId = "4",
                EventStatus = EventStatus.Draft,
                EventText = "Hello Earth",
                UserId = "456"
            };

            var otherEvent1 = new OtherEvent()
            {
                EventId = "5",
                EventStatus = EventStatus.Final,
                EventText = "Hello Earth",
                UserId = "123",
                Amendments = new[] { amendmentUser1, amendmentUser2 }
            };

            var otherEvent2 = new OtherEvent()
            {
                EventId = "6",
                EventStatus = EventStatus.Final,
                EventText = "Hello Earth",
                UserId = "123",
                Amendments = new[] { amendmentUser3, amendmentUser4 }
            };


            var caseBreakdown = new CaseBreakDown()
            {
                OtherEvents = new[] { otherEvent1, otherEvent2 }
            };
            var examination = new Examination()
            {
                Events = caseBreakdown
            };

            var myUser = new MeUser()
            {
                UserId = "456"
            };

            var result = examination.MyUsersView(myUser);

            Assert.Equal(2, result.Events.OtherEvents.Count());
            var firstEvent = result.Events.OtherEvents.First();

            Assert.Single(firstEvent.Amendments);
            var firstTestAmendment = firstEvent.Amendments.First();
            Assert.Equal("1", firstTestAmendment.EventId);
            var lastEvent = result.Events.OtherEvents.Last();
            var secondTestAmendment = lastEvent.Amendments.First();
            Assert.Equal("3", secondTestAmendment.EventId);
            var thirdTestAmendment = lastEvent.Amendments.Last();
            Assert.Equal("4", thirdTestAmendment.EventId);
        }
    }
}
