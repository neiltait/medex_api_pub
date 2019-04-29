using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Authorization.ExaminationContext;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Model Binder Tests.
    /// </summary>
    public class ExaminationValidationModelBinderTests
    {
        [Fact]
        public void BindModelAsync_ThrowsException_WhenOriginalModelBinderIsNull()
        {
            // Arrange
            const IModelBinder expectedModelBinder = null;
            var sut = new ExaminationValidationModelBinder(expectedModelBinder);
            var context = new Mock<ModelBindingContext>();

            // Act
            Func<Task> act = async () => await sut.BindModelAsync(context.Object);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async void BindModelAsync_Completes_WhenOriginalBinderFails()
        {
            // Arrange
            var expectedModelBinder = new Mock<IModelBinder>();

            var sut = new ExaminationValidationModelBinder(expectedModelBinder.Object);
            var context = new Mock<ModelBindingContext>();

            expectedModelBinder
                .Setup(emb => emb.BindModelAsync(It.IsAny<ModelBindingContext>()))
                .Returns((ModelBindingContext mbc) => Task.CompletedTask);

            // Act
            await sut.BindModelAsync(context.Object);

            // Assert, no exceptions thrown.
        }

        [Fact]
        public async void BindModelAsync_ThrowsInvalidOperationException_WhenExaminationRetrievalServiceNotRegistered()
        {
            // Arrange
            var expectedModelBinder = new Mock<IModelBinder>();
            var expectedModelName = "expectedModelName";
            var sut = new ExaminationValidationModelBinder(expectedModelBinder.Object);
            var context = new Mock<ModelBindingContext>(MockBehavior.Strict);
            var expectedModel = new PutMedicalTeamRequest();

            context
                .Setup(c => c.HttpContext.RequestServices.GetService(
                    typeof(IAsyncQueryHandler<ExaminationRetrievalQuery, MedicalExaminer.Models.Examination>)))
                .Returns(null);

            expectedModelBinder
                .Setup(emb => emb.BindModelAsync(It.IsAny<ModelBindingContext>()))
                .Returns((ModelBindingContext mbc) => Task.CompletedTask);

            context.SetupGet(c => c.ModelName).Returns(expectedModelName);
            context
                .Setup(c => c.ValueProvider.GetValue(expectedModelName))
                .Returns(new ValueProviderResult(new[] {"1"}));

            context.SetupGet(c => c.Result).Returns(ModelBindingResult.Success(expectedModel));

            // Act
            Func<Task> act = async () => await sut.BindModelAsync(context.Object);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async void BindModelAsync_SetsExaminationOnContextProvider()
        {
            // Arrange
            var expectedModelBinder = new Mock<IModelBinder>(MockBehavior.Strict);
            var expectedModelName = "expectedModelName";

            var sut = new ExaminationValidationModelBinder(expectedModelBinder.Object);
            var context = new Mock<ModelBindingContext>(MockBehavior.Strict);
            var expectedModel = new PutMedicalTeamRequest();
            var expectedExamination = new Examination();

            var examinationRetrievalServiceMock =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, MedicalExaminer.Models.Examination>>(MockBehavior.Strict);

            examinationRetrievalServiceMock
                .Setup(ers => ers.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(expectedExamination));

            context
                .Setup(c => c.HttpContext.RequestServices.GetService(
                    typeof(IAsyncQueryHandler<ExaminationRetrievalQuery, MedicalExaminer.Models.Examination>)))
                .Returns(examinationRetrievalServiceMock.Object);

            var examinationValidationContextFactory = new ExaminationValidationContextFactory();

            context
                .Setup(c => c.HttpContext.RequestServices.GetService(
                    typeof(ExaminationValidationContextFactory)))
                .Returns(examinationValidationContextFactory);

            var examinationValidationContextProvider = new ExaminationValidationContextProvider();

            context
                .Setup(c => c.HttpContext.RequestServices.GetService(
                    typeof(ExaminationValidationContextProvider)))
                .Returns(examinationValidationContextProvider);

            expectedModelBinder
                .Setup(emb => emb.BindModelAsync(It.IsAny<ModelBindingContext>()))
                .Returns((ModelBindingContext mbc) => Task.CompletedTask);

            context.SetupGet(c => c.ModelName).Returns(expectedModelName);
            context
                .Setup(c => c.ValueProvider.GetValue(expectedModelName))
                .Returns(new ValueProviderResult(new[] { "1" }));

            context.SetupGet(c => c.Result).Returns(ModelBindingResult.Success(expectedModel));

            // Act
            await sut.BindModelAsync(context.Object);

            // Assert
            examinationValidationContextProvider.Current.Examination.Should().Be(expectedExamination);
        }
    }
}
