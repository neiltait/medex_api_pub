using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Medical_Examiner_API
{
    public class ControllerActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var toDo = 1;
            var djpFile = new StreamWriter(@"C:\\Temp\djp.txt");
            djpFile.WriteLine("OnActionExecuted called");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var toDo = 1;
            var djpFile = new StreamWriter(@"C:\\Temp\djp.txt");
            djpFile.WriteLine("OnActionExecuting called");
        }
    }
}
