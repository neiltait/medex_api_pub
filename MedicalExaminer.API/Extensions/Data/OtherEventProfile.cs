using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class OtherEventProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public OtherEventProfile()
        {
            CreateMap<PutOtherEventRequest, OtherEvent>()
                .ForMember(p => p.Created, opt => opt.Ignore())
                .ForMember(p => p.UserId, opt => opt.Ignore());

            CreateMap<OtherEvent, OtherEventItem>();
        }
    }
}