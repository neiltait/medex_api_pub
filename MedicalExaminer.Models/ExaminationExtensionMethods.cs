using System.Linq;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public static class ExaminationExtensionMethods
    {
        public static Examination MyUsersView(this Examination examination, MeUser myUser)
        {
            examination.Events = GetUserEvents(examination.Events, myUser);
            return examination;
        }

        public static Examination SaveEvent(this Examination examination, EventType eventType, IEvent theEvent)
        {
           switch (eventType)
            {
                case EventType.Other:
                    var otherEventContainer = examination.Events.OtherEvents;
                    otherEventContainer.Add((OtherEvent)theEvent);
                    break;
                case EventType.Notes:
                    var notesContainer = examination.Events.NoteworthyEvents;
                    notesContainer.Add((NoteEvent)theEvent);
                    break;
            }
            return examination;
        }

        private static CaseBreakDown GetUserEvents(CaseBreakDown caseBreakDown, MeUser myUser)
        {
            if(caseBreakDown == null)
            {
                return null;
            }

            caseBreakDown.OtherEvents = GetEvents<OtherEvent>(caseBreakDown.OtherEvents, myUser);
            return caseBreakDown;
        }

        private static BaseEventContainter<T> GetEvents<T>(IEventContainer<IEvent> otherEvents, MeUser myUser) where T : IEvent
        {
            var usersDrafts = otherEvents.Drafts.Where(draft => draft.UserId == myUser.UserId);
            otherEvents.Drafts = usersDrafts.ToList();
            return (BaseEventContainter<T>)otherEvents;
        }

        

    }
}
