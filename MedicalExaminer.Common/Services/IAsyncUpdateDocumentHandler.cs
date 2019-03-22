using System.Threading.Tasks;

namespace MedicalExaminer.Common.Services
{
    public interface IAsyncUpdateDocumentHandler
    {
        Task<string> Handle(Models.Examination document);
    }
}
