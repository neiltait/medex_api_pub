using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    /// <summary>
    /// Request object that contains properties relevant to medical team items in an examination
    /// </summary>
    public class PostMedicalTeamRequest
    {
        public string ExaminationId { get; set; }
    }
}
