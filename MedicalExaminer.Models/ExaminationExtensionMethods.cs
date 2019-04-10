using System;
using System.Linq;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public static class ExaminationExtensionMethods
    {
        public static Examination AddEvent(this Examination examination, IEvent theEvent)
        {
            switch (theEvent.EventType)
            {
                case EventType.Other:
                    var otherEventContainer = examination.CaseBreakdown.OtherEvents;

                    if (otherEventContainer == null)
                    {
                        otherEventContainer = new OtherEventContainer();
                    }

                    otherEventContainer.Add((OtherEvent)theEvent);
                    break;
                case EventType.PreScrutiny:
                    var preScrutinyEventContainer = examination.CaseBreakdown.PreScrutiny;

                    if (preScrutinyEventContainer == null)
                    {
                        preScrutinyEventContainer = new PreScrutinyEventContainer();
                    }

                    preScrutinyEventContainer.Add((PreScrutinyEvent)theEvent);
                    break;
                case EventType.BereavedDiscussion:
                    var bereavedDiscussionEventContainer = examination.CaseBreakdown.BereavedDiscussion;

                    if (bereavedDiscussionEventContainer == null)
                    {
                        bereavedDiscussionEventContainer = new BereavedDiscussionEventContainer();
                    }

                    bereavedDiscussionEventContainer.Add((BereavedDiscussionEvent)theEvent);
                    break;
                case EventType.MeoSummary:
                    var meoSummaryEventContainer = examination.CaseBreakdown.MeoSummary;

                    if (meoSummaryEventContainer == null)
                    {
                        meoSummaryEventContainer = new MeoSummaryEventContainer();
                    }

                    meoSummaryEventContainer.Add((MeoSummaryEvent)theEvent);
                    break;
                case EventType.QapDiscussion:
                    var qapDiscussionEventContainer = examination.CaseBreakdown.QapDiscussion;

                    if (qapDiscussionEventContainer == null)
                    {
                        qapDiscussionEventContainer = new QapDiscussionEventContainer();
                    }

                    qapDiscussionEventContainer.Add((QapDiscussionEvent)theEvent);
                    break;
                case EventType.MedicalHistory:
                    var medicalHistoryEventContainer = examination.CaseBreakdown.MedicalHistory;

                    if (medicalHistoryEventContainer == null)
                    {
                        medicalHistoryEventContainer = new MedicalHistoryEventContainer();
                    }

                    medicalHistoryEventContainer.Add((MedicalHistoryEvent)theEvent);
                    break;
                case EventType.Admission:
                    var admissionEventContainer = examination.CaseBreakdown.AdmissionNotes;

                    if (admissionEventContainer == null)
                    {
                        admissionEventContainer = new AdmissionNotesEventContainer();
                    }

                    admissionEventContainer.Add((AdmissionEvent)theEvent);
                    break;
                default:
                    throw new NotImplementedException();
            }

            examination = UpdateCaseStatus(examination);
            return examination;
        }

        private static Examination UpdateCaseStatus(Examination examination)
        {
            return examination;
        }
    }
}
