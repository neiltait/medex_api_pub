using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common.Database
{
    public interface IDocumentClientFactory
    {
        IDocumentClient CreateClient(IConnectionSettings connectionSettings);
    }
}
