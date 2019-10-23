using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.ToolBox.Common.Dtos
{
    public class LocationNode
    {
        public string LocationId;
        public string Name;
        public bool IsMeOffice;
        public LocationNode Parent = null;
        public IEnumerable<LocationNode> Children = new List<LocationNode>();

        public IEnumerable<UserItem> Users = new List<UserItem>();
    }
}
