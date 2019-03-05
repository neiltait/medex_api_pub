using System.Threading.Tasks;

namespace MedicalExaminer.MVCExample.Services
{
    public interface ITokenService
    {
        Task<string> GetToken();
    }
}
