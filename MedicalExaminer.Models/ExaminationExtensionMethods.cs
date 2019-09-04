using System;
using System.Collections.Generic;
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

        public static int GetCaseUrgencySort(this Examination examination)
        {
            return examination.GetCaseUrgencySort(DateTime.Now);
        }

        public static int GetCaseUrgencySort(this Examination examination, DateTime forDateTime)
        {
            if (examination.UrgencySort == null)
            {
                return 0;
            }

            var key = forDateTime.UrgencyKey();
            return examination.UrgencySort.ContainsKey(key)
                ? examination.UrgencySort[key]
                : 0;
        }

        public static bool IsUrgent(this Examination examination)
        {
            var score = CalculateCaseUrgencyScore(examination, DateTime.Now);

            return score > 0;
        }

        public static Examination UpdateCaseUrgencySort(this Examination examination, int numberOfDaysToPreCalculateUrgencySort)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            if (examination.CaseCompleted)
            {
                examination.UrgencySort = new Dictionary<string, int>();
                return examination;
            }

            var now = DateTime.Now;

            var dayList = Enumerable
                .Range(0, numberOfDaysToPreCalculateUrgencySort)
                .Select(days => now.AddDays(days))
                .ToList();

            examination.UrgencySort = dayList
                .ToDictionary(
                    date => date.UrgencyKey(),
                    date => CalculateCaseUrgencySortOrder(examination, date));

            return examination;
        }

        public static Examination UpdateCaseStatus(this Examination examination)
        {
            examination.Unassigned = !(examination.MedicalTeam.MedicalExaminerOfficerUserId != null && examination.MedicalTeam.MedicalExaminerUserId != null);
            examination.PendingAdmissionNotes = CalculateAdmissionNotesPending(examination);
            examination.HaveUnknownBasicDetails = !CalculateBasicDetailsEnteredStatus(examination);
            examination.AdmissionNotesHaveBeenAdded = !examination.PendingAdmissionNotes;
            examination.ReadyForMEScrutiny = CalculateReadyForScrutiny(examination);
            examination.HaveBeenScrutinisedByME = examination.ScrutinyConfirmed;
            examination.PendingAdditionalDetails = !examination.CalculateAdditionalDetailsEnteredStatus();
            examination.PendingDiscussionWithQAP = CalculatePendingQAPDiscussion(examination);
            examination.PendingDiscussionWithRepresentative = CalculatePendingDiscussionWithRepresentative(examination);
            examination.PendingScrutinyNotes = CalculateScrutinyNotesPending(examination);
            examination.HaveFinalCaseOutcomesOutstanding = !CalculateOutstandingCaseOutcomesCompleted(examination);
            examination.CaseOutcome.CaseOutcomeSummary = CalculateScrutinyOutcome(examination);

            return examination;
        }

        public static bool CalculateBasicDetailsEnteredStatus(this Examination examination)
        {
            return examination.GivenNames != null
                   && examination.Surname != null
                   && examination.DateOfBirth != null
                   && examination.DateOfDeath != null
                   && examination.NhsNumber != null;
        }

        public static bool CalculateAdditionalDetailsEnteredStatus(this Examination examination)
        {
            return examination.CaseBreakdown.AdmissionNotes?.Latest != null
                   && examination.MedicalTeam?.ConsultantResponsible?.Name != null
                   && examination.MedicalTeam?.Qap?.Name != null
                   && examination.Representatives?.FirstOrDefault()?.FullName != null
                   && examination.MedicalTeam?.MedicalExaminerUserId != null;
        }

        public static bool CalculateScrutinyCompleteStatus(this Examination examination)
        {
            return examination.CaseBreakdown.PreScrutiny?.Latest != null
                   && examination.CaseBreakdown.QapDiscussion?.Latest != null
                   && examination.CaseBreakdown.BereavedDiscussion?.Latest != null;
        }

        public static bool? CalculateCaseItemsCompleteStatus(this Examination examination)
        {
            if (examination.CaseOutcome.CremationFormStatus == CremationFormStatus.Unknown)
            {
                return null;
            }

            if (examination.CaseOutcome.CremationFormStatus == null
                || examination.CaseOutcome.MccdIssued == null
                || examination.CaseOutcome.GpNotifiedStatus == null
                || examination.CaseCompleted == false
                || (examination.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.ReferToCoroner
                    && !examination.CaseOutcome.CoronerReferralSent))
            {
                return false;
            }

            return true;
        }

        public static bool CalculateOutstandingCaseOutcomesCompleted(this Examination examination)
        {
            if (examination.CaseOutcome.MccdIssued != null && examination.CaseOutcome.MccdIssued.Value)
            {
                if (examination.CaseOutcome.CremationFormStatus == CremationFormStatus.No ||
                    examination.CaseOutcome.CremationFormStatus == CremationFormStatus.Unknown)
                {
                    return true;
                }
                else if (examination.CaseOutcome.GpNotifiedStatus == GPNotified.GPNotified ||
                        examination.CaseOutcome.GpNotifiedStatus == GPNotified.NA)
                {
                    return true;
                }
            }

            if (examination.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.ReferToCoroner)
            {
                return true;
            }

            return false;
        }

        public static bool CalculateCanCompleteScrutiny(this Examination examination)
        {
            if (!examination.ReadyForMEScrutiny)
            {
                return false;
            }

            if (examination.Unassigned)
            {
                return false;
            }

            if (examination.PendingScrutinyNotes)
            {
                return false;
            }

            if (examination.PendingAdmissionNotes)
            {
                return false;
            }

            if (examination.PendingDiscussionWithQAP)
            {
                if (examination.PendingDiscussionWithRepresentative)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CalculateRequiresCoronerReferral(this Examination examination)
        {
            return examination.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.ReferToCoroner ||
                examination.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.IssueMCCDWith100a;
        }

        public static bool CalculatePendingDiscussionWithRepresentative(Examination examination)
        {
            if (examination.ReadyForMEScrutiny)
            {
                return false;
            }
            else
            {
                if (examination.CaseBreakdown.AdmissionNotes.Latest != null &&
                    examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral.Value)
                {
                    return false;
                }
                else
                {
                    var latest = examination.CaseBreakdown.BereavedDiscussion.Latest;

                    if (latest != null && !latest.DiscussionUnableHappen)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static CaseOutcomeSummary? CalculateScrutinyOutcome(this Examination examination)
        {
            if (!examination.Unassigned && examination.CaseBreakdown.AdmissionNotes.Latest?.ImmediateCoronerReferral.Value == true
                && examination.CaseBreakdown.PreScrutiny?.Latest.OutcomeOfPreScrutiny == OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation)
            {
                return CaseOutcomeSummary.ReferToCoroner;
            }

            if (!examination.CalculateCanCompleteScrutiny())
            {
                return null;
            }

            if (examination.CaseBreakdown.BereavedDiscussion?.Latest?.BereavedDiscussionOutcome != null)
            {
                if (examination.CaseBreakdown.BereavedDiscussion?.Latest?.BereavedDiscussionOutcome.Value == BereavedDiscussionOutcome.ConcernsCoronerInvestigation)
                {
                    return CaseOutcomeSummary.ReferToCoroner;
                }

                if (examination.CaseBreakdown.BereavedDiscussion?.Latest?.BereavedDiscussionOutcome.Value == BereavedDiscussionOutcome.ConcernsRequires100a)
                {
                    return CaseOutcomeSummary.IssueMCCDWith100a;
                }
            }

            if (examination.CaseBreakdown.QapDiscussion.Latest != null)
            {
                if (!examination.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                {
                    if (examination.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.ReferToCoronerInvestigation)
                    {
                        return CaseOutcomeSummary.ReferToCoroner;
                    }

                    if (examination.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.ReferToCoronerFor100a)
                    {
                        return CaseOutcomeSummary.IssueMCCDWith100a;
                    }
                }
                else
                {
                    if (examination.CaseBreakdown.PreScrutiny.Latest != null)
                    {
                        if (examination.CaseBreakdown.PreScrutiny.Latest.OutcomeOfPreScrutiny == OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation)
                        {
                            return CaseOutcomeSummary.ReferToCoroner;
                        }
                        if (examination.CaseBreakdown.PreScrutiny.Latest.OutcomeOfPreScrutiny == OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a)
                        {
                            return CaseOutcomeSummary.IssueMCCDWith100a;
                        }
                    }
                }
            }

            return CaseOutcomeSummary.IssueMCCD;
        }

        public static string UrgencyKey(this DateTime dateTime)
        {
            return dateTime
                .Date
                .ToString("yyyy_MM_dd");
        }

        private static int CalculateCaseUrgencySortOrder(Examination examination, DateTime forDate)
        {
            const int defaultScoreMultiplier = 100;

            var daysSinceCreated = (forDate.Date - examination.CreatedAt.Date).Days;
            var sortOrder = Math.Max(0, Math.Min(defaultScoreMultiplier, daysSinceCreated));
            var urgencyScore = CalculateCaseUrgencyScore(examination, forDate);

            sortOrder += urgencyScore * defaultScoreMultiplier;

            return sortOrder;
        }

        private static int CalculateCaseUrgencyScore(Examination examination, DateTime forDate)
        {
            const int defaultScoreWeighting = 100;
            const int overdueScoreWeighting = 1000;

            var score = 0;

            if (examination.ChildPriority)
            {
                score += defaultScoreWeighting;
            }

            if (examination.CoronerPriority)
            {
                score += defaultScoreWeighting;
            }

            if (examination.CulturalPriority)
            {
                score += defaultScoreWeighting;
            }

            if (examination.FaithPriority)
            {
                score += defaultScoreWeighting;
            }

            if (examination.OtherPriority)
            {
                score += defaultScoreWeighting;
            }

            if (forDate.Date.AddDays(-4) > examination.CreatedAt.Date)
            {
                score += overdueScoreWeighting;
            }

            return score;
        }

        private static bool CalculatePendingQAPDiscussion(Examination examination)
        {
            if (examination.ReadyForMEScrutiny)
            {
                return false;
            }
            else
            {
                if (examination.CaseBreakdown.AdmissionNotes.Latest != null &&
                    examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral.Value)
                {
                    return false;
                }
                else
                {
                    var latest = examination.CaseBreakdown.QapDiscussion.Latest;

                    if (latest != null && !latest.DiscussionUnableHappen)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool CalculateAdmissionNotesPending(Examination examination)
        {
            if (examination.CaseBreakdown.AdmissionNotes.Latest != null)
            {
                if (examination.MedicalTeam.ConsultantResponsible != null)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CalculateReadyForScrutiny(this Examination examination)
        {
            if (examination.CaseBreakdown.AdmissionNotes.Latest != null)
            {
                if (examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral.Value)
                {
                    return true;
                }
            }

            if (examination.CaseBreakdown.MeoSummary.Latest != null)
            {
                return true;
            }

            return false;
        }

        private static bool CalculateScrutinyNotesPending(Examination examination)
        {
            if (examination.CaseBreakdown.PreScrutiny.Latest != null)
            {
                return false;
            }

            return true;
        }
    }
}
