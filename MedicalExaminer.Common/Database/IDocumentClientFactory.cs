using Cosmonaut;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.Common.Database
{
    public interface IDocumentClientFactory
    {
        IDocumentClient CreateClient(IClientSettings connectionSettings, bool bypassSsl);
    }
}
