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
            CreateMap<CaseBreakDown, QapDiscussionPrepopulated>()
                .ForMember(prepopulated => prepopulated.CauseOfDeath1a, cbd => cbd.MapFrom(source => source.PreScrutiny.Latest.CauseOfDeath1a ?? ""))
                .ForMember(prepopulated => prepopulated.CauseOfDeath1b, cbd => cbd.MapFrom(source => source.PreScrutiny.Latest.CauseOfDeath1b ?? ""))
                .ForMember(prepopulated => prepopulated.CauseOfDeath1c, cbd => cbd.MapFrom(source => source.PreScrutiny.Latest.CauseOfDeath1c ?? ""))
                .ForMember(prepopulated => prepopulated.CauseOfDeath2, cbd => cbd.MapFrom(source => source.PreScrutiny.Latest.CauseOfDeath2 ?? ""));
            CreateMap<CaseBreakDown, CaseBreakDownItem>()
            .ForMember(caseBreakDownItem => caseBreakDownItem.PatientDeathEvent, cbd => cbd.MapFrom(caseBreakDown => caseBreakDown.DeathEvent))
            .ForMember(caseBreakDownItem => caseBreakDownItem.CaseClosed, cbd => cbd.MapFrom(caseBreakDown => caseBreakDown.CaseClosedEvent))
            .ForMember(caseBreakDownItem => caseBreakDownItem.OtherEvents, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping<OtherEvent, NullPrepopulated>(source.OtherEvents, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.MedicalHistory, ev => ev.MapFrom((source, destination, destinationMember, context) =>
             {
                 return EventContainerMapping<MedicalHistoryEvent, NullPrepopulated>(source.MedicalHistory, context);
             }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.AdmissionNotes, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping<AdmissionEvent, NullPrepopulated>(source.AdmissionNotes, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.BereavedDiscussion, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping<BereavedDiscussionEvent, NullPrepopulated>(source.BereavedDiscussion, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.MeoSummary, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping<MeoSummaryEvent, NullPrepopulated>(source.MeoSummary, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.PreScrutiny, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping<PreScrutinyEvent, NullPrepopulated>(source.PreScrutiny, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.QapDiscussion, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                var container = EventContainerMapping<QapDiscussionEvent, QapDiscussionPrepopulated>(source.QapDiscussion, context);
                container.Prepopulated = context.Mapper.Map<QapDiscussionPrepopulated>(source);
                return container;
            }));
        }

        private EventContainerItem<T, U> EventContainerMapping<T, U>(
            BaseEventContainter<T> source,
            ResolutionContext context)
            where T : IEvent
        {
            var myUser = (MeUser)context.Items["user"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var usersDraftItem = context.Mapper.Map<T>(usersDraft);
            return new EventContainerItem<T, U>
            {
                UsersDraft = usersDraftItem,
                History = source.History.Select(hist => context.Mapper.Map<T>(hist)),
                Latest = context.Mapper.Map<T>(source.Latest),
            };
        }
    }
}
