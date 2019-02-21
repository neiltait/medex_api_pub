using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Examinations;
using Microsoft.Extensions.DependencyInjection;

namespace Medical_Examiner_API.Extensions.Data
{
    /// <summary>
    /// Examination Mapping Extensions.
    /// </summary>
    public class ExaminationProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of the Examination Profile.
        /// </summary>
        public ExaminationProfile()
        {
            CreateMap<Examination, GetExaminationResponse>();
        }
    }
}
