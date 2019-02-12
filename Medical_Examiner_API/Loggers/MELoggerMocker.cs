using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Loggers
{
    public class MELoggerMocker : IMELogger
    {
        public string Message { get; private set; }
        public void Log(string message)
        {
            Message = "MELoggerMocker.Log() called";
        }
    }
}
