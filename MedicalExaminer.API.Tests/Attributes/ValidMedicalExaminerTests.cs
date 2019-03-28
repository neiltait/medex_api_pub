using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Common;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class ValidMedicalExaminerTests
    {
        private readonly Mock<IUserPersistence> _userPersistence;

        private readonly ValidationContext _context;

        public ValidMedicalExaminerTests()
        {
            _userPersistence = new Mock<IUserPersistence>();

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(_userPersistence.Object);

            _context = new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>());
        }

        [Fact]
        public void GetValidationResult_ShouldThrowNullReferenceException_WhenPersistanceNotSetupOnContext()
        {
            // Arrange
            var expectedUserId = "expectedUserId";
            var serviceProvider = new Mock<IServiceProvider>();
            var context = new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>());

            var sut = new ValidMedicalExaminer();

            // Act
            Action act = () => sut.GetValidationResult(expectedUserId, context);

            // Assert
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void MedicalExaminerFound_ReturnsSuccess()
        {
            // Arrange
            var userId = "1";
            var userFound = new MeUser
            {
                UserId = userId,
                UserRole = UserRoles.MedicalExaminer
            };
            var expectedResult = ValidationResult.Success;

            _userPersistence
                .Setup(persistence => persistence.GetUserAsync(userId))
                .Returns(Task.FromResult(userFound));

            var sut = new ValidMedicalExaminer();

            // Act
            var result = sut.GetValidationResult(userId, _context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void MedicalExaminerNotFound_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            var expectedResult = new ValidationResult("The Medical Examiner has not been found");

            _userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Throws(new ArgumentException());

            var sut = new ValidMedicalExaminer();

            // Act
            var result = sut.GetValidationResult(userId, _context);

            // Assert
            Assert.NotNull(result);
            if (result != null)
            {
                Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
            }
        }

        [Fact]
        public void MedicalExaminerUserItemIsNull_ReturnsFail()
        {
            // Arrange
            var expectedResult =
                new ValidationResult("Item not recognised as of type useritem for Medical Examiner");

            var sut = new ValidMedicalExaminer();

            // Act
            var result = sut.GetValidationResult(null, _context);

            // Assert
            Assert.NotNull(result);
            if (result != null)
            {
                Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
            }
        }

        [Fact]
        public void MedicalExaminerWrongUserType_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            var userFound = new MeUser
            {
                UserId = userId,
                UserRole = UserRoles.ServiceAdministrator
            };
            var expectedResult = new ValidationResult("The user is not a Medical Examiner");

            _userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Returns(Task.FromResult(userFound));

            var sut = new ValidMedicalExaminer();

            // Act
            var result = sut.GetValidationResult(userId, _context);

            // Assert
            Assert.NotNull(result);
            if (result != null)
            {
                Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
            }
        }
    }
}
