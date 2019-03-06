using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class Link
    {
        public Link(string rel, string href)
        {
            Relationship = rel;
            Url = href;
        }

        [JsonProperty(PropertyName = "rel")]
        public string Relationship { get; }

        [JsonProperty(PropertyName = "href")]
        public string Url { get; }
    }
}
