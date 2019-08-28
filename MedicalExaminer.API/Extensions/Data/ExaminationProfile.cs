using System;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Models.Enums;
using System.Linq;
using MedicalExaminer.API.Models.v1.Report;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class ExaminationProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public ExaminationProfile()
        {
            CreateMap<Examination, GetCoronerReferralDownloadResponse>()
                .ForMember(dest => dest.AbleToIssueMCCD, opt => opt.MapFrom(src => src.CaseOutcome.OutcomeOfPrescrutiny == OverallOutcomeOfPreScrutiny.ReferToCoronerInvestigation ? false : true))
                .ForMember(dest => dest.CauseOfDeath1a, opt => opt.MapFrom((src, dest, destMember, context) => {
                    if (src.CaseBreakdown.QapDiscussion.Latest == null)
                    {
                        if (src.CaseBreakdown.PreScrutiny.Latest == null)
                        {
                            return null;
                        }
                        else
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a;
                        }
                    }
                    else
                    {
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP ||
                        src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME)
                        {
                            return src.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1a;
                        }
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME)
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a;
                        }
                        return "Referred to Coroner";
                    }
                }))
                .ForMember(dest => dest.CauseOfDeath1b, opt => opt.MapFrom((src, dest, destMember, context) => {
                    if (src.CaseBreakdown.QapDiscussion.Latest == null)
                    {
                        if (src.CaseBreakdown.PreScrutiny.Latest == null)
                        {
                            return null;
                        }
                        else
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b;
                        }
                    }
                    else
                    {
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP ||
                        src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME)
                        {
                            return src.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1b;
                        }
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME)
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b;
                        }
                        return "Referred to Coroner";
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
                        else
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c;
                        }
                    }
                    else
                    {
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP ||
                        src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME)
                        {
                            return src.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1c;
                        }
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME)
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c;
                        }
                        return "Referred to Coroner";
                    }
                }))
                .ForMember(dest => dest.CauseOfDeath2, opt => opt.MapFrom((src, dest, destMember, context) => {
                    if (src.CaseBreakdown.QapDiscussion.Latest == null)
                    {
                        if (src.CaseBreakdown.PreScrutiny.Latest == null)
                        {
                            return null;
                        }
                        else
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2;
                        }
                    }
                    else
                    {
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByQAP ||
                        src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathAgreedByQAPandME)
                        {
                            return src.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath2;
                        }
                        if (src.CaseBreakdown.QapDiscussion.Latest.QapDiscussionOutcome == QapDiscussionOutcome.MccdCauseOfDeathProvidedByME)
                        {
                            return src.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2;
                        }
                        return "Referred to Coroner";
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
                    var shouldUseRor = UsePreScrutiny(source.CaseBreakdown);
                    if (shouldUseRor == null)
                    {
                        return null;
                    }
                    if (shouldUseRor == true)
                    {
                        return source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1a;
                    }
                    return source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1a;
                }))
                .ForMember(dest => dest.CauseOfDeath1b, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    var shouldUseRor = UsePreScrutiny(source.CaseBreakdown);
                    if (shouldUseRor == null)
                    {
                        return null;
                    }
                    if (shouldUseRor == true)
                    {
                        return source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1b;
                    }
                    return source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1b;
                }))
                .ForMember(dest => dest.CauseOfDeath1c, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    var shouldUseRor = UsePreScrutiny(source.CaseBreakdown);
                    if (shouldUseRor == null)
                    {
                        return null;
                    }
                    if (shouldUseRor == true)
                    {
                        return source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath1c;
                    }
                    return source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath1c;
                }))
                .ForMember(dest => dest.CauseOfDeath2, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    var shouldUseRor = UsePreScrutiny(source.CaseBreakdown);
                    if (shouldUseRor == null)
                    {
                        return null;
                    }
                    if (shouldUseRor == true)
                    {
                        return source.CaseBreakdown.PreScrutiny.Latest.CauseOfDeath2;
                    }
                    return source.CaseBreakdown.QapDiscussion.Latest.CauseOfDeath2;
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
            CreateMap<Examination, ExaminationItem>();
            CreateMap<Examination, GetPatientDetailsResponse>()
                .ForMember(response => response.Header, opt => opt.MapFrom(examination => examination))
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
                .ForMember(examination => examination.UrgencyScore, opt => opt.Ignore())
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
                .ForMember(patientCard => patientCard.AppointmentDate,
                    examination => examination.MapFrom(new AppointmentDateResolver(new AppointmentFinder())))
                .ForMember(patientCard => patientCard.AppointmentTime, examination => examination.MapFrom(new AppointmentTimeResolver(new AppointmentFinder())))
                .ForMember(patientCard => patientCard.CaseCreatedDate, opt => opt.MapFrom(examination => examination.CreatedAt))
                .ForMember(patientCard => patientCard.LastAdmission, opt => opt.MapFrom(new AdmissionDateResolver()))
                .ForMember(patientCard => patientCard.CaseOutcome, opt => opt.MapFrom(examination => examination.CaseOutcome.CaseOutcomeSummary))
                .ForMember(patientCard => patientCard.NameEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.GivenNames != null && source.Surname != null))
                .ForMember(patientCard => patientCard.DobEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.DateOfBirth != null))
                .ForMember(patientCard => patientCard.DodEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.DateOfDeath != null))
                .ForMember(patientCard => patientCard.NhsNumberEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.NhsNumber != null))
                .ForMember(patientCard => patientCard.BasicDetailsEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CalculateBasicDetailsEnteredStatus()))
                .ForMember(patientCard => patientCard.LatestAdmissionDetailsEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseBreakdown.AdmissionNotes.Latest != null))
                .ForMember(patientCard => patientCard.DoctorInChargeEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.MedicalTeam.ConsultantResponsible?.Name != null))
                .ForMember(patientCard => patientCard.QapEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.MedicalTeam.Qap?.Name != null))
                .ForMember(patientCard => patientCard.BereavedInfoEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.Representatives?.FirstOrDefault()?.FullName != null))
                .ForMember(patientCard => patientCard.MeAssigned, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.MedicalTeam.MedicalExaminerUserId != null))
                .ForMember(patientCard => patientCard.AdditionalDetailsEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CalculateAdditionalDetailsEnteredStatus()))
                .ForMember(patientCard => patientCard.PreScrutinyEventEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseBreakdown.PreScrutiny.Latest != null))
                .ForMember(patientCard => patientCard.QapDiscussionEventEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseBreakdown.QapDiscussion.Latest != null))
                .ForMember(patientCard => patientCard.BereavedDiscussionEventEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseBreakdown.BereavedDiscussion.Latest != null))
                .ForMember(patientCard => patientCard.IsScrutinyCompleted, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CalculateScrutinyCompleteStatus()))
                .ForMember(patientCard => patientCard.MccdIssued, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseOutcome.MccdIssued != null))
                .ForMember(patientCard => patientCard.CremationFormInfoEntered, opt => opt.MapFrom(
                    (source, dest, destMember, context) =>
                    {
                        switch (source.CaseOutcome.CremationFormStatus)
                        {
                            case CremationFormStatus.No:
                            case CremationFormStatus.Yes:
                                return true;
                            case CremationFormStatus.Unknown:
                                return (bool?)null;
                            case null:
                                return false;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }))
                .ForMember(patientCard => patientCard.GpNotified, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseOutcome.GpNotifiedStatus != null))
                .ForMember(patientCard => patientCard.SentToCoroner, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseOutcome.CoronerReferralSent))
                .ForMember(patientCard => patientCard.CaseClosed, opt => opt.MapFrom(
                    (source, dest, destMember, context) => source.CaseCompleted))
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

        private bool? UsePreScrutiny(CaseBreakDown caseBreakdown)
        {
            if (caseBreakdown.PreScrutiny.Latest == null && caseBreakdown.QapDiscussion.Latest == null)
            {
                return null;
            }

            if (caseBreakdown.QapDiscussion.Latest != null && caseBreakdown.PreScrutiny.Latest == null)
            {
                return null;
            }

            if (caseBreakdown.QapDiscussion.Latest == null && caseBreakdown.PreScrutiny.Latest != null)
            {
                return true;
            }

            return caseBreakdown.QapDiscussion.Latest.CauseOfDeath1a != caseBreakdown.PreScrutiny.Latest.CauseOfDeath1a
                && caseBreakdown.QapDiscussion.Latest.CauseOfDeath1b != caseBreakdown.PreScrutiny.Latest.CauseOfDeath1b
                && caseBreakdown.QapDiscussion.Latest.CauseOfDeath1c != caseBreakdown.PreScrutiny.Latest.CauseOfDeath1c
                && caseBreakdown.QapDiscussion.Latest.CauseOfDeath2 != caseBreakdown.PreScrutiny.Latest.CauseOfDeath2;
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