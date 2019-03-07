using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class NewExaminationProfile : Profile
    {
        public NewExaminationProfile()
        {
            CreateMap<PostNewCaseRequest, ExaminationItem>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
