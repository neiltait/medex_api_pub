using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class CaseOutcomeProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public CaseOutcomeProfile()
        {
             CreateMap<Examination, PutConfirmationOfScrutinyResponse>()
                .ForMember(x => x.ScrutinyConfirmedOn, opt => opt.MapFrom(x => x.ConfirmationOfScrutinyCompletedAt))
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Lookups, opt => opt.Ignore());
            CreateMap<PutOutstandingCaseItemsRequest, CaseOutcome>()
                 .ForMember(x => x.MCCDIssued, opt => opt.MapFrom(y => y.MCCDIssued))
                 .ForMember(x => x.CremationFormStatus, opt => opt.MapFrom(y => y.CremationFormStatus))
                 .ForMember(x => x.GPNotifiedStatus, opt => opt.MapFrom(y => y.GPNotifiedStatus))
                 .ForMember(x => x.CaseMedicalExaminerFullName, opt => opt.Ignore())
                 .ForMember(x => x.CaseCompleted, opt => opt.Ignore())
                 .ForMember(x => x.CaseOutcomeSummary, opt => opt.Ignore())
                 .ForMember(x => x.OutcomeOfPrescrutiny, opt => opt.Ignore())
                 .ForMember(x => x.OutcomeOfRepresentativeDiscussion, opt => opt.Ignore())
                 .ForMember(x => x.ScrutinyConfirmedOn, opt => opt.Ignore())
                 .ForMember(x => x.OutcomeQapDiscussion, opt => opt.Ignore());
        }
    }
}