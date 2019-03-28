using System.Collections.Generic;

namespace MedicalExaminer.Models
{
    public class OtherEventContainer : BaseEventContainter<OtherEvent>
    {
        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; }
        public override IList<IEvent> History { get; set; }

        public override void Add(OtherEvent theEvent)
        {
            if (Drafts == null)
            {
                Drafts = new List<IEvent>();
            }

            if (History == null)
            {
                History = new List<IEvent>();
            }

            base.Add(theEvent);
        }
    }

    public class NoteEventContainer : BaseEventContainter<NoteEvent>
    {
        public override IEvent Latest { get; set; }
        public override IList<IEvent> Drafts { get; set; }
        public override IList<IEvent> History { get; set; }

        public override void Add(NoteEvent theEvent)
        {
            if (Drafts == null)
            {
                Drafts = new List<IEvent>();
            }

            if (History == null)
            {
                History = new List<IEvent>();
            }

            base.Add(theEvent);
        }
    }
}
