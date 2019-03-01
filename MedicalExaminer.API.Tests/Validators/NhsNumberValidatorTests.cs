using System.Linq;
using FluentAssertions;
using MedicalExaminer.API.Models.Validators;
using Xunit;

namespace MedicalExaminer.API.Tests.Validators
{
    public class NhsNumberValidatorTests
    {
        [Fact]
        public async void InsufficientDigitsInNumberReturnsErrors()
        {
            var numberToValidate = "123456";

            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(numberToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Incorrect NHS Number");
            result.First().Code.Should().Be("Invalid");
        }

        [Fact]
        public async void ExcessDigitsInNumberReturnsErrors()
        {
            var numberToValidate = "012345678910";

            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(numberToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Incorrect NHS Number");
            result.First().Code.Should().Be("Invalid");
        }


        [Fact]
        public async void AlphanumericDigitsInNumberReturnsErrors()
        {
            var numberToValidate = "123ac 45678";

            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(numberToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Incorrect NHS Number");
            result.First().Code.Should().Be("Invalid");
        }

        [Fact]
        public async void CorrectNhsNumberReturnsNoErrors()
        {
            var numberToValidate = "943 476 5919";

            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(numberToValidate);

            result.Count().Should().Be(0);
        }

        [Fact]
        public async void IncorrectNhsNumberReturnsErrors()
        {
            var numberToValidate = "987 654 4321";

            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(numberToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Incorrect NHS Number");
            result.First().Code.Should().Be("Invalid");
        }
    }
}
