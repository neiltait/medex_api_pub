using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common
{
    public interface IConnectionSettings
    {
        Uri EndPointUri { get; }
        string PrimaryKey { get; }
        string DatabaseId { get; }
        string Collection { get; }
    }
}
