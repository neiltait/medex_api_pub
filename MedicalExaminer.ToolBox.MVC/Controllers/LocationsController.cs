using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.ToolBox.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.ToolBox.MVC.Controllers
{
    public class LocationsController : Controller
    {
        private GetLocationTreeService _getLocationTreeService;

        public LocationsController(GetLocationTreeService locationTreeService)
        {
            _getLocationTreeService = locationTreeService;
        }

        public IActionResult Index()
        {
            var result = _getLocationTreeService.GetTree();

            return View(result);
        }
    }
}