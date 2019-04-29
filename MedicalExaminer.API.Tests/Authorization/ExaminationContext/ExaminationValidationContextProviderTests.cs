using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MedicalExaminer.API.Authorization.ExaminationContext;
using Xunit;

namespace MedicalExaminer.API.Tests.Authorization.ExaminationContext
{
    public class ExaminationValidationContextProviderTests
    {
        [Fact]
        public void Current_ThrowsException_WhenContextNull()
        {
            // Arrange
            var sut = new ExaminationValidationContextProvider();

            // Act
            Func<ExaminationValidationContext> act = () => sut.Current;

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Current_ReturnsContext_WhenNotNull()
        {
            // Arrange
            var expectedContext = new ExaminationValidationContext(null);
            var sut = new ExaminationValidationContextProvider();
            sut.Set(expectedContext);

            // Act
            var result = sut.Current;

            // Assert
            result.Should().Be(expectedContext);
        }

        [Fact]
        public void Set_ThrowsException_WhenContextAlreadySet()
        {
            // Arrange
            var expectedContext = new ExaminationValidationContext(null);
            var sut = new ExaminationValidationContextProvider();
            sut.Set(expectedContext);

            // Act
            Action act = () => sut.Set(expectedContext);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
