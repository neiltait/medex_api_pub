using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.MVCExample.Services;
using Microsoft.AspNetCore.Authentication;
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
            //string idToken = await HttpContext.GetTokenAsync("id_token");
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            var values = await apiService.GetValues(accessToken);

            return View(values);
        }
    }
}