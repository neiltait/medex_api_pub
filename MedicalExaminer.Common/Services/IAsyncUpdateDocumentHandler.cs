using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.Common.Services
{
    public interface IAsyncUpdateDocumentHandler
    {
        Task<string> Handle(Document document);
    }
}
