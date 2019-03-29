using AutoMapper;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Extensions.Data;

namespace MedicalExaminer.API.Tests.Controllers
{
    /// <summary>
    ///     Base class for controller tests.
    /// </summary>
    /// <typeparam name="T">Type of controller.</typeparam>
    public class ControllerTestsBase<T>
        where T : BaseController
    {
        /// <summary>
        ///     Initialise a new instance of this base class.
        /// </summary>
        public ControllerTestsBase()
        {
            var mapperConfiguration = new MapperConfiguration(config => { config.AddMedicalExaminerProfiles(); });

            // TODO: We should do this at some point but for now it fails with exceptions
            //mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        /// <summary>
        ///     Real AutoMapper.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        ///     The Controller being tested.
        /// </summary>
        protected T Controller { get; set; }
    }
}