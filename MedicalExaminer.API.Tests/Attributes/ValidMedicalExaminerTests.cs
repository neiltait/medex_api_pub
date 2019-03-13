using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MedicalExaminer.API.Tests.Attributes
{
    internal class ValidMedicalExaminerProxy : ValidMedicalExaminer
    {
        public ValidationResult IsValid(object value, ValidationContext context)
        {
            return base.IsValid(value, context);
        }
    }

    public class ValidMedicalExaminerTests
    {
        [Fact]
        public async void MedicalExaminerFound_ReturnsSuccess()
        {
            // Arrange
            var userItem = new UserItem();
            userItem.UserId = "1";
            var userFound = new MeUser();
            userFound.UserId = "1";
            userFound.UserRole = UserRoles.MedicalExaminer;
            var expectedResult = ValidationResult.Success;
            var userPersistence = new Mock<IUserPersistence>();
            userPersistence.Setup(persistence =>
                persistence.GetUserAsync("1")).Returns(Task.FromResult<MeUser>(userFound));
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<MeUser>(It.IsAny<UserItem>()))
                .Returns(userFound);

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider.Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(userPersistence.Object);
            serviceProvider.Setup(context => context.GetService(typeof(IMapper)))
                .Returns(mapper.Object);

            var sut = new ValidMedicalExaminer();

            // Act
            var result = sut.GetValidationResult(userItem, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));
            // Assert

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void MedicalExaminerWrongUserType_ReturnsFail()
        {
            // Arrange
            var userItem = new UserItem();
            userItem.UserId = "1";
            var userFound = new MeUser();
            userFound.UserId = "1";
            userFound.UserRole = UserRoles.ServiceAdministrator; //wrong type!
            var expectedResult = new ValidationResult("The user is not a medical examiner");
            var userPersistence = new Mock<IUserPersistence>();
            userPersistence.Setup(persistence =>
                persistence.GetUserAsync("1")).Returns(Task.FromResult<MeUser>(userFound));
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<MeUser>(It.IsAny<UserItem>()))
                .Returns(userFound);

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider.Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(userPersistence.Object);
            serviceProvider.Setup(context => context.GetService(typeof(IMapper)))
                .Returns(mapper.Object);

            var sut = new ValidMedicalExaminer();

            // Act
            var result = sut.GetValidationResult(userItem, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));
            
            // Assert
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }

        [Fact]
        public async void MedicalExaminerUserItemIsNull_ReturnsFail()
        {
            // Arrange
            UserItem userItem = null; //null
            var userFound = new MeUser();
            userFound.UserId = "1";
            userFound.UserRole = UserRoles.MedicalExaminer;
            var expectedResult = new ValidationResult("Cannot get id for medical examiner");
            var userPersistence = new Mock<IUserPersistence>();
            userPersistence.Setup(persistence =>
                persistence.GetUserAsync("1")).Returns(Task.FromResult<MeUser>(userFound));
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<MeUser>(It.IsAny<UserItem>()))
                .Returns(userFound);

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider.Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(userPersistence.Object);
            serviceProvider.Setup(context => context.GetService(typeof(IMapper)))
                .Returns(mapper.Object);

            var sut = new ValidMedicalExaminer();

            // Act
            var result = sut.GetValidationResult(userItem, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }
    }
}
