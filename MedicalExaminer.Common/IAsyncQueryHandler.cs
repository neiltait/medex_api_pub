using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MedicalExaminer.Common
{
    public interface IAsyncQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery param);
    }
}
