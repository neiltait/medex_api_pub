using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;
using System.Linq;

namespace MedicalExaminer.API.Extensions.Data
{
    public class CaseBreakdownProfile : Profile
    {
        public CaseBreakdownProfile()
        {
            CreateMap<CaseBreakDown, CaseBreakDownItem>()
                .ForMember(x=>x.OtherEvents, examination => examination.MapFrom(new UserDraftResolver()));
        }
    }

    public class UserDraftResolver : IValueResolver<CaseBreakDown, CaseBreakDownItem, EventContainerItem>
    {
        public EventContainerItem Resolve(CaseBreakDown source, CaseBreakDownItem destination, EventContainerItem destMember, ResolutionContext context)
        {
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.OtherEvents.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);

            var eventContainer = new EventContainerItem()
            {
                History = source.OtherEvents.History,
                Latest = source.OtherEvents.Latest,
                UsersDraft = usersDraft
            };
            
            return eventContainer;
        }
    }
}
