using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    public class OtherEventContainer : BaseEventContainter<OtherEvent>
    {

        public override OtherEvent Latest { get; set; }
        public override IList<OtherEvent> Drafts { get; set; } = new List<OtherEvent>();
        public override IList<OtherEvent> History { get; set; } = new List<OtherEvent>();

        public override void Add(OtherEvent theEvent)
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

    public class PreScrutinyEventContainer : BaseEventContainter<PreScrutinyEvent>
    {

        public override PreScrutinyEvent Latest { get; set; }
        public override IList<PreScrutinyEvent> Drafts { get; set; } = new List<PreScrutinyEvent>();
        public override IList<PreScrutinyEvent> History { get; set; } = new List<PreScrutinyEvent>();

        public override void Add(PreScrutinyEvent theEvent)
        {
            base.Add(theEvent);
        }
    }

    public class BereavedDiscussionEventContainer: BaseEventContainter<BereavedDiscussionEvent>
    {
        public override BereavedDiscussionEvent Latest { get; set; }
        public override IList<BereavedDiscussionEvent> Drafts { get; set; } = new List<BereavedDiscussionEvent>();
        public override IList<BereavedDiscussionEvent> History { get; set; } = new List<BereavedDiscussionEvent>();

        public override void Add(BereavedDiscussionEvent theEvent)
        {
            base.Add(theEvent);
        }
    }

    public class MeoSummaryEventContainer: BaseEventContainter<MeoSummaryEvent>
    {
        public override MeoSummaryEvent Latest { get; set; }
        public override IList<MeoSummaryEvent> Drafts { get; set; } = new List<MeoSummaryEvent>();
        public override IList<MeoSummaryEvent> History { get; set; } = new List<MeoSummaryEvent>();

        public override void Add(MeoSummaryEvent theEvent)
        {
            base.Add(theEvent);
        }
    }
    
    public class AdmissionNotesEventContainer: BaseEventContainter<AdmissionEvent>
    {
        public override AdmissionEvent Latest { get; set; }
        public override IList<AdmissionEvent> Drafts { get; set; } = new List<AdmissionEvent>();
        public override IList<AdmissionEvent> History { get; set; } = new List<AdmissionEvent>();

        public override void Add(AdmissionEvent theEvent)
        {
            base.Add(theEvent);
        }
    }

    public class MedicalHistoryEventContainer: BaseEventContainter<MedicalHistoryEvent>
    {
        public override MedicalHistoryEvent Latest { get; set; }
        public override IList<MedicalHistoryEvent> Drafts { get; set; } = new List<MedicalHistoryEvent>();
        public override IList<MedicalHistoryEvent> History { get; set; } = new List<MedicalHistoryEvent>();

        public override void Add(MedicalHistoryEvent theEvent)
        {
            base.Add(theEvent);
        }
    }
    public class QapDiscussionEventContainer: BaseEventContainter<QapDiscussionEvent>
    {
        public override QapDiscussionEvent Latest { get; set; }
        public override IList<QapDiscussionEvent> Drafts { get; set; } = new List<QapDiscussionEvent>();
        public override IList<QapDiscussionEvent> History { get; set; } = new List<QapDiscussionEvent>();

        public override void Add(QapDiscussionEvent theEvent)
        {
            base.Add(theEvent);
        }
    }
}
