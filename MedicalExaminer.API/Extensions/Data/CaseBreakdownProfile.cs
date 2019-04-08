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
            CreateMap<CaseBreakDown, CaseBreakDownItem>().ForMember(x => x.OtherEvents, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                var myUser = (MeUser)context.Items["myUser"];
                var usersDraft = source.OtherEvents.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
                var usersDraftItem = Mapper.Map<OtherEventItem>(usersDraft);
                return new EventContainerItem<OtherEventItem>
                {
                    UsersDraft = usersDraftItem,
                    History = source.OtherEvents.History.Select(hist => Mapper.Map<OtherEventItem>(hist)),
                    Latest = Mapper.Map<OtherEventItem>(source.OtherEvents.Latest)
                };
            }))
            .ForMember(x => x.MedicalHistory, ev => ev.MapFrom((source, destination, destinationMember, context) =>
             {
                 var myUser = (MeUser)context.Items["myUser"];
                 var usersDraft = source.MedicalHistory.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
                 var usersDraftItem = Mapper.Map<MedicalHistoryEventItem>(usersDraft);
                 return new EventContainerItem<MedicalHistoryEventItem>
                 {
                     UsersDraft = usersDraftItem,
                     History = source.MedicalHistory.History.Select(hist => Mapper.Map<MedicalHistoryEventItem>(hist)),
                     Latest = Mapper.Map<MedicalHistoryEventItem>(source.MedicalHistory.Latest)
                 };
             }))
            .ForMember(x => x.AdmissionNotes, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                var myUser = (MeUser)context.Items["myUser"];
                var usersDraft = source.AdmissionNotes.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
                var usersDraftItem = Mapper.Map<AdmissionEventItem>(usersDraft);
                return new EventContainerItem<AdmissionEventItem>
                {
                    UsersDraft = usersDraftItem,
                    History = source.AdmissionNotes.History.Select(hist => Mapper.Map<AdmissionEventItem>(hist)),
                    Latest = Mapper.Map<AdmissionEventItem>(source.AdmissionNotes.Latest)
                };
            }))
            .ForMember(x => x.BereavedDiscussion, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                var myUser = (MeUser)context.Items["myUser"];
                var usersDraft = source.BereavedDiscussion.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
                var usersDraftItem = Mapper.Map<BereavedDiscussionEvent>(usersDraft);
                return new EventContainerItem<BereavedDiscussionEvent>
                {
                    UsersDraft = usersDraftItem,
                    History = source.BereavedDiscussion.History.Select(hist => Mapper.Map<BereavedDiscussionEvent>(hist)),
                    Latest = Mapper.Map<BereavedDiscussionEvent>(source.BereavedDiscussion.Latest)
                };
            }))
            .ForMember(x => x.MeoSummary, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                var myUser = (MeUser)context.Items["myUser"];
                var usersDraft = source.MeoSummary.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
                var usersDraftItem = Mapper.Map<MeoSummaryEventItem>(usersDraft);
                return new EventContainerItem<MeoSummaryEventItem>
                {
                    UsersDraft = usersDraftItem,
                    History = source.MeoSummary.History.Select(hist => Mapper.Map<MeoSummaryEventItem>(hist)),
                    Latest = Mapper.Map<MeoSummaryEventItem>(source.MeoSummary.Latest)
                };
            }))
            .ForMember(x => x.PreScrutiny, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                var myUser = (MeUser)context.Items["myUser"];
                var usersDraft = source.PreScrutiny.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
                var usersDraftItem = Mapper.Map<PreScrutinyEventItem>(usersDraft);
                return new EventContainerItem<PreScrutinyEventItem>
                {
                    UsersDraft = usersDraftItem,
                    History = source.PreScrutiny.History.Select(hist => Mapper.Map<PreScrutinyEventItem>(hist)),
                    Latest = Mapper.Map<PreScrutinyEventItem>(source.PreScrutiny.Latest)
                };
            }))
            .ForMember(x => x.QapDiscussion, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                var myUser = (MeUser)context.Items["myUser"];
                var usersDraft = source.QapDiscussion.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
                var usersDraftItem = Mapper.Map<QapDiscussionEventItem>(usersDraft);
                return new EventContainerItem<QapDiscussionEventItem>
                {
                    UsersDraft = usersDraftItem,
                    History = source.QapDiscussion.History.Select(hist => Mapper.Map<QapDiscussionEventItem>(hist)),
                    Latest = Mapper.Map<QapDiscussionEventItem>(source.QapDiscussion.Latest)
                };
            }));
            //ForMember(x => x.OtherEvents, opt => opt.MapFrom(s => Mapper.Map<BaseEventContainter<OtherEvent>,
            //EventContainerItem<OtherEventItem>>(s.OtherEvents, c => c.Items.Add("myUser", null)))).
            //ForMember(x => x.AdmissionNotes, opt => opt.MapFrom(s => Mapper.Map<BaseEventContainter<AdmissionEvent>,
            //EventContainerItem<AdmissionEventItem>>(s.AdmissionNotes))).
            //ForMember(x => x.QapDiscussion, opt => opt.MapFrom(s => Mapper.Map<BaseEventContainter<QapDiscussionEvent>,
            //EventContainerItem<QapDiscussionEventItem>>(s.QapDiscussion))).
            //ForMember(x => x.PreScrutiny, opt => opt.MapFrom(s => Mapper.Map<BaseEventContainter<PreScrutinyEvent>,
            //EventContainerItem<PreScrutinyEventItem>>(s.PreScrutiny))).
            //ForMember(x => x.MeoSummary, opt => opt.MapFrom(s => Mapper.Map<BaseEventContainter<MeoSummaryEvent>,
            //EventContainerItem<MeoSummaryEventItem>>(s.MeoSummary))).
            //ForMember(x => x.MedicalHistory, opt => opt.MapFrom(s => Mapper.Map<BaseEventContainter<MedicalHistoryEvent>,
            //EventContainerItem<MedicalHistoryEventItem>>(s.MedicalHistory))).
            //ForMember(x => x.BereavedDiscussion, opt => opt.MapFrom(s => Mapper.Map<BaseEventContainter<BereavedDiscussionEvent>,
            //EventContainerItem<BereavedDiscussionEventItem>>(s.BereavedDiscussion)));
        }
    }

    

    public class EventContainerProfile : Profile
    {
        public EventContainerProfile()
        {
            CreateMap<BaseEventContainter<OtherEvent>, EventContainerItem<OtherEventItem>>()
                .ForMember(x => x.UsersDraft, eventContainer => eventContainer.MapFrom((source, destination, destinationMember, context) =>
                {
                    var myUser = (MeUser)context.Items["myUser"];
                    var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
                    var result = Mapper.Map<OtherEventItem>(usersDraft);
                    return result;
                }));
            CreateMap<BaseEventContainter<AdmissionEvent>, EventContainerItem<AdmissionEventItem>>()
                .ForMember(x => x.UsersDraft, eventContainer => eventContainer.MapFrom(new AdmissionEventContainerResolver()));
            CreateMap<BaseEventContainter<QapDiscussionEvent>, EventContainerItem<QapDiscussionEventItem>>()
                .ForMember(x => x.UsersDraft, eventContainer => eventContainer.MapFrom(new QapDiscussionEventContainerResolver()));
            CreateMap<BaseEventContainter<MedicalHistoryEvent>, EventContainerItem<MedicalHistoryEventItem>>()
                .ForMember(x => x.UsersDraft, eventContainer => eventContainer.MapFrom(new MedicalHistoryEventContainerResolver()));
            CreateMap<BaseEventContainter<MeoSummaryEvent>, EventContainerItem<MeoSummaryEventItem>>()
                .ForMember(x => x.UsersDraft, eventContainer => eventContainer.MapFrom(new MeoSummaryEventContainerResolver()));
            CreateMap<BaseEventContainter<BereavedDiscussionEvent>, EventContainerItem<BereavedDiscussionEventItem>>()
                .ForMember(x => x.UsersDraft, eventContainer => eventContainer.MapFrom(new BereavedDiscussionEventContainerResolver()));
            CreateMap<BaseEventContainter<PreScrutinyEvent>, EventContainerItem<PreScrutinyEventItem>>()
                .ForMember(x => x.UsersDraft, eventContainer => eventContainer.MapFrom(new PreScrutinyEventContainerResolver()));
        }
    }

    public class CasebreakdownResolver : IValueResolver<CaseBreakDown, CaseBreakDownItem, CaseBreakDownItem>
    {
        public CaseBreakDownItem Resolve(CaseBreakDown source, CaseBreakDownItem destination, CaseBreakDownItem destMember, ResolutionContext context)
        {
            throw new System.NotImplementedException();
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

    public class AdmissionEventContainerResolver : IValueResolver<BaseEventContainter<AdmissionEvent>, EventContainerItem<AdmissionEventItem>, AdmissionEventItem>
    {
        public AdmissionEventItem Resolve(BaseEventContainter<AdmissionEvent> source, EventContainerItem<AdmissionEventItem> destination, AdmissionEventItem destMember, ResolutionContext context)
        {
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var result = Mapper.Map<AdmissionEventItem>(usersDraft);
            return result;
        }

    }

    public class QapDiscussionEventContainerResolver : IValueResolver<BaseEventContainter<QapDiscussionEvent>, EventContainerItem<QapDiscussionEventItem>, QapDiscussionEventItem>
    {
        public QapDiscussionEventItem Resolve(BaseEventContainter<QapDiscussionEvent> source, EventContainerItem<QapDiscussionEventItem> destination, QapDiscussionEventItem destMember, ResolutionContext context)
        {
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var result = Mapper.Map<QapDiscussionEventItem>(usersDraft);
            return result;
        }
    }

    public class MedicalHistoryEventContainerResolver : IValueResolver<BaseEventContainter<MedicalHistoryEvent>, EventContainerItem<MedicalHistoryEventItem>, MedicalHistoryEventItem>
    {
        public MedicalHistoryEventItem Resolve(BaseEventContainter<MedicalHistoryEvent> source, EventContainerItem<MedicalHistoryEventItem> destination, MedicalHistoryEventItem destMember, ResolutionContext context)
        {
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var result = Mapper.Map<MedicalHistoryEventItem>(usersDraft);
            return result;
        }
    }

    public class MeoSummaryEventContainerResolver : IValueResolver<BaseEventContainter<MeoSummaryEvent>, EventContainerItem<MeoSummaryEventItem>, MeoSummaryEventItem>
    {
        public MeoSummaryEventItem Resolve(BaseEventContainter<MeoSummaryEvent> source, EventContainerItem<MeoSummaryEventItem> destination, MeoSummaryEventItem destMember, ResolutionContext context)
        {
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var result = Mapper.Map<MeoSummaryEventItem>(usersDraft);
            return result;
        }
    }

    public class BereavedDiscussionEventContainerResolver : IValueResolver<BaseEventContainter<BereavedDiscussionEvent>, EventContainerItem<BereavedDiscussionEventItem>, BereavedDiscussionEventItem>
    {
        public BereavedDiscussionEventItem Resolve(BaseEventContainter<BereavedDiscussionEvent> source, EventContainerItem<BereavedDiscussionEventItem> destination, BereavedDiscussionEventItem destMember, ResolutionContext context)
        {
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var result = Mapper.Map<BereavedDiscussionEventItem>(usersDraft);
            return result;
        }
    }

    public class PreScrutinyEventContainerResolver : IValueResolver<BaseEventContainter<PreScrutinyEvent>, EventContainerItem<PreScrutinyEventItem>, PreScrutinyEventItem>
    {
        public PreScrutinyEventItem Resolve(BaseEventContainter<PreScrutinyEvent> source, EventContainerItem<PreScrutinyEventItem> destination, PreScrutinyEventItem destMember, ResolutionContext context)
        {
            var myUser = (MeUser)context.Items["myUser"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var result = Mapper.Map<PreScrutinyEventItem>(usersDraft);
            return result;
        }
    }

}
