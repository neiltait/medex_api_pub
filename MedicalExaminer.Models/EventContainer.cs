using System.Collections.Generic;

namespace MedicalExaminer.Models
{
    public class OtherEventContainer : BaseEventContainter<OtherEvent>
    {

        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; } = new List<IEvent>();
        public override IList<IEvent> History { get; set; } = new List<IEvent>();

        public override void Add(OtherEvent theEvent)
        {
            base.Add(theEvent);
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
