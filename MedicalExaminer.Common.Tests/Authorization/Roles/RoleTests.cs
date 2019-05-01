using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MedicalExaminer.Common.Authorization.Roles;
using Xunit;

namespace MedicalExaminer.Common.Tests.Authorization.Roles
{
    /// <summary>
    /// Role Tests.
    /// </summary>
    public class RoleTests
    {
        /// <summary>
        /// Test MedicalExaminerOfficerRole Constructor Adds Grants.
        /// </summary>
        [Fact]
        public void MedicalExaminerOfficerRole_ConstorAddGrants()
        {
            // Act
            var sut = new MedicalExaminerOfficerRole();

            // Assert
            sut.Granted.Count.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// Test MedicalExaminerRole Constructor Adds Grants.
        /// </summary>
        [Fact]
        public void MedicalExaminerRole_ConstorAddGrants()
        {
            // Act
            var sut = new MedicalExaminerRole();

            // Assert
            sut.Granted.Count.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// Test ServiceAdministratorRole Constructor Adds Grants.
        /// </summary>
        [Fact]
        public void ServiceAdministratorRole_ConstorAddGrants()
        {
            // Act
            var sut = new ServiceAdministratorRole();

            // Assert
            sut.Granted.Count.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// Test ServiceOwnerRole Constructor Adds Grants.
        /// </summary>
        [Fact]
        public void ServiceOwnerRole_ConstorAddGrants()
        {
            // Act
            var sut = new ServiceOwnerRole();

            // Assert
            sut.Granted.Count.Should().BeGreaterThan(0);
        }
    }
}
