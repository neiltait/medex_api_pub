using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Extensions.Data;

namespace Medical_Examiner_API_Tests.Controllers
{
    /// <summary>
    /// Base class for controller tests.
    /// </summary>
    /// <typeparam name="T">Type of controller.</typeparam>
    public class ControllerTestsBase<T>
        where T : BaseController
    {
        /// <summary>
        /// Initialise a new instance of this base class.
        /// </summary>
        public ControllerTestsBase()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddMedicalExaminerProfiles();
            });

            Mapper = mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Real AutoMapper.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// The Controller being tested.
        /// </summary>
        protected T Controller { get; set; }
    }
}
