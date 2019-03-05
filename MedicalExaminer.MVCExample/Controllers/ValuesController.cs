using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.MVCExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.MVCExample.Controllers
{
    public class ValuesController : Controller
    {
        private IAPIService apiService;

        public ValuesController(IAPIService apiService)
        {
            this.apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await apiService.GetValues();
            return View(values);
        }
    }
}