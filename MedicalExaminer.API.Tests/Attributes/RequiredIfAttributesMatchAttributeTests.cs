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
        public class TestDto
        {
            public bool predicateProperty { get; set; }

            [RequiredIfAttributesMatch(nameof(predicateProperty), true)]
            public string testField { get; set; }
        }

        public class TestDtoTdo
        {
            public bool status { get; set; }

            [RequiredIfAttributesMatch(nameof(status), true)]
            public string testField { get; set; }
        }


        [Fact]
        public async void PredicateFinalAndRequiredFieldPopulated_Returns_True()
        {
            // Arrange
            var dto = new TestDtoTdo
            {
                status = true,
                testField = null
            };
            var serviceProvider = new Mock<IServiceProvider>().Object;
            serviceProvider.GetService(dto.GetType());
            var validationContext = new ValidationContext(dto, serviceProvider, new Dictionary<object, object>());
            var sut = new RequiredIfAttributesMatch("status", true);
            var expectedResult = ValidationResult.Success;

            // Act
            var result = sut.GetValidationResult(dto.testField, validationContext);

            //Assert
            Assert.Equal("The TestDtoTdo field is required.", result.ErrorMessage);
        }


        [Fact]
        public async void PredicateTrueAndRequiredFieldPopulated_Returns_True()
        {
            // Arrange
            var dto = new TestDto
            {
                predicateProperty = true,
                testField = "bob"
            };
            var serviceProvider = new Mock<IServiceProvider>().Object;
            serviceProvider.GetService(dto.GetType());
            var validationContext = new ValidationContext(dto, serviceProvider, new Dictionary<object, object>());
            var sut = new RequiredIfAttributesMatch("predicateProperty", true);
            var expectedResult = ValidationResult.Success;

            // Act
            var result = sut.GetValidationResult(dto.testField, validationContext);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void PredicateTrueAndRequiredFieldNull_Returns_False()
        {
            // Arrange
            var dto = new TestDto
            {
                predicateProperty = true,
                testField = null
            };
            var serviceProvider = new Mock<IServiceProvider>();
            var sut = new RequiredIfAttributesMatch("predicateProperty", true);
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>())).Returns(dto);


            // Act
            var result = sut.GetValidationResult(dto.testField,
                new ValidationContext(dto, serviceProvider.Object, new Dictionary<object, object>()));

            //Assert
            Assert.Equal("The TestDto field is required.", result.ErrorMessage);
        }

        [Fact]
        public async void PredicateFalseAndRequiredFieldPopulated_Returns_True()
        {
            // Arrange
            var dto = new TestDto
            {
                predicateProperty = false,
                testField = "something"
            };
            var serviceProvider = new Mock<IServiceProvider>();
            var sut = new RequiredIfAttributesMatch("predicateProperty", true);
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>())).Returns(dto);


            // Act
            var result = sut.GetValidationResult(dto.testField,
                new ValidationContext(dto, serviceProvider.Object, new Dictionary<object, object>()));

            //Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public async void PredicateFalseAndRequiredFieldNull_Returns_True()
        {
            // Arrange
            var dto = new TestDto
            {
                predicateProperty = false,
                testField = null
            };
            var serviceProvider = new Mock<IServiceProvider>();
            var sut = new RequiredIfAttributesMatch("predicateProperty", true);
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>())).Returns(dto);


            // Act
            var result = sut.GetValidationResult(dto.testField, 
                new ValidationContext(dto, serviceProvider.Object, new Dictionary<object, object>()));

            //Assert
            Assert.Equal(ValidationResult.Success, result);
        }

    }
}
