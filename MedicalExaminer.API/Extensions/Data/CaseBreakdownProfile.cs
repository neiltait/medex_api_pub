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
            CreateMap<CaseBreakDown, CaseBreakDownItem>().
                ForMember(x => x.OtherEvents, opt => opt.MapFrom(s => Mapper.Map<BaseEventContainter<OtherEvent>,
                EventContainerItem<OtherEvent>>(s.OtherEvents)));
        }
    }

    public class EventContainerProfile : Profile
    {
        public EventContainerProfile()
        {
            CreateMap<BaseEventContainter<OtherEvent>, EventContainerItem<OtherEventItem>>()
                .ForMember(x => x.UsersDraft, eventContainer => eventContainer.MapFrom(new OtherEventContainerResolver()));
            CreateMap<IEventContainer<AdmissionEvent>, EventContainerItem<AdmissionEvent>>();
            CreateMap<IEventContainer<QapDiscussionEvent>, EventContainerItem<QapDiscussionEvent>>();
            CreateMap<IEventContainer<MedicalHistoryEvent>, EventContainerItem<MedicalHistoryEvent>>();
            CreateMap<IEventContainer<MeoSummaryEvent>, EventContainerItem<MeoSummaryEvent>>();
            CreateMap<IEventContainer<BereavedDiscussionEvent>, EventContainerItem<BereavedDiscussionEvent>>();
            CreateMap<IEventContainer<PreScrutinyEvent>, EventContainerItem<PreScrutinyEvent>>();
        }
    }

    public class OtherEventContainerResolver : IValueResolver<BaseEventContainter<OtherEvent>, EventContainerItem<OtherEventItem>, OtherEventItem>
    {
        public OtherEventItem Resolve(BaseEventContainter<OtherEvent> source, EventContainerItem<OtherEventItem> destination, OtherEventItem destMember, ResolutionContext context)
        {
                    var myUser = (MeUser)context.Items["myUser"];
                    var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var result = Mapper.Map<OtherEventItem>(usersDraft);
                    return result;
        }
    }
    
}
