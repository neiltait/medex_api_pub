using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    public class OtherEventContainer : BaseEventContainter<OtherEvent>
    {

        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; } = new List<IEvent>();
        public override IList<IEvent> History { get; set; } = new List<IEvent>();

        public override void Add(OtherEvent theEvent)
        {
            if (string.IsNullOrEmpty(theEvent.EventId))
            {
                theEvent.EventId = Guid.NewGuid().ToString();
            }

            if (theEvent.IsFinal)
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
            else
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

    public class PreScrutinyEventContainer : BaseEventContainter<PreScrutinyEvent>
    {

        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; } = new List<IEvent>();
        public override IList<IEvent> History { get; set; } = new List<IEvent>();

        public override void Add(PreScrutinyEvent theEvent)
        {
            base.Add(theEvent);
        }
    }

    public class BereavedDiscussionEventContainer: BaseEventContainter<BereavedDiscussionEvent>
    {
        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; } = new List<IEvent>();
        public override IList<IEvent> History { get; set; } = new List<IEvent>();

        public override void Add(BereavedDiscussionEvent theEvent)
        {
            base.Add(theEvent);
        }
    }

    public class MeoSummaryEventContainer: BaseEventContainter<MeoSummaryEvent>
    {
        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; } = new List<IEvent>();
        public override IList<IEvent> History { get; set; } = new List<IEvent>();

        public override void Add(MeoSummaryEvent theEvent)
        {
            base.Add(theEvent);
        }
    }
    
    public class AdmissionNotesEventContainer: BaseEventContainter<AdmissionEvent>
    {
        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; } = new List<IEvent>();
        public override IList<IEvent> History { get; set; } = new List<IEvent>();

        public override void Add(AdmissionEvent theEvent)
        {
            base.Add(theEvent);
        }
    }

    public class MedicalHistoryEventContainer: BaseEventContainter<MedicalHistoryEvent>
    {
        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; } = new List<IEvent>();
        public override IList<IEvent> History { get; set; } = new List<IEvent>();

        public override void Add(MedicalHistoryEvent theEvent)
        {
            base.Add(theEvent);
        }
    }
    public class QapDiscussionEventContainer: BaseEventContainter<QapDiscussionEvent>
    {
        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; } = new List<IEvent>();
        public override IList<IEvent> History { get; set; } = new List<IEvent>();

        public override void Add(QapDiscussionEvent theEvent)
        {
            base.Add(theEvent);
        }
    }
}
