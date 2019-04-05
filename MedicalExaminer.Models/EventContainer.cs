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
}
