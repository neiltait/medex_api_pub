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
            if (theEvent.EventStatus == Enums.EventStatus.Final)
            {
                Latest = theEvent;
                History.Add(theEvent);
                Drafts.Remove(theEvent);
                return;
            }

            if (theEvent.EventStatus == Enums.EventStatus.Draft)
            {
                var usersDraft = Drafts.SingleOrDefault(draft => draft.UserId == theEvent.UserId
                && draft.EventId == theEvent.EventId);
                if (usersDraft != null)
                {
                    usersDraft = theEvent;
                }

                Drafts.Add(theEvent);
            }
        }
    }
}
