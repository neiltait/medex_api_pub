using System;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Models.Enums;

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
            CreateMap<Examination, CaseOutcome>()
                .ForMember(caseOutcome => caseOutcome.CaseMedicalExaminerFullName, opt => opt.MapFrom(examination => examination.CaseOutcome.CaseMedicalExaminerFullName))
                .ForMember(caseOutcome => caseOutcome.CaseCompleted, opt => opt.MapFrom(examination => examination.CaseCompleted))
                .ForMember(caseOutcome => caseOutcome.CaseOutcomeSummary, opt => opt.MapFrom(examination => examination.CaseOutcome.CaseOutcomeSummary))
                .ForMember(caseOutcome => caseOutcome.OutcomeOfPrescrutiny, opt => opt.MapFrom(examination => examination.CaseOutcome.OutcomeOfPrescrutiny))
                .ForMember(caseOutcome => caseOutcome.OutcomeOfRepresentativeDiscussion, opt => opt.MapFrom(examination => examination.CaseOutcome.OutcomeOfRepresentativeDiscussion))
                .ForMember(caseOutcome => caseOutcome.OutcomeQapDiscussion, opt => opt.MapFrom(examination => examination.CaseOutcome.OutcomeQapDiscussion))
                .ForMember(caseOutcome => caseOutcome.ScrutinyConfirmedOn, opt => opt.MapFrom(examination => examination.CaseOutcome.ScrutinyConfirmedOn))
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
                .ForMember(
                    patientCard => patientCard.AppointmentDate,
                    examination => examination.MapFrom(new AppointmentDateResolver(new AppointmentFinder())))
                .ForMember(
                    patientCard => patientCard.AppointmentTime,
                    examination => examination.MapFrom(new AppointmentTimeResolver(new AppointmentFinder())))
                .ForMember(
                    patientCard => patientCard.CaseCreatedDate,
                    opt => opt.MapFrom(examination => examination.CreatedAt))
                .ForMember(
                    patientCard => patientCard.LastAdmission,
                    opt => opt.MapFrom(new AdmissionDateResolver()));

            CreateMap<Representative, RepresentativeItem>();
            CreateMap<Examination, DeathEvent>()
                .ForMember(deathEvent => deathEvent.Created, opt => opt.Ignore())
                .ForMember(deathEvent => deathEvent.UserId, opt => opt.MapFrom(examination => examination.LastModifiedBy))
                .ForMember(deathEvent => deathEvent.EventId, opt => opt.Ignore())
                .ForMember(deathEvent => deathEvent.UsersRole, opt => opt.Ignore())
                .ForMember(deathEvent => deathEvent.UserFullName, opt => opt.Ignore());
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