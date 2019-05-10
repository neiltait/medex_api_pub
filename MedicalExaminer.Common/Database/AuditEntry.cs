using MedicalExaminer.Models;
using System;

namespace MedicalExaminer.Common.Database
{
    public class AuditEntry<T>
    {
        public string id { get; private set; }

        public T Entry { get; private set; }

        public AuditEntry(T item)
        {
            Entry = item;
            id = Guid.NewGuid().ToString();
        }
    }
}
