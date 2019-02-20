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
 
        public Location(string organisationCode, string name, string nationalGrouping, string highLevelHealthGeography, string addressLine1, string addressLine2, string addressLine3, string addressLine4, string addressLine5, string postcode, string openDate, string closeDate, string contactTelephoneNumber, string amendedRecordIndicator, string gORCode)
        {
            OrganisationCode = organisationCode;
            Name = name;
            NationalGrouping = nationalGrouping;
            HighLevelHealthGeography = highLevelHealthGeography;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressLine3 = addressLine3;
            AddressLine4 = addressLine4;
            AddressLine5 = addressLine5;
            Postcode = postcode;
            ContactTelephoneNumber = contactTelephoneNumber;
            AmendedRecordIndicator = amendedRecordIndicator;
            GORCode = gORCode;

        }

        public string OrganisationCode { get; private set; }
        public string Name { get; private set; }
        public string NationalGrouping { get; private set; }
        public string HighLevelHealthGeography { get; private set; }
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string AddressLine3 { get; private set; }
        public string AddressLine4 { get; private set; }
        public string AddressLine5 { get; private set; }
        public string Postcode { get; private set; }
        public string OpenDate { get; private set; }
        public string CloseDate { get; private set; }
        public string ContactTelephoneNumber { get; private set; }
        public string AmendedRecordIndicator { get; private set; }
        public string GORCode { get; private set; }
    }
}
