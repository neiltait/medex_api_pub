using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class MedicalHistoryEventProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public MedicalHistoryEventProfile()
        {
            CreateMap<PutMedicalHistoryEventRequest, MedicalHistoryEvent>()
                .ForMember(medicalHistoryEvent => medicalHistoryEvent.EventType, opt => opt.Ignore())
                .ForMember(medicalHistoryEvent => medicalHistoryEvent.UserId, opt => opt.Ignore())
                .ForMember(medicalHistoryEvent => medicalHistoryEvent.Created, opt => opt.Ignore())
                .ForMember(medicalHistoryEvent => medicalHistoryEvent.UserFullName, opt => opt.Ignore())
                .ForMember(medicalHistoryEvent => medicalHistoryEvent.UsersRole, opt => opt.Ignore());
        }
    }
}