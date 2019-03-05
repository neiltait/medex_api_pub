using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Controllers
{
    [Route("values")]
    [ApiController]
    [Authorize]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
