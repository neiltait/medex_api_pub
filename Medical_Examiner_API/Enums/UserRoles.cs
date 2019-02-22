namespace Medical_Examiner_API.Models
{
    public enum UserRoles
    {
        /// <summary>
        /// Medical Examiner Officer
        /// </summary>
        MedicalExaminerOfficer = 0,
        
        /// <summary>
        /// Medical Examiner
        /// </summary>
        MedicalExaminer = 1,
        
        /// <summary>
        /// Service Administrator
        /// </summary>
        ServiceAdministrator = 2,
        
        /// <summary>
        /// Service Owner
        /// </summary>
        ServiceOwner = 3,
        
    }
}