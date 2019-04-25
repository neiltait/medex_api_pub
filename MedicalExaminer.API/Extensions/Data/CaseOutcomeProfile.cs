using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// Case Outcome Profile.
    /// </summary>
    public class CaseOutcomeProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CaseOutcomeProfile"/>.
        /// </summary>
        public CaseOutcomeProfile()
        {
            CreateMap<Examination, PutConfirmationOfScrutinyResponse>()
                .ForMember(x => x.ScrutinyConfirmedOn, opt => opt.MapFrom(x => x.ConfirmationOfScrutinyCompletedAt))
                .ForMember(x => x.Errors, opt => opt.Ignore());
        }
    }
}