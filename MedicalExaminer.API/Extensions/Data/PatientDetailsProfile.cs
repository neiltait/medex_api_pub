using AutoMapper;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// Patient Details Profile.
    /// </summary>
    public class PatientDetailsProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of <see cref="PatientDetailsProfile"/>.
        /// </summary>
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