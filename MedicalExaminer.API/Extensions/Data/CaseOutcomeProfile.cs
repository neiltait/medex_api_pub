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
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public CaseOutcomeProfile()
        {
            CreateMap<PutOutstandingCaseItemsRequest, CaseOutcome>()
                .ForMember(x => x.MCCDIssued, opt => opt.MapFrom(y => y.MCCDIssued))
                .ForMember(x => x.CremationFormStatus, opt => opt.MapFrom(y => y.CremationFormStatus))
                .ForMember(x => x.GPNotifiedStatus, opt => opt.MapFrom(y => y.GPNotifiedStatus));
            //CreateMap<CaseOutcome, Examination>()
            //    .ForMember(x => x.CaseOutcome, opt => opt.MapFrom(y => y));
        }
    }
}