using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Loggers
{
    public interface IMELogger
    {
        void Log(string message);
    }
}
