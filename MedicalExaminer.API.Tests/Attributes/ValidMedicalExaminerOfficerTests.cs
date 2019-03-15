﻿using System;
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
    public class ValidMedicalExaminerOfficerTests
    {
        [Fact]
        public async void MedicalExaminerOfficerFound_ReturnsSuccess()
        {
            // Arrange
            var userId = "1";
            var userItem = new UserItem();
            userItem.UserId = userId;
            var userFound = new MeUser();
            userFound.UserId = userId;
            userFound.UserRole = UserRoles.MedicalExaminerOfficer;
            var expectedResult = ValidationResult.Success;
            var userPersistence = new Mock<IUserPersistence>();
            userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Returns(Task.FromResult<MeUser>(userFound));
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<MeUser>(It.IsAny<UserItem>()))
                .Returns(userFound);

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider.Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(userPersistence.Object);
            serviceProvider.Setup(context => context.GetService(typeof(IMapper)))
                .Returns(mapper.Object);

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(userItem, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void MedicalExaminerOfficerWrongUserType_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            var userItem = new UserItem();
            userItem.UserId = userId;
            var userFound = new MeUser();
            userFound.UserId = userId;
            userFound.UserRole = UserRoles.ServiceAdministrator; //wrong type!
            var expectedResult = new ValidationResult("The user is not a medical examiner officer");
            var userPersistence = new Mock<IUserPersistence>();
            userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Returns(Task.FromResult<MeUser>(userFound));
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<MeUser>(It.IsAny<UserItem>()))
                .Returns(userFound);

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider.Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(userPersistence.Object);
            serviceProvider.Setup(context => context.GetService(typeof(IMapper)))
                .Returns(mapper.Object);

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(userItem, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }

        [Fact]
        public async void MedicalExaminerOfficerUserItemIsNull_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            UserItem userItem = null; //null
            var userFound = new MeUser();
            userFound.UserId = userId;
            userFound.UserRole = UserRoles.MedicalExaminerOfficer;
            var expectedResult = new ValidationResult("Item not recognised as of type useritem for medical examiner officer");
            var userPersistence = new Mock<IUserPersistence>();
            userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Returns(Task.FromResult<MeUser>(userFound));
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<MeUser>(It.IsAny<UserItem>()))
                .Returns(userFound);

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider.Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(userPersistence.Object);
            serviceProvider.Setup(context => context.GetService(typeof(IMapper)))
                .Returns(mapper.Object);

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(userItem, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }

        [Fact]
        public async void MedicalExaminerOfficerNotFound_ReturnsFail()
        {
            // Arrange
            var userId = "1";
            var userItem = new UserItem();
            userItem.UserId = userId;
            var userFound = new MeUser();
            userFound.UserId = userId;
            userFound.UserRole = UserRoles.MedicalExaminerOfficer;
            var expectedResult = new ValidationResult("The medical examiner officer has not been found");
            var userPersistence = new Mock<IUserPersistence>();
            userPersistence.Setup(persistence =>
                persistence.GetUserAsync(userId)).Throws(new ArgumentException());
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<MeUser>(It.IsAny<UserItem>()))
                .Returns(userFound);

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider.Setup(context => context.GetService(typeof(IUserPersistence)))
                .Returns(userPersistence.Object);
            serviceProvider.Setup(context => context.GetService(typeof(IMapper)))
                .Returns(mapper.Object);

            var sut = new ValidMedicalExaminerOfficer();

            // Act
            var result = sut.GetValidationResult(userItem, new ValidationContext(new object(), serviceProvider.Object, new Dictionary<object, object>()));

            // Assert
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }
    }
}