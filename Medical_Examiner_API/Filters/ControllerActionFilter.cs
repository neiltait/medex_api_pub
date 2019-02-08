using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Medical_Examiner_API
{
    public class ControllerActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do nothing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var toDo = 1;
        }
    }
}
