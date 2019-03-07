using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries
{
    public class LocationRetrivalByIdQuery : IQuery<Location>
    {
        public readonly string Id;

        public LocationRetrivalByIdQuery(string id)
        {
            Id = id;
        }
    }
}
