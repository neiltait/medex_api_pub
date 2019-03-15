
namespace MedicalExaminer.Common.Queries.Examination 
{
    
    public class ExaminationMedicalTeamPostQuery : IQuery<Models.IMedicalTeam>
    {
        public Models.IMedicalTeam MedicalTeam { get; }

        public ExaminationMedicalTeamPostQuery(Models.IMedicalTeam medicalTeam)
        {
            MedicalTeam = medicalTeam;
        }
    }
}
