using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Seeders
{
    public class Location
    {
        public Location(string organisationCode, string name) 
        {
            OrganisationCode = organisationCode;
            Name = name;
        }

        public string OrganisationCode { get; private set; }

        public string Name { get; private set; }
    }
}
