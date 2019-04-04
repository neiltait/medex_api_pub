using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using System;
using MedicalExaminer.API.Models.v1.PatientDetails;

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
            CreateMap<Examination, GetExaminationResponse>()
                .ForMember(getExaminationResponse => getExaminationResponse.Errors, opt => opt.Ignore());
            CreateMap<Examination, ExaminationItem>();
            CreateMap<PostExaminationRequest, Examination>()
                .ForMember(examination => examination.UrgencyScore, opt => opt.Ignore())
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
                .ForMember(examination => examination.CaseCreated, opt => opt.Ignore())
                .ForMember(examination => examination.CulturalPriority, opt => opt.Ignore())
                .ForMember(examination => examination.MedicalTeam, opt => opt.Ignore())
                .ForMember(examination => examination.FaithPriority, opt => opt.Ignore())
                .ForMember(examination => examination.ChildPriority, opt => opt.Ignore())
                .ForMember(examination => examination.CoronerPriority, opt => opt.Ignore())
                .ForMember(examination => examination.OtherPriority, opt => opt.Ignore())
                .ForMember(examination => examination.PriorityDetails, opt => opt.Ignore())
                .ForMember(examination => examination.Completed, opt => opt.Ignore())
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
                .ForMember(examination => examination.HaveFinalCaseOutstandingOutcomes, opt => opt.Ignore())
                .ForMember(examination => examination.CaseOfficer, opt => opt.Ignore())
                .ForMember(examination => examination.ExaminationId, opt => opt.Ignore())
                .ForMember(examination => examination.LastModifiedBy, opt => opt.Ignore())
                .ForMember(examination => examination.ModifiedAt, opt => opt.Ignore())
                .ForMember(examination => examination.CreatedAt, opt => opt.Ignore())
                .ForMember(examination => examination.DeletedAt, opt => opt.Ignore());

            CreateMap<Examination, PatientCardItem>()
                .ForMember(patientCard => patientCard.AppointmentDate,
                    examination => examination.MapFrom(new AppointmentDateResolver(new AppointmentFinder())))
                .ForMember(patientCard => patientCard.AppointmentTime,
                    examination => examination.MapFrom(new AppointmentTimeResolver(new AppointmentFinder())));

            CreateMap<Representative, RepresentativeItem>();
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