using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

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
                .ForMember(qapDiscussionEvent => qapDiscussionEvent.EventType, opt => opt.Ignore())
                .ForMember(qapDiscussionEvent => qapDiscussionEvent.UserId, opt => opt.Ignore())
                .ForMember(qapDiscussionEvent => qapDiscussionEvent.Created, opt => opt.Ignore())
                .ForMember(qapDiscussionEvent => qapDiscussionEvent.UserFullName, opt => opt.Ignore())
                .ForMember(qapDiscussionEvent => qapDiscussionEvent.UsersRole, opt => opt.Ignore())
                .ForMember(qapDiscussionEvent => qapDiscussionEvent.QapDiscussionOutcome, opt => opt.MapFrom(
                    (src, dest, destMember, context) => src.DiscussionUnableHappen ? QapDiscussionOutcome.DiscussionUnableToHappen : src.QapDiscussionOutcome));
        }
    }
}
