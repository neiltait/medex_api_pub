using System.Threading.Tasks;

namespace MedicalExaminer.Common.Services
{
    public interface IAsyncUpdateDocumentHandler
    {
        Task<Models.Examination> Handle(Models.Examination document);
    }
}