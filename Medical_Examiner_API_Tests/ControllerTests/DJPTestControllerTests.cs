
using FluentAssertions;
using ME_API_tests.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Medical_Examiners_API.Controllers;
using Medical_Examiner_API.Controllers;
using Medical_Examiner_API.Loggers;
using System.Threading.Tasks;
using Xunit;

namespace Medical_Examiner_API_Tests.ControllerTests
{
    public class DJPTestControllerTests
    {
        MELoggerMocker _mockLogger;
        DJPTestController _controller;

        public DJPTestControllerTests()
        {
            _mockLogger = new MELoggerMocker();
            _controller = new DJPTestController(_mockLogger); 
        }

        [Fact]
        public void CheckCallToLogger()
        {
            _controller.Get();

            var message = _mockLogger.Message;

            var djp = 1;
        }
    }
}
