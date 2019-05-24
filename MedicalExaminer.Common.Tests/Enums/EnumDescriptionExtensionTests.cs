using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FluentAssertions;
using MedicalExaminer.Common.Enums;
using Xunit;

namespace MedicalExaminer.Common.Tests.Enums
{
    /// <summary>
    /// Enum Description Extension Tests.
    /// </summary>
    public class EnumDescriptionExtensionTests
    {
        private enum Test
        {
            [Description("Item 1")]
            Item1,

            Item2,
        }

        /// <summary>
        /// Get Description Should Return Description.
        /// </summary>
        [Fact]
        public void GetDescription_ShouldReturnDescription()
        {
            const string expectedResult = "Item 1";
            const Test test = Test.Item1;

            test.GetDescription().Should().Be(expectedResult);
        }

        /// <summary>
        /// Get Description Should Return Description.
        /// </summary>
        [Fact]
        public void GetDescription_ShouldReturnNull_WhenNoAttributePresent()
        {
            const Test test = Test.Item2;

            test.GetDescription().Should().BeNull();
        }
    }
}
