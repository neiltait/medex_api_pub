using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    public class OtherEventContainer : BaseEventContainter<OtherEvent>
    {
        public OtherEventContainer()
        {
            Drafts = new List<OtherEvent>();
            History = new List<OtherEvent>();
        }

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
                theEvent.Created = theEvent.Created == null ? DateTime.Now : theEvent.Created;
                var userHasDraft = Drafts.Any(draft => draft.UserId == theEvent.UserId);
                if (userHasDraft)
                {
                    var usersDraft = Drafts.SingleOrDefault(draft => draft.EventId == theEvent.EventId);

                    if (usersDraft == null)
                    {
                        throw new ArgumentException(nameof(theEvent.EventId));
                    }

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
        public PreScrutinyEventContainer()
        {
            Drafts = new List<PreScrutinyEvent>();
            History = new List<PreScrutinyEvent>();
        }
    }

    public class BereavedDiscussionEventContainer: BaseEventContainter<BereavedDiscussionEvent>
    {
        public BereavedDiscussionEventContainer()
        {
            Drafts = new List<BereavedDiscussionEvent>();
            History = new List<BereavedDiscussionEvent>();
        }
    }

    public class MeoSummaryEventContainer: BaseEventContainter<MeoSummaryEvent>
    {
        public MeoSummaryEventContainer()
        {
            Drafts = new List<MeoSummaryEvent>();
            History = new List<MeoSummaryEvent>();
        }
    }
    
    public class AdmissionNotesEventContainer: BaseEventContainter<AdmissionEvent>
    {
        public AdmissionNotesEventContainer()
        {
            Drafts = new List<AdmissionEvent>();
            History = new List<AdmissionEvent>();
        }
    }

    public class MedicalHistoryEventContainer: BaseEventContainter<MedicalHistoryEvent>
    {
        public MedicalHistoryEventContainer()
        {
            Drafts = new List<MedicalHistoryEvent>();
            History = new List<MedicalHistoryEvent>();
        }
    }

    public class QapDiscussionEventContainer: BaseEventContainter<QapDiscussionEvent>
    {
        public QapDiscussionEventContainer()
        {
            Drafts = new List<QapDiscussionEvent>();
            History = new List<QapDiscussionEvent>();
        }
    }
}
