using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class QapDiscussionPrepopulated
    {
        /// <summary>
        /// Clinical Professional: QAP details from Medical Team
        /// </summary>
        public ClinicalProfessional Qap { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1a
        /// </summary>
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1b
        /// </summary>
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1c
        /// </summary>
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 2
        /// </summary>
        public string CauseOfDeath2 { get; set; }

        /// <summary>
        /// Medical examiners full name
        /// </summary>
        public string MedicalExaminer { get; set; }

        /// <summary>
        /// Medical Examiner Gmc Number
        /// </summary>
        public string MedicalExaminerGmcNumber { get; set; }

        /// <summary>
        /// has the pre-scrutiny occured
        /// </summary>
        public PreScrutinyStatus PreScrutinyStatus { get; set; }

        /// <summary>
        /// date of the latest pre-scrutiny
        /// </summary>
        public DateTime? DateOfLatestPreScrutiny { get; set; }

        /// <summary>
        /// the users full name that added the latest pre scrutiny
        /// </summary>
        public string UserForLatestPrescrutiny { get; set; }

        /// <summary>
        /// GMC Number Of User For Latest Pre-scrutiny
        /// </summary>
        public string GmcNumberOfUserForLatestPrescrutiny { get; set; }

        /// <summary>
        /// Has the qap cause of death been entered.
        /// </summary>
        public StatusBarResult QapCodEntered { get; set; }

        /// <summary>
        /// The name of the QAP.
        /// </summary>
        public string UserForQapOriginalCauseOfDeath { get; set; }

        /// <summary>
        /// Date of the original Cause of death established by the QAP.
        /// </summary>
        public DateTime? DateOfQapOriginalCauseOfDeath { get; set; }

        /// <summary>
        /// Original cause of death established by QAP 1a
        /// </summary>
        public string QapOriginalCauseOfDeath1a { get; set; }

        /// <summary>
        /// Original cause of death established by QAP 1b
        /// </summary>
        public string QapOriginalCauseOfDeath1b { get; set; }

        /// <summary>
        /// Original cause of death established by QAP 1c
        /// </summary>
        public string QapOriginalCauseOfDeath1c { get; set; }

        /// <summary>
        /// Original cause of death established by QAP 2
        /// </summary>
        public string QapOriginalCauseOfDeath2 { get; set; }
    }
}
