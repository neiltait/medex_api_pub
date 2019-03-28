using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class ValidMedicalExaminerOfficerTests
    {
        private readonly Mock<IUserPersistence> _userPersistence;

        private readonly Mock<IServiceProvider> _serviceProvider;

        private readonly ValidationContext _context;

        public ValidMedicalExaminerOfficerTests()
        {
            _userPersistence = new Mock<IUserPersistence>();

            _serviceProvider = new Mock<IServiceProvider>();

            _serviceProvider
                .Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(_userPersistence.Object);

            _context = new ValidationContext(new object(), _serviceProvider.Object, new Dictionary<object, object>());
        }

        [Fact]
        public async void MedicalExaminerOfficerFound_ReturnsSuccess()
        {
            // Arrange
            var userId = "1";
            var userFound = new MeUser
            {
                UserId = userId,
                UserRole = UserRoles.MedicalExaminerOfficer
            };
            var expectedResult = ValidationResult.Success;

            _userPersistence
                .Setup(persistence => persistence.GetUserAsync(userId))
                .Returns(Task.FromResult(userFound));

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(userId, _context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void MedicalExaminerOfficerNotFound_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            var expectedResult = new ValidationResult("The Medical Examiner Officer has not been found");

            _userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Throws(new ArgumentException());

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(userId, _context);

            // Assert
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }

        [Fact]
        public async void MedicalExaminerOfficerUserItemIsNull_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            var userFound = new MeUser
            {
                UserId = userId,
                UserRole = UserRoles.MedicalExaminerOfficer
            };
            var expectedResult =
                new ValidationResult("Item not recognised as of type useritem for Medical Examiner Officer");

            _userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Returns(Task.FromResult(userFound));

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(userId, _context);

            // Assert
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }

        [Fact]
        public async void MedicalExaminerOfficerWrongUserType_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            var userFound = new MeUser
            {
                UserId = userId,
                UserRole = UserRoles.ServiceAdministrator
            };
            var expectedResult = new ValidationResult("The user is not a Medical Examiner Officer");

            _userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Returns(Task.FromResult(userFound));

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(userId, _context);

            // Assert
            result.Should().NotBeNull();
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }
    }
}