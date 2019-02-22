using AutoMapper;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Examinations;

namespace Medical_Examiner_API.Extensions.Data
{
    /// <summary>
    /// Examination Profile for AutoMapper
    /// </summary>
    public class ExaminationProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public ExaminationProfile()
        {
            CreateMap<Examination, GetExaminationResponse>();
            CreateMap<Examination, ExaminationItem>();
        }
    }
}
