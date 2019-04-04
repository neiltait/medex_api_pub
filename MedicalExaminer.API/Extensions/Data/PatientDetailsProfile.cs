using AutoMapper;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class PatientDetailsProfile : Profile
    {
        public PatientDetailsProfile()
        {
            CreateMap<PutPatientDetailsRequest, PatientDetails>();
            CreateMap<Examination, GetPatientDetailsResponse>()
                .ForMember(getPatientDetailsResponse => getPatientDetailsResponse.Errors, opt => opt.Ignore());
            CreateMap<PatientDetails, Examination>()
                .ForMember(x => x.ExaminationId, opt => opt.Ignore());
        }
    }
}