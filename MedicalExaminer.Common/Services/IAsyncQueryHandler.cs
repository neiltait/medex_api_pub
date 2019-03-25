using System.Threading.Tasks;
using MedicalExaminer.Common.Queries;

namespace MedicalExaminer.Common.Services
{
    public interface IAsyncQueryHandler<in TQuery, TResult> 
        where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery param);
    }
}