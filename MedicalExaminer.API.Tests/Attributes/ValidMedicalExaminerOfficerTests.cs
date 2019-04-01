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
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class ValidMedicalExaminerOfficerTests
    {
        private readonly Mock<IUserPersistence> _userPersistence;

        private readonly ValidationContext _context;

        public ValidMedicalExaminerOfficerTests()
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

        // TODO: Re-enable once we have permissions and roles integrated again
        //[Fact]
        //public void MedicalExaminerOfficerFound_ReturnsSuccess()
        //{
        //    // Arrange
        //    var userId = "1";
        //    var userItem = new UserItem();
        //    userItem.UserId = userId;
        //    var userFound = new MeUser();
        //    userFound.UserId = userId;
        //    var expectedResult = ValidationResult.Success;

        //    _userPersistence
        //        .Setup(persistence => persistence.GetUserAsync(userId))
        //        .Returns(Task.FromResult(userFound));

        //    var sut = new ValidMedicalExaminerOfficer();

        //    // Act
        //    var result = sut.GetValidationResult(userId, _context);

        //    // Assert
        //    Assert.Equal(expectedResult, result);
        //}

        // TODO: Re-enable once we have permissions and roles integrated again
        //[Fact]
        //public void MedicalExaminerOfficerNotFound_ReturnsFail()
        //{
        //    // Arrange
        //    var userId = "1";
        //    var expectedResult = new ValidationResult("The Medical Examiner Officer has not been found");

        //    _userPersistence.Setup(persistence =>
        //        persistence.GetUserAsync(userId)).Throws(new ArgumentException());

        //    var sut = new ValidMedicalExaminerOfficer();

        //    // Act
        //    var result = sut.GetValidationResult(userId, _context);

        //    // Assert
        //    Assert.NotNull(result);
        //    if (result != null)
        //    {
        //        Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        //    }
        //}

        [Fact]
        public void MedicalExaminerOfficerUserItemIsNotString_ReturnsFail()
        {
            // Arrange
            var expectedResult =
                new ValidationResult("Item not recognised as of type useritem for Medical Examiner Officer");

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(new object(), _context);

            // Assert
            Assert.NotNull(result);
            if (result != null)
            {
                Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
            }
        }

        // TODO: Re-enable once we have permissions and roles integrated again
        //[Fact]
        //public void MedicalExaminerOfficerWrongUserType_ReturnsFail()
        //{
        //    // Arrange
        //    var userId = "1";
        //    var userFound = new MeUser
        //    {
        //        UserId = userId,
        //        UserRole = UserRoles.ServiceAdministrator
        //    };
        //    var expectedResult = new ValidationResult("The user is not a Medical Examiner Officer");

        //    _userPersistence.Setup(persistence =>
        //        persistence.GetUserAsync(userId)).Returns(Task.FromResult(userFound));

        //    var sut = new ValidMedicalExaminerOfficer();

        //    // Act
        //    var result = sut.GetValidationResult(userId, _context);

        //    // Assert
        //    Assert.NotNull(result);
        //    if (result != null)
        //    {
        //        Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        //    }
        //}
    }
}