using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using MedicalExaminer.API.Models.v1;

namespace MedicalExaminer.API.Tests.Models.v1
{
    public class ResponseBaseTests
    {
        [Fact]
        public void Lookup_IsNull_ByDefault()
        {
            var sut = new ResponseBase();
            sut.Lookups.Should().BeNull();
        }

        [Fact]
        public void Lookup_IsAssigned_WhenFirstLookupIsAdded()
        {
            var sut = new ResponseBase();
            sut.AddLookup("key", new List<object>());
            sut.Lookups.Should().NotBeNull();
        }

        [Fact]
        public void AddLookup_ThrowsArgumentException_WhenKeyAddedTwice()
        {
            // Arrang
            var sut = new ResponseBase();
            var expectedKey = "expectedKey";
            var expectedLookup = new List<object>();
            sut.AddLookup(expectedKey, expectedLookup);

            // Act
            Action act = () => sut.AddLookup(expectedKey, expectedLookup);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
