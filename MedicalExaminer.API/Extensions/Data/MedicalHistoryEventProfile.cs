﻿using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class MedicalHistoryEventProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public MedicalHistoryEventProfile()
        {
            CreateMap<PutMedicalHistoryEventRequest, MedicalHistoryEvent>()
                .ForMember(p => p.EventType, opt => opt.Ignore())
                .ForMember(p => p.UserId, opt => opt.Ignore());
        }
    }
}