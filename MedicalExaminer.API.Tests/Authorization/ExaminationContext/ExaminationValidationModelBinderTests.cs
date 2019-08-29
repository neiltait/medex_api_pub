using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Authorization.ExaminationContext;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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
            var expectedParameterName = "expectedParameterName";
            var sut = new ExaminationValidationModelBinder(expectedModelBinder.Object);
            var context = new Mock<ModelBindingContext>(MockBehavior.Strict);
            var expectedModel = new PutMedicalTeamRequest();
            var modelMetadata = MockDefaultModelMetadata();

            context
                .Setup(c => c.HttpContext.RequestServices.GetService(
                    typeof(IAsyncQueryHandler<ExaminationRetrievalQuery, MedicalExaminer.Models.Examination>)))
                .Returns(null);

            expectedModelBinder
                .Setup(emb => emb.BindModelAsync(It.IsAny<ModelBindingContext>()))
                .Returns((ModelBindingContext mbc) => Task.CompletedTask);

            context
                .SetupGet(c => c.ModelMetadata)
                .Returns(modelMetadata);
            context
                .Setup(c => c.ValueProvider.GetValue(expectedParameterName))
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
            var expectedParameterName = "expectedParameterName";

            var modelMetadata = MockDefaultModelMetadata();
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

            context
                .SetupGet(c => c.ModelMetadata)
                .Returns(modelMetadata);

            context
                .Setup(c => c.ValueProvider.GetValue(expectedParameterName))
                .Returns(new ValueProviderResult(new[] { "1" }));

            context.SetupGet(c => c.Result).Returns(ModelBindingResult.Success(expectedModel));

            // Act
            await sut.BindModelAsync(context.Object);

            // Assert
            examinationValidationContextProvider.Current.Examination.Should().Be(expectedExamination);
        }

        /// <summary>
        /// Create a mock <see cref="DefaultModelMetadata"/> to be used in place of the mock model binding.
        /// </summary>
        /// <remarks>Bare minimum required to make model binding function. Not necessarily correct.</remarks>
        /// <returns>A Default Model Metadata object.</returns>
        private DefaultModelMetadata MockDefaultModelMetadata()
        {
            var memberInfo = new Mock<MemberInfo>(MockBehavior.Strict);
            memberInfo.Setup(mi => mi.MemberType).Returns(MemberTypes.Method);
            memberInfo.Setup(mi => mi.DeclaringType).Returns(typeof(object));

            var parameterInfo = new Mock<ParameterInfo>(MockBehavior.Strict);
            parameterInfo.Setup(pi => pi.Name).Returns("name");
            parameterInfo.Setup(pi => pi.ParameterType).Returns(typeof(object));
            parameterInfo.Setup(pi => pi.Member).Returns(memberInfo.Object);

            var attributes = new Attribute[]
            {
                new ExaminationValidationModelBinderContextAttribute("expectedParameterName"),
            };

            // ReSharper disable once CoVariantArrayConversion
            parameterInfo.Setup(pi => pi.GetCustomAttributes(typeof(Attribute), false)).Returns(attributes);

            var modelAttributes = ModelAttributes.GetAttributesForParameter(parameterInfo.Object);

            var provider = new Mock<IModelMetadataProvider>(MockBehavior.Strict);
            var detailsProvider = new Mock<ICompositeMetadataDetailsProvider>(MockBehavior.Strict);
            var modelMetadataDetails = new DefaultMetadataDetails(ModelMetadataIdentity.ForType(typeof(object)), modelAttributes);

            var modelMetadata = new DefaultModelMetadata(provider.Object, detailsProvider.Object, modelMetadataDetails);

            return modelMetadata;
        }
    }
}
