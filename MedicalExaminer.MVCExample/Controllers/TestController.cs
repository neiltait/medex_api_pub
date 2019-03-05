using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.MVCExample.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        public bool Index()
        {
            return true;
        }
    }
}