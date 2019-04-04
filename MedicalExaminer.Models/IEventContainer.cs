using System.Collections.Generic;

namespace MedicalExaminer.Models
{
    public interface IEventContainer<out TEvent>
        where TEvent : IEvent
    {
        IEvent Latest { get; }

        IList<IEvent> Drafts { get; set; }

        IList<IEvent> History { get; set; }
    }    
}
