using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// New Examination Profile.
    /// </summary>
    public class NewExaminationProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of <see cref="NewExaminationProfile"/>.
        /// </summary>
        public NewExaminationProfile()
        {
            CreateMap<PostExaminationRequest, ExaminationItem>()
                .ForMember(p => p.ExaminationId, opt => opt.Ignore())
                .ForMember(p => p.MedicalTeam, opt => opt.Ignore())
                .ForMember(p => p.UrgencyScore, opt => opt.Ignore());
        }
    }
}