using System;
using System.Linq;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.API.Models.v1.Report;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class ExaminationProfile : Profile
    {
        private static readonly DateTime NoneDate = Convert.ToDateTime("0001 - 01 - 01T00: 00:00");

        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public ExaminationProfile()
        {
            CreateMap<Examination, GetCoronerReferralDownloadResponse>()
                .ForMember(dest => dest.AbleToIssueMCCD, opt => opt.MapFrom(src => src.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.ReferToCoroner ? false : true))
                .ForMember(dest => dest.CauseOfDeath1a, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.CaseBreakdown.QapDiscussion.Latest == null)
                    {
                        if (src.CaseBreakdown.PreScrutiny.Latest == null)
                        {
                            return null;
                        }
                        return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a;
                    }
                    else
                    {
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME ||
                            src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.DiscussionUnableToHappen)
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a;
                        }
                        return src.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1a;
                    }
                }))
                .ForMember(dest => dest.CauseOfDeath1b, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.CaseBreakdown.QapDiscussion.Latest == null)
                    {
                        if (src.CaseBreakdown.PreScrutiny.Latest == null)
                        {
                            return null;
                        }
                        return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b;
                    }
                    else
                    {
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME ||
                            src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.DiscussionUnableToHappen)
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b;
                        }
                        return src.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1b;
                    }
                }))
                .ForMember(dest => dest.CauseOfDeath1c, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.CaseBreakdown.QapDiscussion.Latest == null)
                    {
                        if (src.CaseBreakdown.PreScrutiny.Latest == null)
                        {
                            return null;
                        }
                        return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c;
                    }
                    else
                    {
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME ||
                            src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.DiscussionUnableToHappen)
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c;
                        }
                        return src.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1c;
                    }
                }))
                .ForMember(dest => dest.CauseOfDeath2, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.CaseBreakdown.QapDiscussion.Latest == null)
                    {
                        if (src.CaseBreakdown.PreScrutiny.Latest == null)
                        {
                            return null;
                        }
                        return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2;
                    }
                    else
                    {
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME ||
                            src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.DiscussionUnableToHappen)
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2;
                        }
                        return src.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath2;
                    }
                }))
                .ForMember(dest => dest.PlaceOfDeath, opt => opt.MapFrom(src => src.PlaceDeathOccured))
                .ForMember(dest => dest.LatestBereavedDiscussion, opt => opt.MapFrom(src => src.CaseBreakdown.BereavedDiscussion.Latest))
                .ForMember(dest => dest.Qap, opt => opt.MapFrom(src => new ClinicalProfessional()
                {
                    Name = src.CaseBreakdown.QapDiscussion.Latest.ParticipantName,
                    Organisation = src.CaseBreakdown.QapDiscussion.Latest.ParticipantOrganisation,
                    Notes = src.CaseBreakdown.QapDiscussion.Latest.DiscussionDetails,
                    Phone = src.CaseBreakdown.QapDiscussion.Latest.ParticipantPhoneNumber,
                    Role = src.CaseBreakdown.QapDiscussion.Latest.ParticipantRole
                }))
                .ForMember(dest => dest.Consultant, opt => opt.MapFrom(src => src.MedicalTeam.ConsultantResponsible))
                .ForMember(dest => dest.GP, opt => opt.MapFrom(src => src.MedicalTeam.GeneralPractitioner))
                .ForMember(dest => dest.LatestAdmissionDetails, opt => opt.MapFrom(src => src.CaseBreakdown.AdmissionNotes.Latest))
                .ForMember(dest => dest.DetailsAboutMedicalHistory, opt => opt.MapFrom((src, dest, destMember, context) => {
                    var temp = src.CaseBreakdown.MedicalHistory.History.
                    Select(hstry => hstry?.Text).Aggregate("", (current, next) => current + next + Environment.NewLine);
                    if (temp != null)
                    {
                        temp = temp.TrimEnd(Environment.NewLine.ToCharArray());
                    }

                    return temp;
                }));

            CreateMap<Examination, BereavedDiscussionPrepopulated>()
                .ForMember(dest => dest.Representatives, opt => opt.MapFrom(source => source.Representatives))
                .ForMember(dest => dest.CauseOfDeath1a, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    var shouldUseQap = UseQap(source.CaseBreakdown);
                    if (shouldUseQap == null)
                    {
                        return null;
                    }
                    if (shouldUseQap == true)
                    {
                        if (source.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                        {
                            return null;
                        }
                        return source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1a;
                    }
                    return source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a;
                }))
                .ForMember(dest => dest.CauseOfDeath1b, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    var shouldUseQap = UseQap(source.CaseBreakdown);
                    if (shouldUseQap == null)
                    {
                        return null;
                    }
                    if (shouldUseQap == true)
                    {
                        if (source.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                        {
                            return null;
                        }
                        return source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1b;
                    }
                    return source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b;
                }))
                .ForMember(dest => dest.CauseOfDeath1c, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    var shouldUseQap = UseQap(source.CaseBreakdown);
                    if (shouldUseQap == null)
                    {
                        return null;
                    }
                    if (shouldUseQap == true)
                    {
                        if (source.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                        {
                            return null;
                        }
                        return source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1c;
                    }
                    return source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c;
                }))
                .ForMember(dest => dest.CauseOfDeath2, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    var shouldUseQap = UseQap(source.CaseBreakdown);
                    if (shouldUseQap == null)
                    {
                        return null;
                    }
                    if (shouldUseQap == true)
                    {
                        if (source.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                        {
                            return null;
                        }
                        return source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath2;
                    }
                    return source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2;
                }))
                .ForMember(dest => dest.DateOfLatestPreScrutiny, opt => opt.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest.Created))
                .ForMember(dest => dest.DateOfLatestQAPDiscussion, opt => opt.MapFrom(source => source.CaseBreakdown.QapDiscussion.Latest.DateOfConversation))
                .ForMember(dest => dest.MedicalExaminer, opt => opt.MapFrom(source => source.MedicalTeam.MedicalExaminerFullName))
                .ForMember(dest => dest.PreScrutinyStatus, opt => opt.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest == null ? PreScrutinyStatus.PrescrutinyNotHappened : PreScrutinyStatus.PrescrutinyHappened))
                .ForMember(dest => dest.QAPDiscussionStatus, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    if (source.CaseBreakdown.QapDiscussion.Latest == null)
                    {
                        return QAPDiscussionStatus.NoRecord;
                    }
                    if (source.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                    {
                        return QAPDiscussionStatus.CouldNotHappen;
                    }
                    if (source.CaseBreakdown.PreScrutiny.Latest == null)
                    {
                        return QAPDiscussionStatus.HappenedNoRevision;
                    }
                    if (source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1a == source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a
                && source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1b == source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b
                && source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1c == source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c
                && source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath2 == source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2)
                    {
                        return QAPDiscussionStatus.HappenedNoRevision;
                    }
                    return QAPDiscussionStatus.HappenedWithRevisions;
                }))
                .ForMember(dest => dest.QAPNameForLatestQAPDiscussion, opt => opt.MapFrom(source => source.CaseBreakdown.QapDiscussion.Latest.ParticipantName))
                .ForMember(dest => dest.UserForLatestPrescrutiny, opt => opt.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest.UserFullName))
                .ForMember(dest => dest.UserForLatestQAPDiscussion, opt => opt.MapFrom(source => source.CaseBreakdown.QapDiscussion.Latest.UserFullName));
            CreateMap<Examination, QapDiscussionPrepopulated>()
                .ForMember(prepopulated => prepopulated.Qap, opt => opt.MapFrom(source => source.MedicalTeam.Qap))
                .ForMember(prepopulated => prepopulated.CauseOfDeath1a, cbd => cbd.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a))
                .ForMember(prepopulated => prepopulated.CauseOfDeath1b, cbd => cbd.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b))
                .ForMember(prepopulated => prepopulated.CauseOfDeath1c, cbd => cbd.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c))
                .ForMember(prepopulated => prepopulated.CauseOfDeath2, cbd => cbd.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2))
                .ForMember(prepopulated => prepopulated.MedicalExaminer, cbd => cbd.MapFrom(source => source.MedicalTeam.MedicalExaminerFullName)) 
                .ForMember(prepopulated => prepopulated.DateOfLatestPreScrutiny, cbd => cbd.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest.Created))
                .ForMember(prepopulated => prepopulated.PreScrutinyStatus, cbd => cbd.MapFrom(source =>
                        source.CaseBreakdown.PreScrutiny.Latest == null
                            ? PreScrutinyStatus.PrescrutinyNotHappened
                            : PreScrutinyStatus.PrescrutinyHappened))
                .ForMember(prepopulated => prepopulated.UserForLatestPrescrutiny, cbd => cbd.MapFrom(source => source.CaseBreakdown.PreScrutiny.Latest.UserFullName));

            CreateMap<Examination, CaseBreakDownItem>()
                .ForMember(cbi => cbi.AdmissionNotes, opt => opt.MapFrom((source, destination, destinationMember, context) =>
                {
                    var container = EventContainerMapping<AdmissionEvent, NullPrepopulated>(source.CaseBreakdown.AdmissionNotes, context);
                    container.Prepopulated = context.Mapper.Map<NullPrepopulated>(source);
                    return container;
                }))
                .ForMember(cbi => cbi.BereavedDiscussion, opt => opt.MapFrom((source, destination, destinationMember, context) =>
                {
                    var container = EventContainerMapping<BereavedDiscussionEvent, BereavedDiscussionPrepopulated>(source.CaseBreakdown.BereavedDiscussion, context);
                    container.Prepopulated = context.Mapper.Map<BereavedDiscussionPrepopulated>(source);
                    return container;
                }))
                .ForMember(cbi => cbi.MedicalHistory, opt => opt.MapFrom((source, destination, destinationMember, context) =>
                {
                    var container = EventContainerMapping<MedicalHistoryEvent, NullPrepopulated>(source.CaseBreakdown.MedicalHistory, context);
                    container.Prepopulated = context.Mapper.Map<NullPrepopulated>(source);
                    return container;
                }))
                .ForMember(cbi => cbi.MeoSummary, opt => opt.MapFrom((source, destination, destinationMember, context) =>
                {
                    var container = EventContainerMapping<MeoSummaryEvent, NullPrepopulated>(source.CaseBreakdown.MeoSummary, context);
                    container.Prepopulated = context.Mapper.Map<NullPrepopulated>(source);
                    return container;
                }))
                .ForMember(cbi => cbi.OtherEvents, opt => opt.MapFrom((source, destination, destinationMember, context) =>
                {
                    var container = EventContainerMapping<OtherEvent, NullPrepopulated>(source.CaseBreakdown.OtherEvents, context);
                    container.Prepopulated = context.Mapper.Map<NullPrepopulated>(source);
                    return container;
                }))
                .ForMember(cbi => cbi.PreScrutiny, opt => opt.MapFrom((source, destination, destinationMember, context) =>
                {
                    var container = EventContainerMapping<PreScrutinyEvent, NullPrepopulated>(source.CaseBreakdown.PreScrutiny, context);
                    container.Prepopulated = context.Mapper.Map<NullPrepopulated>(source);
                    return container;
                }))
                .ForMember(cbi => cbi.QapDiscussion, opt => opt.MapFrom((source, destination, destinationMember, context) =>
                {
                    var container = EventContainerMapping<QapDiscussionEvent, QapDiscussionPrepopulated>(source.CaseBreakdown.QapDiscussion, context);
                    container.Prepopulated = context.Mapper.Map<QapDiscussionPrepopulated>(source);
                    return container;
                }))
                .ForMember(cbi => cbi.PatientDeathEvent, opt => opt.MapFrom(src => src.CaseBreakdown.DeathEvent))
                .ForMember(cbi => cbi.CaseClosed, opt => opt.MapFrom(src => src.CaseBreakdown.CaseClosedEvent));
            CreateMap<Examination, CaseOutcome>()
                .ForMember(caseOutcome => caseOutcome.CaseMedicalExaminerFullName, opt => opt.MapFrom(examination => examination.CaseOutcome.CaseMedicalExaminerFullName))
                .ForMember(caseOutcome => caseOutcome.CaseCompleted, opt => opt.MapFrom(examination => examination.CaseCompleted))
                .ForMember(caseOutcome => caseOutcome.CaseOutcomeSummary, opt => opt.MapFrom(examination => examination.CaseOutcome.CaseOutcomeSummary))
                .ForMember(caseOutcome => caseOutcome.OutcomeOfPrescrutiny, opt => opt.MapFrom(examination => examination.CaseOutcome.OutcomeOfPrescrutiny))
                .ForMember(caseOutcome => caseOutcome.OutcomeOfRepresentativeDiscussion, opt => opt.MapFrom(examination => examination.CaseOutcome.OutcomeOfRepresentativeDiscussion))
                .ForMember(caseOutcome => caseOutcome.OutcomeQapDiscussion, opt => opt.MapFrom(examination => examination.CaseOutcome.OutcomeQapDiscussion))
                .ForMember(caseOutcome => caseOutcome.ScrutinyConfirmedOn, opt => opt.MapFrom(examination => examination.CaseOutcome.ScrutinyConfirmedOn))
                .ForMember(caseOutcome => caseOutcome.CoronerReferralSent, opt => opt.MapFrom(examination => examination.CoronerReferralSent))
                .ForMember(caseOutcome => caseOutcome.MccdIssued, opt => opt.MapFrom(examination => examination.CaseOutcome.MccdIssued))
                .ForMember(caseOutcome => caseOutcome.CremationFormStatus, opt => opt.MapFrom(examination => examination.CaseOutcome.CremationFormStatus))
                .ForMember(caseOutcome => caseOutcome.GpNotifiedStatus, opt => opt.MapFrom(examination => examination.CaseOutcome.GpNotifiedStatus));
            CreateMap<Examination, GetCaseOutcomeResponse>()
                .ForMember(response => response.Header, opt => opt.MapFrom(examination => examination))
                .ForMember(response => response.CaseMedicalExaminerFullName, opt => opt.MapFrom(new MedicalExaminerFullNameResolver()))
                .ForMember(response => response.CaseMedicalExaminerId, opt => opt.MapFrom(new MedicalExaminerIdResolver()))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore())
                .ForMember(response => response.CaseCompleted, opt => opt.MapFrom(examination => examination.CaseCompleted))
                .ForMember(response => response.CaseOutcomeSummary, opt => opt.MapFrom(examination => examination.CaseOutcome.CaseOutcomeSummary))
                .ForMember(response => response.OutcomeOfPrescrutiny, opt => opt.MapFrom(new PreScrutinyOutcomeResolver()))
                .ForMember(response => response.OutcomeOfRepresentativeDiscussion, opt => opt.MapFrom(new RepresentativeDiscussionOutcomeResolver()))
                .ForMember(response => response.OutcomeQapDiscussion, opt => opt.MapFrom(new QAPDiscussionOutcomeResolver()))
                .ForMember(response => response.ScrutinyConfirmedOn, opt => opt.MapFrom(examination => examination.CaseOutcome.ScrutinyConfirmedOn))
                .ForMember(response => response.MccdIssued, opt => opt.MapFrom(examination => examination.CaseOutcome.MccdIssued))
                .ForMember(response => response.CremationFormStatus, opt => opt.MapFrom(examination => examination.CaseOutcome.CremationFormStatus))
                .ForMember(response => response.GpNotifiedStatus, opt => opt.MapFrom(examination => examination.CaseOutcome.GpNotifiedStatus));
            CreateMap<Examination, ExaminationItem>()
                .ForMember(response => response.UrgencyScore, opt => opt.MapFrom(examination => examination.IsUrgent() ? 1 : 0));
            CreateMap<Examination, GetPatientDetailsResponse>()
                .ForMember(response => response.Header, opt => opt.MapFrom(examination => examination))
                .ForMember(response => response.UrgencyScore, opt => opt.MapFrom(examination => examination.IsUrgent() ? 1 : 0))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<Examination, PutMedicalTeamResponse>()
                .ForMember(response => response.Header, opt => opt.MapFrom(examination => examination))
                .ForMember(response => response.ConsultantResponsible, opt => opt.MapFrom(examination => examination.MedicalTeam.ConsultantResponsible))
                .ForMember(response => response.ConsultantsOther, opt => opt.MapFrom(examination => examination.MedicalTeam.ConsultantsOther))
                .ForMember(response => response.GeneralPractitioner, opt => opt.MapFrom(examination => examination.MedicalTeam.GeneralPractitioner))
                .ForMember(response => response.MedicalExaminerOfficerUserId, opt => opt.MapFrom(examination => examination.MedicalTeam.MedicalExaminerOfficerUserId))
                .ForMember(response => response.MedicalExaminerUserId, opt => opt.MapFrom(examination => examination.MedicalTeam.MedicalExaminerUserId))
                .ForMember(response => response.NursingTeamInformation, opt => opt.MapFrom(examination => examination.MedicalTeam.NursingTeamInformation))
                .ForMember(response => response.Qap, opt => opt.MapFrom(examination => examination.MedicalTeam.Qap))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<Examination, GetMedicalTeamResponse>()
                .ForMember(response => response.Header, opt => opt.MapFrom(examination => examination))
                .ForMember(response => response.ConsultantResponsible, opt => opt.MapFrom(examination => examination.MedicalTeam.ConsultantResponsible))
                .ForMember(response => response.ConsultantsOther, opt => opt.MapFrom(examination => examination.MedicalTeam.ConsultantsOther))
                .ForMember(response => response.GeneralPractitioner, opt => opt.MapFrom(examination => examination.MedicalTeam.GeneralPractitioner))
                .ForMember(response => response.MedicalExaminerOfficerUserId, opt => opt.MapFrom(examination => examination.MedicalTeam.MedicalExaminerOfficerUserId))
                .ForMember(response => response.MedicalExaminerUserId, opt => opt.MapFrom(examination => examination.MedicalTeam.MedicalExaminerUserId))
                .ForMember(response => response.NursingTeamInformation, opt => opt.MapFrom(examination => examination.MedicalTeam.NursingTeamInformation))
                .ForMember(response => response.Qap, opt => opt.MapFrom(examination => examination.MedicalTeam.Qap))
                .ForMember(response => response.MedicalExaminerFullName, opt => opt.MapFrom(examination => examination.MedicalTeam.MedicalExaminerFullName))
                .ForMember(response => response.MedicalExaminerOfficerFullName, opt => opt.MapFrom(examination => examination.MedicalTeam.MedicalExaminerOfficerFullName))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<Examination, GetCaseBreakdownResponse>()
                .ForMember(response => response.Header, opt => opt.MapFrom(examination => examination))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<PostExaminationRequest, Examination>()
                .ForMember(examination => examination.ExaminationId, opt => opt.Ignore())
                .ForMember(examination => examination.HouseNameNumber, opt => opt.Ignore())
                .ForMember(examination => examination.Street, opt => opt.Ignore())
                .ForMember(examination => examination.Town, opt => opt.Ignore())
                .ForMember(examination => examination.County, opt => opt.Ignore())
                .ForMember(examination => examination.Postcode, opt => opt.Ignore())
                .ForMember(examination => examination.Country, opt => opt.Ignore())
                .ForMember(examination => examination.LastOccupation, opt => opt.Ignore())
                .ForMember(examination => examination.OrganisationCareBeforeDeathLocationId, opt => opt.Ignore())
                .ForMember(examination => examination.ModeOfDisposal, opt => opt.Ignore())
                .ForMember(examination => examination.FuneralDirectors, opt => opt.Ignore())
                .ForMember(examination => examination.AnyPersonalEffects, opt => opt.Ignore())
                .ForMember(examination => examination.PersonalEffectDetails, opt => opt.Ignore())
                .ForMember(examination => examination.LastAdmission, opt => opt.Ignore())
                .ForMember(examination => examination.CulturalPriority, opt => opt.Ignore())
                .ForMember(examination => examination.MedicalTeam, opt => opt.Ignore())
                .ForMember(examination => examination.FaithPriority, opt => opt.Ignore())
                .ForMember(examination => examination.ChildPriority, opt => opt.Ignore())
                .ForMember(examination => examination.CoronerPriority, opt => opt.Ignore())
                .ForMember(examination => examination.OtherPriority, opt => opt.Ignore())
                .ForMember(examination => examination.PriorityDetails, opt => opt.Ignore())
                .ForMember(examination => examination.CaseCompleted, opt => opt.Ignore())
                .ForMember(examination => examination.CoronerStatus, opt => opt.Ignore())
                .ForMember(examination => examination.AnyImplants, opt => opt.Ignore())
                .ForMember(examination => examination.ImplantDetails, opt => opt.Ignore())
                .ForMember(examination => examination.Representatives, opt => opt.Ignore())
                .ForMember(examination => examination.HaveUnknownBasicDetails, opt => opt.Ignore())
                .ForMember(examination => examination.AdmissionNotesHaveBeenAdded, opt => opt.Ignore())
                .ForMember(examination => examination.ReadyForMEScrutiny, opt => opt.Ignore())
                .ForMember(examination => examination.Unassigned, opt => opt.Ignore())
                .ForMember(examination => examination.HaveBeenScrutinisedByME, opt => opt.Ignore())
                .ForMember(examination => examination.PendingAdditionalDetails, opt => opt.Ignore())
                .ForMember(examination => examination.PendingAdmissionNotes, opt => opt.Ignore())
                .ForMember(examination => examination.PendingDiscussionWithQAP, opt => opt.Ignore())
                .ForMember(examination => examination.PendingDiscussionWithRepresentative, opt => opt.Ignore())
                .ForMember(examination => examination.PendingScrutinyNotes, opt => opt.Ignore())
                .ForMember(examination => examination.HaveFinalCaseOutcomesOutstanding, opt => opt.Ignore())
                .ForMember(examination => examination.ExaminationId, opt => opt.Ignore())
                .ForMember(examination => examination.LastModifiedBy, opt => opt.Ignore())
                .ForMember(examination => examination.ModifiedAt, opt => opt.Ignore())
                .ForMember(examination => examination.CreatedAt, opt => opt.Ignore())
                .ForMember(examination => examination.DeletedAt, opt => opt.Ignore())
                .ForMember(examination => examination.CaseBreakdown, opt => opt.Ignore())
                .ForMember(examination => examination.MedicalTeam, opt => opt.Ignore())
                .ForMember(examination => examination.MedicalExaminerOfficeResponsibleName, opt => opt.Ignore())
                .ForMember(examination => examination.UrgencySort, opt => opt.Ignore())
                .ForMember(examination => examination.NationalLocationId, opt => opt.Ignore())
                .ForMember(examination => examination.RegionLocationId, opt => opt.Ignore())
                .ForMember(examination => examination.TrustLocationId, opt => opt.Ignore())
                .ForMember(examination => examination.SiteLocationId, opt => opt.Ignore())
                .ForMember(examination => examination.ConfirmationOfScrutinyCompletedAt, opt => opt.Ignore())
                .ForMember(examination => examination.ConfirmationOfScrutinyCompletedBy, opt => opt.Ignore())
                .ForMember(examination => examination.CoronerReferralSent, opt => opt.Ignore())
                .ForMember(examination => examination.ScrutinyConfirmed, opt => opt.Ignore())
                .ForMember(examination => examination.OutstandingCaseItemsCompleted, opt => opt.Ignore())
                .ForMember(examination => examination.CaseOutcome, opt => opt.Ignore())
                .ForMember(examination => examination.CreatedBy, opt => opt.Ignore());
            CreateMap<Examination, PatientCardItem>()
                .ForMember(response => response.UrgencyScore, opt => opt.MapFrom(examination => examination.IsUrgent() ? 1 : 0))
                .ForMember(patientCard => patientCard.AppointmentDate, examination => examination.MapFrom(new AppointmentDateResolver(new AppointmentFinder())))
                .ForMember(patientCard => patientCard.AppointmentTime, examination => examination.MapFrom(new AppointmentTimeResolver(new AppointmentFinder())))
                .ForMember(patientCard => patientCard.CaseCreatedDate, opt => opt.MapFrom(examination => examination.CreatedAt))
                .ForMember(patientCard => patientCard.LastAdmission, opt => opt.MapFrom(new AdmissionDateResolver()))
                .ForMember(patientCard => patientCard.CaseOutcome, opt => opt.MapFrom(examination => examination.CaseOutcome.CaseOutcomeSummary))
                .ForMember(patientCard => patientCard.NameEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (string.IsNullOrEmpty(source.GivenNames) && string.IsNullOrEmpty(source.Surname))
                        {
                            return StatusBarResult.Unknown;
                        }

                        return StatusBarResult.Complete;
                    }))
                .ForMember(patientCard => patientCard.DobEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.DateOfBirth == NoneDate)
                        {
                            return StatusBarResult.Unknown;
                        }

                        if (source.DateOfBirth == null)
                        {
                            return StatusBarResult.Incomplete;
                        }

                        return StatusBarResult.Complete;
                    }))
                .ForMember(patientCard => patientCard.DodEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.DateOfDeath == NoneDate)
                        {
                            return StatusBarResult.Unknown;
                        }

                        if (source.DateOfDeath == null)
                        {
                            return StatusBarResult.Incomplete;
                        }

                        return StatusBarResult.Complete;
                    }))
                .ForMember(patientCard => patientCard.NhsNumberEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => string.IsNullOrEmpty(source.NhsNumber) ? StatusBarResult.Unknown : StatusBarResult.Complete))
                .ForMember(patientCard => patientCard.BasicDetailsEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CalculateBasicDetailsEnteredStatus()))
                .ForMember(patientCard => patientCard.LatestAdmissionDetailsEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseBreakdown.AdmissionNotes.Latest != null ? StatusBarResult.Complete : StatusBarResult.Incomplete))
                .ForMember(patientCard => patientCard.DoctorInChargeEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>source.MedicalTeam.ConsultantResponsible?.Name != null ? StatusBarResult.Complete : StatusBarResult.Incomplete))
                .ForMember(patientCard => patientCard.QapEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.MedicalTeam.Qap?.Name != null ? StatusBarResult.Complete : StatusBarResult.Incomplete))
                .ForMember(patientCard => patientCard.BereavedInfoEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.Representatives?.FirstOrDefault()?.FullName != null ? StatusBarResult.Complete : StatusBarResult.Incomplete))
                .ForMember(patientCard => patientCard.MeAssigned, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.MedicalTeam.MedicalExaminerUserId != null ? StatusBarResult.Complete : StatusBarResult.Incomplete))
                .ForMember(patientCard => patientCard.AdditionalDetailsEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CalculateAdditionalDetailsEnteredStatus()))
                .ForMember(patientCard => patientCard.PreScrutinyEventEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseBreakdown.PreScrutiny.Latest != null ? StatusBarResult.Complete : StatusBarResult.Incomplete))
                .ForMember(patientCard => patientCard.QapDiscussionEventEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.CaseBreakdown.QapDiscussion.Latest != null)
                        {
                            if (source.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                            {
                                return StatusBarResult.NotApplicable;
                            }

                            return StatusBarResult.Complete;
                        }

                        return StatusBarResult.Incomplete;
                    }))
                .ForMember(patientCard => patientCard.BereavedDiscussionEventEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.CaseBreakdown.BereavedDiscussion.Latest != null)
                        {
                            if (source.CaseBreakdown.BereavedDiscussion.Latest.DiscussionUnableHappen)
                            {
                                return StatusBarResult.NotApplicable;
                            }

                            return StatusBarResult.Complete;
                        }

                        return StatusBarResult.Incomplete;
                    }))
                .ForMember(patientCard => patientCard.MeScrutinyConfirmed, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.ScrutinyConfirmed ? StatusBarResult.Complete : StatusBarResult.Incomplete))
                .ForMember(patientCard => patientCard.IsScrutinyCompleted, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CalculateScrutinyCompleteStatus()))
                .ForMember(patientCard => patientCard.MccdIssued, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.CaseOutcome.CaseOutcomeSummary != CaseOutcomeSummary.ReferToCoroner)
                        {
                            if (source.CaseOutcome.MccdIssued != null)
                            {
                                return StatusBarResult.Complete;
                            }

                            return StatusBarResult.Incomplete;
                        }

                        return StatusBarResult.NotApplicable;
                    }))
                .ForMember(patientCard => patientCard.CremationFormInfoEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.CaseOutcome.CaseOutcomeSummary != CaseOutcomeSummary.ReferToCoroner)
                        {
                            switch (source.CaseOutcome.CremationFormStatus)
                            {
                                case CremationFormStatus.Yes:
                                case CremationFormStatus.No:
                                    return StatusBarResult.Complete;
                                case CremationFormStatus.Unknown:
                                    return StatusBarResult.Unknown;
                                case null:
                                    return StatusBarResult.Incomplete;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        return StatusBarResult.NotApplicable;
                    }))
                .ForMember(patientCard => patientCard.GpNotified, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.CaseOutcome.CaseOutcomeSummary != CaseOutcomeSummary.ReferToCoroner)
                        {
                            if (source.CaseOutcome.GpNotifiedStatus != null)
                            {
                                return StatusBarResult.Complete;
                            }

                            return StatusBarResult.Incomplete;
                        }

                        return StatusBarResult.NotApplicable;
                    }))
                .ForMember(patientCard => patientCard.SentToCoroner, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.ReferToCoroner
                            || source.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.IssueMCCDWith100a)
                        {
                            if (source.CaseOutcome.CoronerReferralSent)
                            {
                                return StatusBarResult.Complete;
                            }

                            return StatusBarResult.Incomplete;
                        }

                        return StatusBarResult.NotApplicable;
                    }))
                .ForMember(patientCard => patientCard.CaseClosed, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        if (source.CaseCompleted)
                        {
                            return StatusBarResult.Complete;
                        }
                        return StatusBarResult.Incomplete;
                    }))
                .ForMember(patientCard => patientCard.IsCaseItemsCompleted, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CalculateCaseItemsCompleteStatus()));

            CreateMap<Representative, RepresentativeItem>();
            CreateMap<Examination, DeathEvent>()
                .ForMember(deathEvent => deathEvent.Created, opt => opt.Ignore())
                .ForMember(deathEvent => deathEvent.UserId, opt => opt.MapFrom(examination => examination.LastModifiedBy))
                .ForMember(deathEvent => deathEvent.EventId, opt => opt.Ignore())
                .ForMember(deathEvent => deathEvent.UsersRole, opt => opt.Ignore())
                .ForMember(deathEvent => deathEvent.UserFullName, opt => opt.Ignore());
        }

        private bool? UseQap(CaseBreakDown caseBreakdown)
        {
            if (caseBreakdown.PreScrutiny.Latest == null && caseBreakdown.QapDiscussion.Latest == null)
            {
                return null;
            }

            if (caseBreakdown.QapDiscussion.Latest != null)
            {
                if (caseBreakdown.PreScrutiny.Latest == null)
                {
                    return true;
                }

                if (caseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME || caseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                {
                    return false;
                }
            }

            if (caseBreakdown.QapDiscussion.Latest == null && caseBreakdown.PreScrutiny.Latest != null)
            {
                return false;
            }

            return caseBreakdown.QapDiscussion.Latest?.CauseOfDeath1a != caseBreakdown.PreScrutiny.Latest?.CauseOfDeath1a
                   || caseBreakdown.QapDiscussion.Latest?.CauseOfDeath1b != caseBreakdown.PreScrutiny.Latest?.CauseOfDeath1b
                   || caseBreakdown.QapDiscussion.Latest?.CauseOfDeath1c != caseBreakdown.PreScrutiny.Latest?.CauseOfDeath1c
                   || caseBreakdown.QapDiscussion.Latest?.CauseOfDeath2 != caseBreakdown.PreScrutiny.Latest?.CauseOfDeath2;
        }

        private EventContainerItem<T, U> EventContainerMapping<T, U>(
            BaseEventContainter<T> source,
            ResolutionContext context)
            where T : IEvent
        {
            var myUser = (MeUser)context.Items["user"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var usersDraftItem = context.Mapper.Map<T>(usersDraft);
            return new EventContainerItem<T, U>
            {
                UsersDraft = usersDraftItem,
                History = source.History.Select(hist => context.Mapper.Map<T>(hist)),
                Latest = context.Mapper.Map<T>(source.Latest),
            };
        }
    }

    internal class QAPDiscussionOutcomeResolver : IValueResolver<Examination, GetCaseOutcomeResponse, QapDiscussionOutcome?>
    {
        public QapDiscussionOutcome? Resolve(Examination source, GetCaseOutcomeResponse destination, QapDiscussionOutcome? destMember, ResolutionContext context)
        {
            return source.CaseBreakdown.QapDiscussion?.Latest?.QapDiscussionOutcome;
        }
    }

    internal class RepresentativeDiscussionOutcomeResolver : IValueResolver<Examination, GetCaseOutcomeResponse, BereavedDiscussionOutcome?>
    {
        public BereavedDiscussionOutcome? Resolve(Examination source, GetCaseOutcomeResponse destination, BereavedDiscussionOutcome? destMember, ResolutionContext context)
        {
            return source.CaseBreakdown.BereavedDiscussion?.Latest?.BereavedDiscussionOutcome;
        }
    }

    internal class PreScrutinyOutcomeResolver : IValueResolver<Examination, GetCaseOutcomeResponse, OverallOutcomeOfPreScrutiny?>
    {
        public OverallOutcomeOfPreScrutiny? Resolve(Examination source, GetCaseOutcomeResponse destination, OverallOutcomeOfPreScrutiny? destMember, ResolutionContext context)
        {
            return source.CaseBreakdown.PreScrutiny?.Latest?.OutcomeOfPreScrutiny;
        }
    }

    internal class AdmissionDateResolver : IValueResolver<Examination, PatientCardItem, DateTime?>
    {
        public DateTime? Resolve(Examination source, PatientCardItem destination, DateTime? destMember, ResolutionContext context)
        {
            return source.CaseBreakdown.AdmissionNotes?.Latest?.AdmittedDate;
        }
    }

    internal class MedicalExaminerFullNameResolver : IValueResolver<Examination, GetCaseOutcomeResponse, string>
    {
        public string Resolve(Examination source, GetCaseOutcomeResponse destination, string destMember, ResolutionContext context)
        {
            return source.MedicalTeam.MedicalExaminerFullName;
        }
    }

    internal class MedicalExaminerIdResolver : IValueResolver<Examination, GetCaseOutcomeResponse, string>
    {
        public string Resolve(Examination source, GetCaseOutcomeResponse destination, string destMember, ResolutionContext context)
        {
            return source.MedicalTeam.MedicalExaminerUserId;
        }
    }

    internal class AppointmentDateResolver : IValueResolver<Examination, PatientCardItem, DateTime?>
    {
        private readonly AppointmentFinder _appointmentFinder;

        public AppointmentDateResolver(AppointmentFinder appointmentFinder)
        {
            _appointmentFinder = appointmentFinder;
        }

        public DateTime? Resolve(Examination source, PatientCardItem destination, DateTime? destMember, ResolutionContext context)
        {
            return _appointmentFinder.FindAppointment(source.Representatives)?.AppointmentDate;
        }
    }

    internal class AppointmentTimeResolver : IValueResolver<Examination, PatientCardItem, TimeSpan?>
    {
        private AppointmentFinder _appointmentFinder;

        public AppointmentTimeResolver(AppointmentFinder appointmentFinder)
        {
            _appointmentFinder = appointmentFinder;
        }

        public TimeSpan? Resolve(Examination source, PatientCardItem destination, TimeSpan? destMember, ResolutionContext context)
        {
            return _appointmentFinder.FindAppointment(source.Representatives)?.AppointmentTime;
        }
    }
}