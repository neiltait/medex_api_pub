using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class PatientDetailsProfile : Profile
    {
        public PatientDetailsProfile()
        {
            CreateMap<PutPatientDetailsRequest, PatientDetails>();

            CreateMap<PatientDetails, Examination>()
                .ForMember(x => x.AltLink, opt => opt.Ignore())
                .ForMember(x => x.ETag, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.ResourceId, opt => opt.Ignore())
                .ForMember(x => x.SelfLink, opt => opt.Ignore())
                .ForMember(x => x.Timestamp, opt => opt.Ignore());
        }
    }
}
