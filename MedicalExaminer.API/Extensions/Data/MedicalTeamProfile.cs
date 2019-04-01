using AutoMapper;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// Medical Team Profile.
    /// </summary>
    public class MedicalTeamProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of <see cref="MedicalTeamProfile"/>.
        /// </summary>
        public MedicalTeamProfile()
        {
            CreateMap<PutMedicalTeamRequest, MedicalTeam>();
            CreateMap<MedicalTeam, GetMedicalTeamResponse>();
            CreateMap<MedicalTeam, MedicalTeamItem>();

            CreateMap<MedicalTeam, PutMedicalTeamResponse>();
        }
    }
}