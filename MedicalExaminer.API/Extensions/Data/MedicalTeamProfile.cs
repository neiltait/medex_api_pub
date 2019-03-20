using AutoMapper;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class MedicalTeamProfile : Profile
    {
        public MedicalTeamProfile()
        {
            CreateMap<PostMedicalTeamRequest, MedicalTeam>();
            CreateMap<MedicalTeam, GetMedicalTeamResponse>();
            CreateMap<MedicalTeam, MedicalTeamItem>();
        }
    }
}