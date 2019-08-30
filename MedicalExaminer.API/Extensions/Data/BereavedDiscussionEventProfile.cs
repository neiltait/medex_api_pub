using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

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
                .ForMember(bereavedDiscussionEvent => bereavedDiscussionEvent.EventType, opt => opt.Ignore())
                .ForMember(bereavedDiscussionEvent => bereavedDiscussionEvent.UserId, opt => opt.Ignore())
                .ForMember(bereavedDiscussionEvent => bereavedDiscussionEvent.Created, opt => opt.Ignore())
                .ForMember(bereavedDiscussionEvent => bereavedDiscussionEvent.UserFullName, opt => opt.Ignore())
                .ForMember(bereavedDiscussionEvent => bereavedDiscussionEvent.UsersRole, opt => opt.Ignore())
                .ForMember(bereavedDiscussionEvent => bereavedDiscussionEvent.BereavedDiscussionOutcome, opt => opt.MapFrom(
                    (src, dest, destMember, context) => src.DiscussionUnableHappen ? BereavedDiscussionOutcome.DiscussionUnableToHappen : src.BereavedDiscussionOutcome));
        }
    }
}