using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.ToolBox.Common.Dtos.Generate;
using MedicalExaminer.ToolBox.Common.Services;
using MedicalExaminer.ToolBox.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.ToolBox.MVC.Controllers
{
    public class GenerateController : Controller
    {
        private GenerateConfigurationService _generateConfigurationService;

        public GenerateController(GenerateConfigurationService generateConfigurationService)
        {
            _generateConfigurationService = generateConfigurationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Configure()
        {
            return View(new GenerateViewModel());

        }

        [HttpPost]
        public async Task<ActionResult<GenerateResultViewModel>> Configure(GenerateViewModel viewModel)
        {
            await _generateConfigurationService.Generate(new GenerateConfiguration()
            {
                NumberOfRegions = viewModel.NumberOfRegions,
                NumberOfTrusts = viewModel.NumberOfTrusts,
                NumberOfSites = viewModel.NumberOfSites,

                NumberOfMedicalExaminersPerSite = viewModel.NumberOfMedicalExaminersPerSite,
                NumberOfMedicalExaminerOfficersPerSite = viewModel.NumberOfMedicalExaminerOfficersPerSite,

                NumberOfMedicalExaminersPerTrust = viewModel.NumberOfMedicalExaminersPerTrust,
                NumberOfMedicalExaminerOfficersPerTrust = viewModel.NumberOfMedicalExaminerOfficersPerTrust,

                NumberOfMedicalExaminersPerRegion = viewModel.NumberOfMedicalExaminersPerRegion,
                NumberOfMedicalExaminerOfficersPerRegion = viewModel.NumberOfMedicalExaminerOfficersPerRegion,
                NumberOfServiceAdministratorsPerRegion = viewModel.NumberOfServiceAdministratorsPerRegion,

                NumberOfServiceOwnersPerNational = viewModel.NumberOfServiceOwnersPerNational,
            });

            return  View("ConfigurePost", new GenerateResultViewModel()
            {
                Success = false,
            });
        }
    }
}