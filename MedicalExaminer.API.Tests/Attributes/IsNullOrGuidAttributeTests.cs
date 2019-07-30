using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using MedicalExaminer.API.Attributes;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class IsNullOrGuidAttributeTests
    {
        [Fact]
        public void IsNullOrGuid_When_Value_Is_Null()
        {
            // Arrange
            var validationContext = new Mock<IServiceProvider>().Object;
            var sut = new IsNullOrGuid();
            var expectedResult = ValidationResult.Success;

            // Act
            var result = sut.GetValidationResult(null, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsNullOrGuid_When_Value_Is_GUID()
        {
            // Arrange
            var value = new Guid();
            var validationContext = new Mock<IServiceProvider>().Object;
            var sut = new IsNullOrGuid();
            var expectedResult = ValidationResult.Success;

            // Act
            var result = sut.GetValidationResult(value, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsNullOrGuid_When_Value_Is_Not_GUID_Or_Null()
        {
            // Arrange
            var value = "test";
            var validationContext = new Mock<IServiceProvider>().Object;
            var sut = new IsNullOrGuid();

            // Act
            var result = sut.GetValidationResult(value, new ValidationContext(validationContext));

            // Assert
            result.Should().NotBe(ValidationResult.Success);
        }
    }
}