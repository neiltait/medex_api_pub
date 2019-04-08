using AutoMapper;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// Patient Details Profile.
    /// </summary>
    public class PatientDetailsProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of <see cref="PatientDetailsProfile"/>.
        /// </summary>
        public PatientDetailsProfile()
        {
            CreateMap<PutPatientDetailsRequest, PatientDetails>();
            CreateMap<Examination, GetPatientDetailsResponse>()
                .ForMember(getPatientDetailsResponse => getPatientDetailsResponse.Errors, opt => opt.Ignore());
            CreateMap<PatientDetails, Examination>()
                .ForMember(x => x.ExaminationId, opt => opt.Ignore())
                .ForMember(x => x.UrgencyScore, opt => opt.Ignore())
                .ForMember(x => x.LastAdmission, opt => opt.Ignore())
                .ForMember(x => x.CaseCreated, opt => opt.Ignore())
                .ForMember(x => x.MedicalTeam, opt => opt.Ignore())
                .ForMember(x => x.CaseBreakdown, opt => opt.Ignore())
                .ForMember(x => x.AdmissionNotesHaveBeenAdded, opt => opt.Ignore())
                .ForMember(x => x.ReadyForMEScrutiny, opt => opt.Ignore())
                .ForMember(x => x.Unassigned, opt => opt.Ignore())
                .ForMember(x => x.HaveBeenScrutinisedByME, opt => opt.Ignore())
                .ForMember(x => x.PendingAdmissionNotes, opt => opt.Ignore())
                .ForMember(x => x.PendingDiscussionWithQAP, opt => opt.Ignore())
                .ForMember(x => x.PendingDiscussionWithRepresentative, opt => opt.Ignore())
                .ForMember(x => x.HaveFinalCaseOutstandingOutcomes, opt => opt.Ignore())
                .ForMember(x => x.CaseOfficer, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedAt, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore());
        }
    }
}