using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class RequiredIfAttributesMatchAttributeTests
    {
        [Fact]
        public void PredicateFinalAndRequiredFieldPopulated_Returns_True()
        {
            // Arrange
            var dto = new TestDtoTdo
            {
                Status = true,
                TestField = null
            };
            var serviceProvider = new Mock<IServiceProvider>().Object;
            serviceProvider.GetService(dto.GetType());
            var validationContext = new ValidationContext(dto, serviceProvider, new Dictionary<object, object>());
            var sut = new RequiredIfAttributesMatch(nameof(TestDtoTdo.Status), true);

            // Act
            var result = sut.GetValidationResult(dto.TestField, validationContext);

            // Assert
            Assert.Equal("The TestDtoTdo field is required.", result?.ErrorMessage);
        }

        [Fact]
        public void PredicateTrueAndRequiredFieldPopulated_Returns_True()
        {
            // Arrange
            var dto = new TestDto
            {
                PredicateProperty = true,
                TestField = "bob"
            };
            var serviceProvider = new Mock<IServiceProvider>().Object;
            serviceProvider.GetService(dto.GetType());
            var validationContext = new ValidationContext(dto, serviceProvider, new Dictionary<object, object>());
            var sut = new RequiredIfAttributesMatch(nameof(TestDto.PredicateProperty), true);
            var expectedResult = ValidationResult.Success;

            // Act
            var result = sut.GetValidationResult(dto.TestField, validationContext);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void PredicateTrueAndRequiredFieldNull_Returns_False()
        {
            // Arrange
            var dto = new TestDto
            {
                PredicateProperty = true,
                TestField = null
            };
            var serviceProvider = new Mock<IServiceProvider>();
            var sut = new RequiredIfAttributesMatch(nameof(TestDto.PredicateProperty), true);
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>())).Returns(dto);

            // Act
            var result = sut.GetValidationResult(
                dto.TestField,
                new ValidationContext(dto, serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal("The TestDto field is required.", result?.ErrorMessage);
        }

        [Fact]
        public void PredicateFalseAndRequiredFieldPopulated_Returns_True()
        {
            // Arrange
            var dto = new TestDto
            {
                PredicateProperty = false,
                TestField = "something"
            };
            var serviceProvider = new Mock<IServiceProvider>();
            var sut = new RequiredIfAttributesMatch(nameof(TestDto.PredicateProperty), true);
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>())).Returns(dto);

            // Act
            var result = sut.GetValidationResult(dto.TestField,
                new ValidationContext(dto, serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void PredicateFalseAndRequiredFieldNull_Returns_True()
        {
            // Arrange
            var dto = new TestDto
            {
                PredicateProperty = false,
                TestField = null
            };
            var serviceProvider = new Mock<IServiceProvider>();
            var sut = new RequiredIfAttributesMatch(nameof(TestDto.PredicateProperty), true);
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>())).Returns(dto);

            // Act
            var result = sut.GetValidationResult(
                dto.TestField, 
                new ValidationContext(dto, serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void PredicateNullAndRequiredFieldNull_Returns_True()
        {
            // Arrange
            var dto = new TestDto
            {
                PredicateProperty = null,
                TestField = null
            };
            var serviceProvider = new Mock<IServiceProvider>();
            var sut = new RequiredIfAttributesMatch(nameof(TestDto.PredicateProperty), true);
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>())).Returns(dto);

            // Act
            var result = sut.GetValidationResult(
                dto.TestField,
                new ValidationContext(dto, serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        public class TestDto
        {
            public bool? PredicateProperty { get; set; }

            [RequiredIfAttributesMatch(nameof(PredicateProperty), true)]
            public string TestField { get; set; }
        }

        public class TestDtoTdo
        {
            public bool Status { get; set; }

            [RequiredIfAttributesMatch(nameof(Status), true)]
            public string TestField { get; set; }
        }
    }
}