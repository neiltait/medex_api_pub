using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    public class QapDiscussionEventProfile : Profile
    {
        /// <summary>
        ///     Initialise a new instance of the Examination AutoMapper Profile.
        /// </summary>
        public QapDiscussionEventProfile()
        {
            CreateMap<PutQapDiscussionEventRequest, QapDiscussionEvent>()
                .ForMember(p => p.EventType, opt => opt.Ignore())
                .ForMember(p => p.UserId, opt => opt.Ignore());
        }
    }
}
