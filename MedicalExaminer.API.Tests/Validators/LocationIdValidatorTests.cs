using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MedicalExaminer.API.Models.Validators;
using MedicalExaminer.Common;
using Moq;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Tests.Validators
{
    public class LocationIdValidatorTests
    {
        [Fact]
        public async void NoLocationIsFoundReturnsError()
        {
            var locationId = new Mock<LocationIdString>("bad location");
            var locationPersistence = new Mock<ILocationPersistence>();
            locationPersistence.Setup(persistence =>
                persistence.GetLocationAsync("bad location")).Throws<DocumentClientException>();
                   // .Returns(Task.FromResult<Location>(null));

            var sut = new LocationIdValidator(locationPersistence.Object);

            var result = await sut.ValidateAsync(locationId.Object);

            result.Count().Should().Be(1);
        }

        [Fact]
        public async void OneLocationIsFoundReturnsNoError()
        {

        }

        [Fact]
        public async void MultipleLocationsAreFoundReturnsError()
        {

        }
    }
}
