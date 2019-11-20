using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.ToolBox.Common.Dtos
{
    public class LocationResponse
    {
        public IEnumerable<LocationNode> Roots;

        public IEnumerable<LocationNode> Locations;
        public IEnumerable<UserItem> Users;
    }
}
