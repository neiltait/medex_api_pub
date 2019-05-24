using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class AdmissionEventProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public AdmissionEventProfile()
        {
            CreateMap<PutAdmissionEventRequest, AdmissionEvent>()
                .ForMember(admissionEvent => admissionEvent.EventType, opt => opt.Ignore())
                .ForMember(admissionEvent => admissionEvent.UserId, opt => opt.Ignore())
                .ForMember(admissionEvent => admissionEvent.Created, opt => opt.Ignore())
                .ForMember(admissionEvent => admissionEvent.UserFullName, opt => opt.Ignore())
                .ForMember(admissionEvent => admissionEvent.UsersRole, opt => opt.Ignore());

            CreateMap<AdmissionEvent, AdmissionEventItem>();
        }
    }
}