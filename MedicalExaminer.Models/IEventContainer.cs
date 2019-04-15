using System.Collections;
using System.Collections.Generic;

namespace MedicalExaminer.Models
{
    public interface IEventContainer<TEvent>
        where TEvent : IEvent
    {
        TEvent Latest { get; }

        IList<TEvent> Drafts { get; set; }

        IList<TEvent> History { get; set; }
    }    
}
