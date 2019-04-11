using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MedicalExaminer.API.Helpers;
using Xunit;

namespace MedicalExaminer.API.Tests.Helpers
{
    public class OktaTokenParserTest
    {
        [Fact]
        public void OktaTokenParser_FullHttpRequest()
        {
            //Arrange
            var httpRequestAuthorisation = "Bearer 1234fgtvcjk0sdyakkq";

            //Act
            var result = OktaTokenParser.ParseHttpRequestAuthorisation(httpRequestAuthorisation);

            //Assert
            Assert.Equal("1234fgtvcjk0sdyakkq", result);

        }

        [Fact]
        public void OktaTokenParse_NullThrowsException()
        {

            // Act
            Action act = () => OktaTokenParser.ParseHttpRequestAuthorisation(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();

        }


        [Fact]
        public void OktaTokenParse_TooShortParameterThrowsException()
        {

            // Act
            Action act = () => OktaTokenParser.ParseHttpRequestAuthorisation("1");

            // Assert
            act.Should().Throw<ArgumentException>();

        }
    }
}
