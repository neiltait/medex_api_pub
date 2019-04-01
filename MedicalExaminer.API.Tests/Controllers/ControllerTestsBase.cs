using System;
using System.Net;
using System.Reflection;
using AutoMapper;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.Common.Loggers;
using Microsoft.Azure.Documents;
using Moq;

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

            Mapper = mapperConfiguration.CreateMapper();

            LoggerMock = new Mock<IMELogger>();
        }

        /// <summary>
        /// Logger Mock.
        /// </summary>
        public Mock<IMELogger> LoggerMock { get; set; }

        /// <summary>
        ///     Real AutoMapper.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        ///     The Controller being tested.
        /// </summary>
        protected T Controller { get; set; }

        /// <summary>
        /// Create Documemnt Client Exception for Testing.
        /// </summary>
        /// <returns>A <see cref="DocumentClientException"/>.</returns>
        protected static DocumentClientException CreateDocumentClientExceptionForTesting()
        {
            var error = new Error
            {
                Id = Guid.NewGuid().ToString(),
                Code = "some_code",
                Message = "some_message"
            };

            var testException = CreateDocumentClientExceptionForTesting(
                error,
                HttpStatusCode.InternalServerError);

            return testException;
        }

        private static DocumentClientException CreateDocumentClientExceptionForTesting(
            Error error, HttpStatusCode httpStatusCode)
        {
            var type = typeof(DocumentClientException);

            var documentClientExceptionInstance = type.Assembly.CreateInstance(
                type.FullName,
                false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new object[] { error, null, httpStatusCode },
                null,
                null);

            return (DocumentClientException)documentClientExceptionInstance;
        }

    }
}