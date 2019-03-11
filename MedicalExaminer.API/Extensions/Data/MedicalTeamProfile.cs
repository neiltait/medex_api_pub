using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class MedicalTeamProfile : Profile
    {
        public MedicalTeamProfile()
        {
            CreateMap<PostMedicalTeamRequest, MedicalTeam>();
            CreateMap<MedicalTeam, GetMedicalTeamResponse>();
            //CreateMap<IExamination, GetExaminationResponse>();
        }
    }
}
