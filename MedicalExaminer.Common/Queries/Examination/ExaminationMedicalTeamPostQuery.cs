using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.Examination
{
    public class ExaminationMedicalTeamPostQuery : IQuery<IMedicalTeam>
    {
        public ExaminationMedicalTeamPostQuery(IMedicalTeam medicalTeam)
        {
            MedicalTeam = medicalTeam;
        }

        private IMedicalTeam MedicalTeam { get; }
    }
}