using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common
{
    public class DatabaseResult<T>
    {
        public T Result { get; }
        public bool IsFourOhFour { get; }
        public DatabaseResult(T result, bool isFourOhFour)
        {
            Result = result;
            this.IsFourOhFour = isFourOhFour;
        }
    }
}
