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
            .ForMember(x=>x.PatientDeathEvent, cbd => cbd.MapFrom(x=>x.DeathEvent))
            .ForMember(x => x.OtherEvents, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.OtherEvents, context);
            }))
            .ForMember(x => x.MedicalHistory, ev => ev.MapFrom((source, destination, destinationMember, context) =>
             {
                 return EventContainerMapping(source.MedicalHistory, context);
             }))
            .ForMember(x => x.AdmissionNotes, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.AdmissionNotes, context);
            }))
            .ForMember(x => x.BereavedDiscussion, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.BereavedDiscussion, context);
            }))
            .ForMember(x => x.MeoSummary, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.MeoSummary, context);
            }))
            .ForMember(x => x.PreScrutiny, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.PreScrutiny, context);
            }))
            .ForMember(x => x.QapDiscussion, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.QapDiscussion, context);
            }));
        }

        private EventContainerItem<T> EventContainerMapping<T>(
            BaseEventContainter<T> source,
            ResolutionContext context)
            where T : IEvent
        {
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var usersDraftItem = context.Mapper.Map<T>(usersDraft);
            return new EventContainerItem<T>
            {
                UsersDraft = usersDraftItem,
                History = source.History.Select(hist => context.Mapper.Map<T>(hist)),
                Latest = context.Mapper.Map<T>(source.Latest)
            };
        }
    }
}
