using System;
using System.Collections.Generic;
using FluentAssertions;
using MedicalExaminer.API.Authorization.ExaminationContext;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Model Binder Provider Tests.
    /// </summary>
    public class ExaminationValidationModelBinderProviderTests
    {
        [Fact]
        public void GetBinder_ThrowsArgumentNullException_WhenContextNull()
        {
            // Arrange
            var sut = new ExaminationValidationModelBinderProvider(null);

            // Act
            Action act = () => sut.GetBinder(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetBinder_ReturnsNull_WhenMetadataModelTypeNotMatched()
        {
            // Arrange
            var sut = new ExaminationValidationModelBinderProvider(null);
            var metadata = new Mock<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(object)));
            var context = new Mock<ModelBinderProviderContext>();
            context
                .SetupGet(c => c.Metadata)
                .Returns(metadata.Object);

            // Act
            var result = sut.GetBinder(context.Object);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetBinder_ReturnsBinder_WhenTypeMatches()
        {
            // Arrange
            var modelBinderProvider = new Mock<IModelBinderProvider>(MockBehavior.Strict);
            modelBinderProvider
                .Setup(mbp => mbp
                    .GetBinder(It.IsAny<ModelBinderProviderContext>()))
                .Returns((IModelBinder)null);

            var sut = new ExaminationValidationModelBinderProvider(modelBinderProvider.Object);
            var metadata = new Mock<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(IExaminationValidationModel)));
            var context = new Mock<ModelBinderProviderContext>();

            context
                .SetupGet(c => c.Metadata)
                .Returns(metadata.Object);

            // Act
            var result = sut.GetBinder(context.Object);

            // Assert
            result.Should().BeOfType<ExaminationValidationModelBinder>();
        }
    }
}
