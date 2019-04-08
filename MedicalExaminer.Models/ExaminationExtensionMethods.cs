using System;
using System.Linq;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public static class ExaminationExtensionMethods
    {
        public static Examination AddEvent(this Examination examination, IEvent theEvent)
        {
           switch (theEvent.EventType)
           {
                case EventType.Other:
                    var otherEventContainer = examination.CaseBreakdown.OtherEvents;

                    if (otherEventContainer == null)
                    {
                        otherEventContainer = new OtherEventContainer();
                    }

                    otherEventContainer.Add((OtherEvent)theEvent);
                    break;
                default:
                    throw new NotImplementedException();
           }

            examination = UpdateCaseStatus(examination);
            return examination;
        }

        public static Examination UpdateCaseUrgencyScore(this Examination examination)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            var score = 0;
            if (examination.ChildPriority)
            {
                score = score + 100;
            }

            if (examination.CoronerPriority)
            {
                score = score + 100;
            }

            if (examination.CulturalPriority)
            {
                score = score + 100;
            }

            if (examination.FaithPriority)
            {
                score = score + 100;
            }

            if (examination.OtherPriority)
            {
                score = score + 100;
            }

            if (DateTime.Now > examination.CaseCreated.AddDays(5))
            {
                score = score + 1000;
            }

            examination.UrgencyScore = score;
            return examination;
        }

        private static Examination UpdateCaseStatus(Examination examination)
        {
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
