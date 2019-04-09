using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class MeoSummaryEventProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public MeoSummaryEventProfile()
        {
            CreateMap<PutMeoSummaryEventRequest, MeoSummaryEvent>()
                .ForMember(p => p.EventType, opt => opt.Ignore())
                .ForMember(p => p.UserId, opt => opt.Ignore())
                .ForMember(p => p.Created, opt => opt.Ignore());

        }
    }
}
