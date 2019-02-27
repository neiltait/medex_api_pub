using System;
using FluentAssertions;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.Validators;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Validators
{
    public class ExaminationItemValidatorTests
    {
        [Fact]
        public async void IncorrectPatientGivenNameRaisesError()
        {
            var nhsNumberValidator = new NhsNumberValidator();

            var dataToValidate = new ExaminationItem()
            {
                GivenName = "M",
                Surname = "Sharkey",
                Gender = ExaminationGender.Male
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count.Should().Be(1);
            result[0].Message.Should().Be("Invalid Given Name");
            result[0].Code.Should().Be("Invalid");
        }

        [Fact]
        public async void IncorrectPatientSurnameRaisesError()
        {
            var nhsNumberValidator = new NhsNumberValidator();

            var dataToValidate = new ExaminationItem()
            {
                GivenName = "Ma",
                Surname = "S",
                Gender = ExaminationGender.Male
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count.Should().Be(1);
            result[0].Message.Should().Be("Invalid Surname");
            result[0].Code.Should().Be("Invalid");
        }

        [Fact]
        public async void DateOfBirthKnownButNotCompletedReturnsError()
        {
            var nhsNumberValidator = new NhsNumberValidator();
            var dataToValidate = new ExaminationItem()
            {
                GivenName = "Mark",
                Surname = "Sharkey",
                DateOfBirthKnown = true,
                DateOfBirth = null,
                Gender = ExaminationGender.Male
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count.Should().Be(1);
            result[0].Message.Should().Be("If date of birth is known a date must be provided");
            result[0].Code.Should().Be("IsNull");
        }

        [Fact]
        public async void DateOfDeathKnownButNotCompletedReturnsError()
        {
            var nhsNumberValidator = new NhsNumberValidator();
            var dataToValidate = new ExaminationItem()
            {
                GivenName = "Mark",
                Surname = "Sharkey",
                DateOfDeathKnown = true,
                DateOfBirth = null,
                Gender = ExaminationGender.Male
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count.Should().Be(1);
            result[0].Message.Should().Be("If date of death is known a date must be provided.");
            result[0].Code.Should().Be("IsNull");
        }

        [Fact]
        public async void GenderNotCompletedExaminationItemReturnsError()
        {
            var nhsNumberValidator = new NhsNumberValidator();
            var dataToValidate = new ExaminationItem()
            {
                GivenName = "Mark",
                Surname = "Sharkey",
                DateOfBirthKnown = true,
                DateOfBirth = new DateTime(1984, 12, 24),
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count.Should().Be(1);
            result[0].Message.Should().Be("Gender must be specified");
            result[0].Code.Should().Be("IsNull");
        }

        [Fact]
        public async void MinimumDataRequiredExaminationItemReturnsNoErrors()
        {
            var nhsNumberValidator = new NhsNumberValidator();
            var dataToValidate = new ExaminationItem()
            {
                GivenName = "Mark",
                Surname = "Sharkey",
                DateOfBirthKnown = true,
                DateOfBirth = new DateTime(1984, 12, 24),
                Gender = ExaminationGender.Male
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count.Should().Be(0);
        }

        [Fact]
        public async void DiedBeforeTheyWereBornReturnsError()
        {
            var nhsNumberValidator = new NhsNumberValidator();
            var dataToValidate = new ExaminationItem()
            {
                GivenName = "Mark",
                Surname = "Sharkey",
                DateOfBirthKnown = true,
                DateOfBirth = new DateTime(2018, 12, 24),
                DateOfDeathKnown = true,
                DateOfDeath = new DateTime(1984, 12, 24),
                TimeOfDeathKnown = true,
                TimeOfDeath = new TimeSpan(12, 21, 00),
                Gender = ExaminationGender.Male
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count.Should().Be(1);
            result[0].Message.Should().Be("Date of birth must be before date of death.");
            result[0].Code.Should().Be("Invalid");
        }

        [Fact]
        public async void FullDataRequiredExaminationItemReturnsNoErrors()
        {
            var nhsNumberValidator = new NhsNumberValidator();
            var dataToValidate = new ExaminationItem()
            {
                GivenName = "Mark",
                Surname = "Sharkey",
                DateOfBirthKnown = true,
                DateOfBirth = new DateTime(1984, 12, 24),
                DateOfDeathKnown = true,
                DateOfDeath = new DateTime(2018, 12, 24),
                TimeOfDeathKnown = true,
                TimeOfDeath = new TimeSpan(12, 21, 00),
                NhsNumberKnown = true,
                NhsNumber = "943 476 5919",
                MedicalExaminerOfficeResponsible = "Somewhere",
                PlaceDeathOccured = "Somewhere Else",
                Gender = ExaminationGender.Male
            };

            var sut = new CheckExaminationItemValidator(nhsNumberValidator);
            var result = await sut.ValidateAsync(dataToValidate);

            result.Count.Should().Be(0);
        }
    }
}
