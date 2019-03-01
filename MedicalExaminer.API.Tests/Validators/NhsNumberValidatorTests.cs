using System.Linq;
using FluentAssertions;
using MedicalExaminer.API.Models.Validators;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Validators
{
    public class NhsNumberValidatorTests
    {
        [Fact]
        public async void InsufficientDigitsInNumberReturnsErrors()
        {
            var nhsNumberString = new Mock<NhsNumberString>("123456");
            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(nhsNumberString.Object);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Incorrect NHS Number");
            result.First().Code.Should().Be("Invalid");
        }

        [Fact]
        public async void ExcessDigitsInNumberReturnsErrors()
        {
            var nhsNumberString = new Mock<NhsNumberString>("012345678910");
            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(nhsNumberString.Object);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Incorrect NHS Number");
            result.First().Code.Should().Be("Invalid");
        }


        [Fact]
        public async void AlphanumericDigitsInNumberReturnsErrors()
        {
            var nhsNumberString = new Mock<NhsNumberString>("123ac 45678");
            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(nhsNumberString.Object);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Incorrect NHS Number");
            result.First().Code.Should().Be("Invalid");
        }

        [Fact]
        public async void CorrectNhsNumberReturnsNoErrors()
        {
            var nhsNumberString = new Mock<NhsNumberString>("943 476 5919");
            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(nhsNumberString.Object);

            result.Count().Should().Be(0);
        }

        [Fact]
        public async void IncorrectNhsNumberReturnsErrors()
        {
            var nhsNumberString = new Mock<NhsNumberString>("987 654 4321");
            var sut = new NhsNumberValidator();
            var result = await sut.ValidateAsync(nhsNumberString.Object);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Incorrect NHS Number");
            result.First().Code.Should().Be("Invalid");
        }
    }
}
