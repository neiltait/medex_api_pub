using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Medical_Examiner_API.Extensions.Data
{
    public static class MedicalExaminerProfiles
    {
        public static void AddMedicalExaminerProfiles(this IMapperConfigurationExpression config)
        {
            config.AddProfile<ExaminationProfile>();
            config.AddProfile<UsersProfile>();
        }
    }
}
