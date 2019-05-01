using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Authorization.ExaminationContext;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Attributes
{
    public class ValidRoleOnExaminationTests
    {
        private readonly ValidationContext _context;

        private readonly Mock<IServiceProvider> _serviceProvideMock;

        private readonly Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>
            _userRetrievalByIdServiceMock;

        public ValidRoleOnExaminationTests()
        {
            _serviceProvideMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            _userRetrievalByIdServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>(MockBehavior.Strict);

            _serviceProvideMock
                .Setup(context =>
                    context.GetService(
                        typeof(IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>)))
                .Returns(_userRetrievalByIdServiceMock.Object);

            _context = new ValidationContext(new object(), _serviceProvideMock.Object, new Dictionary<object, object>());
        }

        private void SetupExaminationValidationContextProvider(
            Mock<IServiceProvider> serviceProvider,
            Examination expectedExamination)
        {
            var examinationValidationContextProvider = new ExaminationValidationContextProvider();
            examinationValidationContextProvider.Set(new ExaminationValidationContext(expectedExamination));

            serviceProvider
                .Setup(context => context.GetService(typeof(ExaminationValidationContextProvider)))
                .Returns(examinationValidationContextProvider);
        }

        private void SetupUserRetrievalByIdMock(string userId, MeUser expectedUser)
        {
            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(query => query.UserId == userId)))
                .Returns(Task.FromResult(expectedUser));
        }

        [Fact]
        public void GetValidationResult_ShouldThrowInvalidOperationException_WhenExaminationValidationContextProviderNotSetupOnContext()
        {
            // Arrange
            var expectedUserId = "expectedUserId";
            var serviceProvider = new Mock<IServiceProvider>();
            var context = new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>());

            var sut = new ValidRoleOnExamination(UserRoles.MedicalExaminer);

            // Act
            Action act = () => sut.GetValidationResult(expectedUserId, context);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetValidationResult_Fails_WhenExaminationNotSet()
        {
            // Arrange
            SetupExaminationValidationContextProvider(_serviceProvideMock, null);
            var userId = "1";
            var expectedResult = new ValidationResult("The examination was not present to confirm role validation.");

            var sut = new ValidRoleOnExamination(UserRoles.MedicalExaminer);

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
        public void GetValidationResult_ShouldThrowInvalidOperationException_WhenUserRetrievalServiceNotSetupOnContext()
        {
            // Arrange
            Mock<IServiceProvider> serviceProvideMock = new Mock<IServiceProvider>();
            SetupExaminationValidationContextProvider(serviceProvideMock, new Examination());
            var expectedUserId = "expectedUserId";
            var context = new ValidationContext(new object(), serviceProvideMock.Object, new Dictionary<object, object>());

            var sut = new ValidRoleOnExamination(UserRoles.MedicalExaminer);

            // Act
            Action act = () => sut.GetValidationResult(expectedUserId, context);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void MedicalExaminerNotFound_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            var expectedResult = new ValidationResult("The user with id `1` was not found.");
            SetupUserRetrievalByIdMock(userId, null);
            SetupExaminationValidationContextProvider(_serviceProvideMock, new Examination());

            var sut = new ValidRoleOnExamination(UserRoles.MedicalExaminer);

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
        public void MedicalExaminerUserItemIsNotString_ReturnsFail()
        {
            // Arrange
            SetupExaminationValidationContextProvider(_serviceProvideMock, new Examination());
            var expectedResult =
                new ValidationResult("Item not recognised as of type useritem for `Medical Examiner`.");

            var sut = new ValidRoleOnExamination(UserRoles.MedicalExaminer);

            // Act
            var result = sut.GetValidationResult(new object(), _context);

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
            var expectedUser = new MeUser
            {
                UserId = userId,
                Permissions = new List<MEUserPermission>()
            };
            var expectedExamination = new Examination()
            {
                SiteLocationId = "site",
            };

            var expectedResult = new ValidationResult("The user is not a `Medical Examiner`.");
            SetupUserRetrievalByIdMock(userId, expectedUser);
            SetupExaminationValidationContextProvider(_serviceProvideMock, expectedExamination);

            var sut = new ValidRoleOnExamination(UserRoles.MedicalExaminer);

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
        public void MedicalExaminerFound_ReturnsSuccess()
        {
            // Arrange
            var expectedResult = ValidationResult.Success;
            var userId = "1";
            var expectedUser = new MeUser
            {
                UserId = userId,
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        LocationId = "site",
                        UserRole = (int)UserRoles.MedicalExaminer,
                    }
                }
            };
            var expectedExamination = new Examination()
            {
                SiteLocationId = "site",
            };
            SetupUserRetrievalByIdMock(userId, expectedUser);
            SetupExaminationValidationContextProvider(_serviceProvideMock, expectedExamination);

            var sut = new ValidRoleOnExamination(UserRoles.MedicalExaminer);

            // Act
            var result = sut.GetValidationResult(userId, _context);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
