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
                ForMember(x=>x.AdmissionNotes, opt => opt.MapFrom(s => Mapper.Map<IEventContainer<IEvent>, EventContainerItem>((IEventContainer<IEvent>)s.OtherEvents)));
        }
    }

    public class EventContainerProfile : Profile
    {
        public EventContainerProfile()
        {
            CreateMap<IEventContainer<IEvent>, EventContainerItem>();
               // .ForMember(eci => eci.UsersDraft, x => x.MapFrom(new UserDraftResolver()));
        }
    }

    public class UserDraftResolver : IValueResolver<IEventContainer<IEvent>, EventContainerItem, IEvent>
    {
        public IEvent Resolve(IEventContainer<IEvent> source, EventContainerItem destination, IEvent destMember, ResolutionContext context)
        {
            
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);

            return usersDraft;
        }
    }
}
