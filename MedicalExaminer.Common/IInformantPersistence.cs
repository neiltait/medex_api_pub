using System.Threading.Tasks;

namespace MedicalExaminer.Common
{
    public interface IInformantPersistence
    {
        Task<object> GetInformants(string examinationId);
        Task<object> GetInformant(string examinationId, string informantId);
        Task<object> DeleteInformant(string examinationId, string informantId);
        Task<object> AddInformant(string examinationId);
        Task<object> UpdateInformant(string examinationId, string informantId);
    }
}
