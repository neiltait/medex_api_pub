using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Database
{
    public class AuditEntry<T>
    {
        public string id { get; set; }

        public T Entry { get; set; }

        public AuditEntry(T item)
        {
            Entry = item;
        }
    }
}
