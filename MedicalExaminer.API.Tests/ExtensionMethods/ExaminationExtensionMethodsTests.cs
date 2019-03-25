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
                EventId = "1",
                EventStatus = EventStatus.Draft,
                EventText = "Hello Earth",
                UserId = "123"
            };

            var amendmentUser3 = new OtherEvent()
            {
                EventId = "1",
                EventStatus = EventStatus.Final,
                EventText = "Hello Earth",
                UserId = "123"
            };

            var amendmentUser4 = new OtherEvent()
            {
                EventId = "1",
                EventStatus = EventStatus.Draft,
                EventText = "Hello Earth",
                UserId = "456"
            };

            var otherEvent1 = new OtherEvent()
            {
                EventId = "1",
                EventStatus = EventStatus.Final,
                EventText = "Hello Earth",
                UserId = "123",
                Amendments = new[] { amendmentUser1, amendmentUser2 }
            };

            var otherEvent2 = new OtherEvent()
            {
                EventId = "1",
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

            Assert.Single(result.Events.OtherEvents);
            var myEvent = result.Events.OtherEvents.First();
            Assert.Single(myEvent.Amendments);
        }
    }
}
