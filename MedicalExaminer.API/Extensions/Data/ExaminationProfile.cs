using System;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.API.Models.v1.MedicalTeams;

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
                .ForMember(x => x.CaseMedicalExaminerFullName, opt => opt.MapFrom(y => y.CaseOutcome.CaseMedicalExaminerFullName))
                .ForMember(x => x.CaseOpen, opt => opt.MapFrom(y => y.CaseOutcome.CaseOpen))
                .ForMember(x => x.CaseOutcomeSummary, opt => opt.MapFrom(y => y.CaseOutcome.CaseOutcomeSummary))
                .ForMember(x => x.OutcomeOfPrescrutiny, opt => opt.MapFrom(y => y.CaseOutcome.OutcomeOfPrescrutiny))
                .ForMember(x => x.OutcomeOfRepresentativeDiscussion, opt => opt.MapFrom(y => y.CaseOutcome.OutcomeOfRepresentativeDiscussion))
                .ForMember(x => x.OutcomeQapDiscussion, opt => opt.MapFrom(y => y.CaseOutcome.OutcomeQapDiscussion))
                .ForMember(x => x.ScrutinyConfirmedOn, opt => opt.MapFrom(y => y.CaseOutcome.ScrutinyConfirmedOn))
                .ForMember(x => x.CremationFormStatus, opt => opt.MapFrom(y => y.CaseOutcome.CremationFormStatus))
                .ForMember(x => x.MCCDIssued, opt => opt.MapFrom(y => y.CaseOutcome.MCCDIssued))
                .ForMember(x => x.GPNotifiedStatus, opt => opt.MapFrom(y => y.CaseOutcome.GPNotifiedStatus));
            CreateMap<Examination, GetCaseOutcomeResponse>()
                .ForMember(x => x.Header, opt => opt.MapFrom(y => y))
                .ForMember(x => x.CaseMedicalExaminerFullName, opt => opt.MapFrom(x => x.MedicalExaminerOfficeResponsibleName))
                .ForMember(x => x.MCCDIssued, opt => opt.MapFrom(y => y.CaseOutcome.MCCDIssued))
                .ForMember(x => x.CremationFormStatus, opt => opt.MapFrom(y => y.CaseOutcome.CremationFormStatus))
                .ForMember(x => x.GPNotifedStatus, opt => opt.MapFrom(y => y.CaseOutcome.GPNotifiedStatus))
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore())
                .ForMember(x => x.CaseOpen, opt => opt.MapFrom(y => y.CaseOutcome.CaseOpen))
                .ForMember(x => x.CaseOutcomeSummary, opt => opt.MapFrom(y => y.CaseOutcome.CaseOutcomeSummary))
                .ForMember(x => x.OutcomeOfPrescrutiny, opt => opt.MapFrom(y => y.CaseOutcome.OutcomeOfPrescrutiny))
                .ForMember(x => x.OutcomeOfRepresentativeDiscussion, opt => opt.MapFrom(y => y.CaseOutcome.OutcomeOfRepresentativeDiscussion))
                .ForMember(x => x.OutcomeQapDiscussion, opt => opt.MapFrom(y => y.CaseOutcome.OutcomeQapDiscussion))
                .ForMember(x => x.ScrutinyConfirmedOn, opt => opt.MapFrom(y => y.ConfirmationOfScrutinyCompletedAt));
            CreateMap<Examination, ExaminationItem>();
            CreateMap<Examination, GetPatientDetailsResponse>()
                .ForMember(x => x.Header, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<Examination, PutMedicalTeamResponse>()
                .ForMember(x => x.Header, opt => opt.MapFrom(y => y))
                .ForMember(x => x.ConsultantResponsible, opt => opt.MapFrom(x => x.MedicalTeam.ConsultantResponsible))
                .ForMember(x => x.ConsultantsOther, opt => opt.MapFrom(x => x.MedicalTeam.ConsultantsOther))
                .ForMember(x => x.GeneralPractitioner, opt => opt.MapFrom(x => x.MedicalTeam.GeneralPractitioner))
                .ForMember(x => x.MedicalExaminerOfficerUserId, opt => opt.MapFrom(x => x.MedicalTeam.MedicalExaminerOfficerUserId))
                .ForMember(x => x.MedicalExaminerUserId, opt => opt.MapFrom(x => x.MedicalTeam.MedicalExaminerUserId))
                .ForMember(x => x.NursingTeamInformation, opt => opt.MapFrom(x => x.MedicalTeam.NursingTeamInformation))
                .ForMember(x => x.Qap, opt => opt.MapFrom(x => x.MedicalTeam.Qap))
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<Examination, GetMedicalTeamResponse>()
                .ForMember(x => x.Header, opt => opt.MapFrom(y => y))
                .ForMember(x => x.ConsultantResponsible, opt => opt.MapFrom(x => x.MedicalTeam.ConsultantResponsible))
                .ForMember(x => x.ConsultantsOther, opt => opt.MapFrom(x => x.MedicalTeam.ConsultantsOther))
                .ForMember(x => x.GeneralPractitioner, opt => opt.MapFrom(x => x.MedicalTeam.GeneralPractitioner))
                .ForMember(x => x.MedicalExaminerOfficerUserId, opt => opt.MapFrom(x => x.MedicalTeam.MedicalExaminerOfficerUserId))
                .ForMember(x => x.MedicalExaminerUserId, opt => opt.MapFrom(x => x.MedicalTeam.MedicalExaminerUserId))
                .ForMember(x => x.NursingTeamInformation, opt => opt.MapFrom(x => x.MedicalTeam.NursingTeamInformation))
                .ForMember(x => x.Qap, opt => opt.MapFrom(x => x.MedicalTeam.Qap))
                .ForMember(x => x.MedicalExaminerFullName, opt => opt.MapFrom(x => x.MedicalTeam.MedicalExaminerFullName))
                .ForMember(x => x.MedicalExaminerOfficerFullName, opt => opt.MapFrom(x => x.MedicalTeam.MedicalExaminerOfficerFullName))
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<Examination, GetCaseBreakdownResponse>()
                .ForMember(x => x.Header, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
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
                .ForMember(patientCard => patientCard.AppointmentTime,
                    examination => examination.MapFrom(new AppointmentTimeResolver(new AppointmentFinder())))
                    .ForMember(patientCard => patientCard.CaseCreatedDate, opt => opt.MapFrom(examination => examination.CreatedAt));

            CreateMap<Representative, RepresentativeItem>();
            CreateMap<Examination, DeathEvent>()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.UserId, opt => opt.MapFrom(y => y.LastModifiedBy));
        }
    }

    /// <summary>
    /// Appointment Date Resolver.
    /// </summary>
    public class AppointmentDateResolver : IValueResolver<Examination, PatientCardItem, DateTime?>
    {
        private readonly AppointmentFinder _appointmentFinder;

        /// <summary>
        /// Initialise a new instance of <see cref="AppointmentDateResolver"/>.
        /// </summary>
        /// <param name="appointmentFinder">Appointment Finder.</param>
        public AppointmentDateResolver(AppointmentFinder appointmentFinder)
        {
            _appointmentFinder = appointmentFinder;
        }

        /// <inheritdoc/>
        public DateTime? Resolve(Examination source, PatientCardItem destination, DateTime? destMember, ResolutionContext context)
        {
            return _appointmentFinder.FindAppointment(source.Representatives)?.AppointmentDate;
        }
    }

    /// <summary>
    /// Appointment Time Resolver.
    /// </summary>
    public class AppointmentTimeResolver : IValueResolver<Examination, PatientCardItem, TimeSpan?>
    {
        private AppointmentFinder _appointmentFinder;

        /// <summary>
        /// Initialise a new instance of <see cref="AppointmentTimeResolver"/>.
        /// </summary>
        /// <param name="appointmentFinder">Appointment Filder.</param>
        public AppointmentTimeResolver(AppointmentFinder appointmentFinder)
        {
            _appointmentFinder = appointmentFinder;
        }

        /// <inheritdoc/>
        public TimeSpan? Resolve(Examination source, PatientCardItem destination, TimeSpan? destMember, ResolutionContext context)
        {
            return _appointmentFinder.FindAppointment(source.Representatives)?.AppointmentTime;
        }
    }
}