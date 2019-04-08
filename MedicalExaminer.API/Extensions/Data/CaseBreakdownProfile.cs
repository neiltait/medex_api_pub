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
        }
    }
}
