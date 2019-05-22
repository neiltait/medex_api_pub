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
                .ForMember(response => response.ScrutinyConfirmedOn, opt => opt.MapFrom(examination => examination.ConfirmationOfScrutinyCompletedAt))
                .ForMember(response => response.Errors, opt => opt.Ignore())
                .ForMember(response => response.Lookups, opt => opt.Ignore());
            CreateMap<PutOutstandingCaseItemsRequest, OutstandingCaseItems>()
                 .ForMember(outstandingCaseItems => outstandingCaseItems.MccdIssued, opt => opt.MapFrom(request => request.MccdIssued))
                 .ForMember(outstandingCaseItems => outstandingCaseItems.CremationFormStatus, opt => opt.MapFrom(request => request.CremationFormStatus))
                 .ForMember(outstandingCaseItems => outstandingCaseItems.GpNotifiedStatus, opt => opt.MapFrom(request => request.GpNotifiedStatus));
        }
    }
}