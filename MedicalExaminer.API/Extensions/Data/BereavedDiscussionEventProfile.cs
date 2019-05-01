﻿using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Examination Profile for AutoMapper
    /// </summary>
    public class BereavedDiscussionEventProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public BereavedDiscussionEventProfile()
        {
            CreateMap<PutBereavedDiscussionEventRequest, BereavedDiscussionEvent>()
                .ForMember(p => p.EventType, opt => opt.Ignore())
                .ForMember(p => p.UserId, opt => opt.Ignore())
                .ForMember(p => p.Created, opt => opt.Ignore())
                .ForMember(p => p.UserFullName, opt => opt.Ignore())
                .ForMember(p => p.UsersRole, opt => opt.Ignore());
        }
    }
}