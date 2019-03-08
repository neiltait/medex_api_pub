using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
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
            CreateMap<PostNewCaseRequest, Examination>();
            //CreateMap<IExamination, GetExaminationResponse>();
        }
    }
}
