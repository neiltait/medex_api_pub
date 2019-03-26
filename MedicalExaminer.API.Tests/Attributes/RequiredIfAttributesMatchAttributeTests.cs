using MedicalExaminer.API.Attributes;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
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

        [Fact]
        public async void PredicateTrueAndRequiredFieldPopulated_Returns_True()
        {
            // Arrange
            var dto = new TestDto
            {
                predicateProperty = true,
                testField = "bob"
            };
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(context => context.GetService(It.IsAny<Type>()))
                .Returns(dto);
            var validationContext = new ValidationContext(dto, new Dictionary<object, object>());
            var sut = new RequiredIfAttributesMatch("predicateProperty", true);

            // Act
            var result = sut.GetValidationResult(dto.testField, validationContext);

            //Assert
            //Assert.True(result);
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

            var sut = new RequiredIfAttributesMatch("predicateProperty", true);

            // Act
            var result = sut.IsValid(dto.testField);

            //Assert
            Assert.False(result);
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

            var sut = new RequiredIfAttributesMatch("predicateProperty", true);

            // Act
            var result = sut.IsValid(dto.testField);

            //Assert
            Assert.True(result);
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

            var sut = new RequiredIfAttributesMatch("predicateProperty", true);

            // Act
            var result = sut.IsValid(dto.testField);

            //Assert
            Assert.True(result);
        }

    }
}
