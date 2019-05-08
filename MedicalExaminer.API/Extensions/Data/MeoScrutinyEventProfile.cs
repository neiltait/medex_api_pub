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
                .ForMember(meoSummaryEvent => meoSummaryEvent.EventType, opt => opt.Ignore())
                .ForMember(meoSummaryEvent => meoSummaryEvent.UserId, opt => opt.Ignore())
                .ForMember(meoSummaryEvent => meoSummaryEvent.Created, opt => opt.Ignore())
                .ForMember(meoSummaryEvent => meoSummaryEvent.UserFullName, opt => opt.Ignore())
                .ForMember(meoSummaryEvent => meoSummaryEvent.UsersRole, opt => opt.Ignore());
        }
    }
}
