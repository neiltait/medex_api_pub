using System.Collections.Generic;

namespace MedicalExaminer.Models
{
    public class OtherEventContainer : BaseEventContainter<OtherEvent>
    {

        public override OtherEvent Latest { get; set; }
        public override IList<OtherEvent> Drafts { get; set; } = new List<OtherEvent>();
        public override IList<OtherEvent> History { get; set; } = new List<OtherEvent>();

        public override void Add(OtherEvent theEvent)
        {
            base.Add(theEvent);
        }
    }
}
