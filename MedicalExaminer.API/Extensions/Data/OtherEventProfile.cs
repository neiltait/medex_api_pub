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
                .ForMember(otherEvent => otherEvent.Created, opt => opt.Ignore())
                .ForMember(otherEvent => otherEvent.UserId, opt => opt.Ignore())
                .ForMember(otherEvent => otherEvent.UserFullName, opt => opt.Ignore())
                .ForMember(otherEvent => otherEvent.UsersRole, opt => opt.Ignore());
        }
    }
}