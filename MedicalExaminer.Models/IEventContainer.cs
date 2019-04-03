using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    public interface IEventContainer<out TEvent>
        where TEvent : IEvent
    {
        IEvent Latest { get; }

        IList<IEvent> Drafts { get; set; }

        IList<IEvent> History { get; set; }
    }

    public abstract class BaseEventContainter<TEvent> : IEventContainer<TEvent>
        where TEvent : IEvent
    {
        public abstract IEvent Latest { get; set; }

        public abstract IList<IEvent> Drafts { get; set; }

        public abstract IList<IEvent> History { get; set; }


        public virtual void Add(TEvent theEvent)
        {
            if (string.IsNullOrEmpty(theEvent.EventId))
            {
                theEvent.EventId = Guid.NewGuid().ToString();
            }

            if (theEvent.EventStatus == Enums.EventStatus.Final)
            {
                Latest = theEvent;
                History.Add(theEvent);
                var draft = Drafts.SingleOrDefault(d => d.EventId == theEvent.EventId);
                if (draft != null)
                {
                    Drafts.Remove(draft);
                }

                return;
            }

            if (theEvent.EventStatus == Enums.EventStatus.Draft)
            {
                var userHasDraft = Drafts.Any(draft => draft.UserId == theEvent.UserId);
                if (userHasDraft)
                {
                    var usersDraft = Drafts.Single(draft => draft.EventId == theEvent.EventId);
                    Drafts.Remove(usersDraft);
                    Drafts.Add(theEvent);
                    return;
                }

                Drafts.Add(theEvent);
            }
        }

    }
}
