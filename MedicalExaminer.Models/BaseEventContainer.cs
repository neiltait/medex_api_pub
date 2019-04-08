using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    public abstract class BaseEventContainter<TEvent> : IEventContainer<TEvent>
        where TEvent : IEvent
    {
        public abstract TEvent Latest { get; set; }

        public abstract IList<TEvent> Drafts { get; set; }

        public abstract IList<TEvent> History { get; set; }


        public virtual void Add(TEvent theEvent)
        {
            if (string.IsNullOrEmpty(theEvent.EventId))
            {
                theEvent.EventId = Guid.NewGuid().ToString();
            }

            if (theEvent.IsFinal)
            {
                theEvent.Created = DateTime.Now;
                Latest = theEvent;
                History.Add(theEvent);
                Drafts.Clear();
                return;
            }
            else
            {
                var userHasDraft = Drafts.Any(draft => draft.UserId == theEvent.UserId);
                if (userHasDraft)
                {
                    theEvent.Created = theEvent.Created == null ? DateTime.Now : theEvent.Created;
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