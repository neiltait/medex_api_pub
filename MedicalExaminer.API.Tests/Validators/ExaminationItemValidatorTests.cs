using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Models.Validators;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Validators
{
    public class ExaminationItemValidatorTests
    {

        [Fact]
        public async void IncorrectPatientGivenNameRaisesError()
        {
            var nhsNumberValidator = new Moq.Mock<IValidator<NhsNumberString>>().Object;
            var locationIdValidator = new Moq.Mock<IValidator<LocationIdString>>().Object;
            var dataToValidate = new ExaminationItem()
            {
                GivenNames = "J",
                Surname = "Beer",
                Gender = ExaminationGender.Female
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator, locationIdValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Invalid Given Name");
            result.First().Code.Should().Be("Invalid");
        }

        [Fact]
        public async void IncorrectPatientSurnameRaisesError()
        {
            var nhsNumberValidator = new Moq.Mock<IValidator<NhsNumberString>>().Object;
            var locationIdValidator = new Moq.Mock<IValidator<LocationIdString>>().Object;
            var dataToValidate = new ExaminationItem()
            {
                GivenNames = "Jo",
                Surname = "B",
                Gender = ExaminationGender.Female
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator, locationIdValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Invalid Surname");
            result.First().Code.Should().Be("Invalid");
        }

        [Fact]
        public async void DateOfBirthKnownButNotCompletedReturnsError()
        {
            var nhsNumberValidator = new Moq.Mock<IValidator<NhsNumberString>>().Object;
            var locationIdValidator = new Moq.Mock<IValidator<LocationIdString>>().Object;
            var dataToValidate = new ExaminationItem()
            {
                GivenNames = "Joanna",
                Surname = "Beer",
                DateOfBirthKnown = true,
                DateOfBirth = null,
                Gender = ExaminationGender.Female
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator, locationIdValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("If date of birth is known a date must be provided");
            result.First().Code.Should().Be("IsNull");
        }

        [Fact]
        public async void DateOfDeathKnownButNotCompletedReturnsError()
        {
            var nhsNumberValidator = new Moq.Mock<IValidator<NhsNumberString>>().Object;
            var locationIdValidator = new Moq.Mock<IValidator<LocationIdString>>().Object;
            var dataToValidate = new ExaminationItem()
            {
                GivenNames = "Joanna",
                Surname = "Beer",
                DateOfDeathKnown = true,
                DateOfBirth = null,
                Gender = ExaminationGender.Female
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator, locationIdValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("If date of death is known a date must be provided.");
            result.First().Code.Should().Be("IsNull");
        }

        [Fact]
        public async void GenderNotCompletedExaminationItemReturnsError()
        {
            var nhsNumberValidator = new Moq.Mock<IValidator<NhsNumberString>>().Object;
            var locationIdValidator = new Moq.Mock<IValidator<LocationIdString>>().Object;

            var dataToValidate = new ExaminationItem()
            {
                GivenNames = "Joanna",
                Surname = "Beer",
                DateOfBirthKnown = true,
                DateOfBirth = new DateTime(1984, 12, 24),
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator, locationIdValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Gender must be specified");
            result.First().Code.Should().Be("IsNull");
        }

        [Fact]
        public async void MinimumDataRequiredExaminationItemReturnsNoErrors()
        {
            var nhsNumberValidator = new Moq.Mock<IValidator<NhsNumberString>>().Object;
            var locationIdValidator = new Moq.Mock<IValidator<LocationIdString>>().Object;

            var dataToValidate = new ExaminationItem()
            {
                GivenNames = "Joanna",
                Surname = "Beer",
                DateOfBirthKnown = true,
                DateOfBirth = new DateTime(1984, 12, 24),
                Gender = ExaminationGender.Female
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator, locationIdValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count().Should().Be(0);
        }

        [Fact]
        public async void DiedBeforeTheyWereBornReturnsError()
        {
            var nhsNumberValidator = new Moq.Mock<IValidator<NhsNumberString>>().Object;
            var locationIdValidator = new Moq.Mock<IValidator<LocationIdString>>().Object;

            var dataToValidate = new ExaminationItem()
            {
                GivenNames = "Joanna",
                Surname = "Beer",
                DateOfBirthKnown = true,
                DateOfBirth = new DateTime(2018, 12, 24),
                DateOfDeathKnown = true,
                DateOfDeath = new DateTime(1984, 12, 24),
                TimeOfDeathKnown = true,
                TimeOfDeath = new TimeSpan(12, 21, 00),
                Gender = ExaminationGender.Female
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator, locationIdValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count().Should().Be(1);
            result.First().Message.Should().Be("Date of birth must be before date of death.");
            result.First().Code.Should().Be("Invalid");
        }

        [Fact]
        public async void FullDataRequiredExaminationItemReturnsNoErrors()
        {
            // Arrange
            var nhsNumberValidator = new Moq.Mock<IValidator<NhsNumberString>>();
            nhsNumberValidator.Setup(validator => validator.ValidateAsync(new NhsNumberString("1111111111")))
                .Returns(Task.FromResult(Enumerable.Empty<ValidationError>()));

            var locationIdValidator = new Moq.Mock<IValidator<LocationIdString>>().Object;

            var dataToValidate = new ExaminationItem()
            {
                GivenNames = "Joanna",
                Surname = "Beer",
                DateOfBirthKnown = true,
                DateOfBirth = new DateTime(1984, 12, 24),
                DateOfDeathKnown = true,
                DateOfDeath = new DateTime(2018, 12, 24),
                TimeOfDeathKnown = true,
                TimeOfDeath = new TimeSpan(12, 21, 00),
                NhsNumberKnown = true,
                NhsNumber = "1111111111",
                MedicalExaminerOfficeResponsible = "Somewhere",
                PlaceDeathOccured = "Somewhere Else",
                Gender = ExaminationGender.Female
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator.Object, locationIdValidator);

            // Act
            var result = await sut.ValidateAsync(dataToValidate);

            // Assert
            result.Count().Should().Be(0);
        }
    }
}
