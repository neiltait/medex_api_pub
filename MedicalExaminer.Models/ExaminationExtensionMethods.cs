using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    public static class ExaminationExtensionMethods
    {
        public static Examination MyUsersView(this Examination examination, MeUser myUser)
        {
            examination.Events = GetUserEvents(examination.Events, myUser);
            
            return examination;
        }

        private static CaseBreakDown GetUserEvents(CaseBreakDown caseBreakDown, MeUser myUser)
        {
            if(caseBreakDown == null)
            {
                return null;
            }

            caseBreakDown.OtherEvents = (IEnumerable<OtherEvent>)GetEventsView(caseBreakDown.OtherEvents, myUser);

            return caseBreakDown;
        }

        private static IEnumerable<IEvent> GetEventsView(IEnumerable<IEvent> otherEvents, MeUser myUser)
        {
            if(otherEvents == null)
            {
                return null;
            }
            foreach(var amendment in otherEvents)
            {
                if (amendment.Amendments != null)
                {
                    var finalsAtLevel = amendment.Amendments.Where(o => (o.EventStatus == Enums.EventStatus.Final)
                || ((o.EventStatus == Enums.EventStatus.Draft) && (o.UserId == myUser.UserId)));
                    amendment.Amendments = GetEventsView(finalsAtLevel, myUser);
                }
            }
            return otherEvents;
        }
    }
}
