using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class ValidNhsNumberNullAllowedAttributeTest
    {
        [Fact]
        public async void AlphanumericDigitsInNumberReturnsErrors()
        {
            // Arrange
            var nhsNumberString = "123ac 45678";
            var validationContext = new Mock<IServiceProvider>().Object;
            var expectedError = "Invalid NHS Number";
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedError, result.ErrorMessage);
        }

        [Fact]
        public async void CorrectNhsNumberReturnsNoErrors()
        {
            // Arrange
            var nhsNumberString = "943 476 5919";
            var validationContext = new Mock<IServiceProvider>().Object;
            var expectedResult = ValidationResult.Success;
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public async void ExcessDigitsInNumberReturnsErrors()
        {
            // Arrange
            var nhsNumberString = "012345678910";
            var validationContext = new Mock<IServiceProvider>().Object;
            var expectedError = "Invalid NHS Number";
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedError, result.ErrorMessage);
        }

        [Fact]
        public async void IncorrectNhsNumberReturnsErrors()
        {
            // Arrange
            var nhsNumberString = "987 654 4321";
            var validationContext = new Mock<IServiceProvider>().Object;
            var expectedError = "Invalid NHS Number";
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            // Assert
            Assert.Equal(expectedError, result.ErrorMessage);
        }

        [Fact]
        public async void InsufficientDigitsInNumberReturnsErrors()
        {
            // Arrange
            var nhsNumberString = "12345";
            var validationContext = new Mock<IServiceProvider>().Object;

            var expectedError = "Invalid NHS Number";
            var sut = new ValidNhsNumberNullAllowedAttribute();

            // Act
            var result = sut.GetValidationResult(nhsNumberString, new ValidationContext(validationContext));

            //Assert
            Assert.Equal(expectedError, result.ErrorMessage);
        }

        [Fact]
        public async void NullNhsNumberReturnsSuccess()
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