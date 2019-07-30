using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class IsNullOrGuidAttributeTest
    {
        [Fact]
        public void IsNullOrGuid_When_Value_Is_Null()
        {
            // Arrange

            var validationContext = new Mock<IServiceProvider>().Object;
            var sut = new is();
            var expectedResult = ValidationResult.Success;

        }
    }
}
