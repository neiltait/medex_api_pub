using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class ValidNhsNumberNullAllowedAttributeTest
    {
        [Fact]
        public void Alphanumeric10DigitsNumberReturnsOk()
        {
            // Arrange
            var nhsNumberString = "12345sa67890";
            var validationContext = new Mock<IServiceProvider>().Object;
            var sut = new ValidNhsNumberNullAllowedAttribute();
            var expectedResult = ValidationResult.Success;

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CorrectNhsNumberReturnsNoErrors()
        {
            // Arrange
            var nhsNumberString = "9434765919";
            var validationContext = new Mock<IServiceProvider>().Object;
            var expectedResult = ValidationResult.Success;
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ExcessDigitsInNumberReturnsErrors()
        {
            // Arrange
            var nhsNumberString = "0123456789101234";
            var validationContext = new Mock<IServiceProvider>().Object;
            var expectedError = nameof(SystemValidationErrors.InvalidNhsNumber);
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedError, result.ErrorMessage);
        }

        [Fact]
        public void NhsNumberWithWhiteSpacesReturnsErrors()
        {
            // Arrange
            var nhsNumberString = "987£654 4321";
            var validationContext = new Mock<IServiceProvider>().Object;
            var expectedError = nameof(SystemValidationErrors.ContainsWhitespace);
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedError, result.ErrorMessage);
        }

        [Fact]
        public void InsufficientDigitsInNumberReturnsErrors()
        {
            // Arrange
            var nhsNumberString = "12345";
            var validationContext = new Mock<IServiceProvider>().Object;

            var expectedError = nameof(SystemValidationErrors.InvalidNhsNumber);
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            //Assert
            Assert.Equal(expectedError, result.ErrorMessage);
        }

        [Fact]
        public void NullNhsNumberReturnsSuccess()
        {
            // Arrange
            string nhsNumberString = null;
            var validationContext = new Mock<IServiceProvider>().Object;

            var expectedResult = ValidationResult.Success;
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}