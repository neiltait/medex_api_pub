using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    public static class ExaminationExtensionMethods
    {
        public static Examination MyUsersView(this Examination examination, MeUser myUser)
        {
            IEnumerable<OtherEvent> otherEvents = GetOtherEventsView(examination.Events.OtherEvents, myUser);

            examination.Events.OtherEvents = otherEvents;
            return examination;
        }

        private static IEnumerable<OtherEvent> GetOtherEventsView(IEnumerable<OtherEvent> otherEvents, MeUser myUser)
        {
            foreach(var amendment in otherEvents)
            {
                if (amendment.Amendments != null)
                {
                    var finalsAtLevel = amendment.Amendments.Where(o => (o.EventStatus == Enums.EventStatus.Final)
                || ((o.EventStatus == Enums.EventStatus.Draft) && (o.UserId == myUser.UserId)));
                    amendment.Amendments = GetOtherEventsView(finalsAtLevel, myUser);
                }
            }
            return otherEvents;
        }
    }
}
