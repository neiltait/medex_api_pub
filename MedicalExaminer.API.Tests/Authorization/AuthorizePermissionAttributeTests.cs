using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.Common.Authorization;
using Xunit;

namespace MedicalExaminer.API.Tests.Authorization
{
    public class AuthorizePermissionAttributeTests
    {
        [Fact]
        public void AuthorizePermissionAttribute_Initialises()
        {
            var expectedPermission = Permission.CreateExamination;
            var sut = new AuthorizePermissionAttribute(expectedPermission);

            sut.Policy.Should().Be($"HasPermission={expectedPermission}");
        }
    }
}
