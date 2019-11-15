using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class PreScrutinyEventProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public PreScrutinyEventProfile()
        {
            CreateMap<PutPreScrutinyEventRequest, PreScrutinyEvent>()
                .ForMember(preScrutinyEvent => preScrutinyEvent.EventType, opt => opt.Ignore())
                .ForMember(preScrutinyEvent => preScrutinyEvent.UserId, opt => opt.Ignore())
                .ForMember(preScrutinyEvent => preScrutinyEvent.Created, opt => opt.Ignore())
                .ForMember(preScrutinyEvent => preScrutinyEvent.UserFullName, opt => opt.Ignore())
                .ForMember(preScrutinyEvent => preScrutinyEvent.GmcNumber, opt => opt.Ignore())
                .ForMember(preScrutinyEvent => preScrutinyEvent.UsersRole, opt => opt.Ignore());
        }
    }
}