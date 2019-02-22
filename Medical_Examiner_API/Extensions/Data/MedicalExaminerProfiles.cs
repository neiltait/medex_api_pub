using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Medical_Examiner_API.Extensions.Data
{
    /// <summary>
    /// Extension method to add our profiles to the mapper configuration.
    /// </summary>
    public static class MedicalExaminerProfiles
    {
        /// <summary>
        /// Add Our Profiles to the mapper configuration.
        /// </summary>
        /// <param name="config">The mapper configuration.</param>
        public static void AddMedicalExaminerProfiles(this IMapperConfigurationExpression config)
        {
            config.AddProfile<ExaminationProfile>();
            config.AddProfile<UsersProfile>();
            config.AddProfile<PermissionsProfile>();
        }
    }
}
