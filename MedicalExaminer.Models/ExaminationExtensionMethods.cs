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
            var score = 0;
            const int defaultScoreWeighting = 100;
            const int overdueScoreWeighting = 1000;

            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            if (examination.ChildPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (examination.CoronerPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (examination.CulturalPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (examination.FaithPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (examination.OtherPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (DateTime.Now.AddDays(-4) > examination.CaseCreated)
            {
                score = score + overdueScoreWeighting;
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
