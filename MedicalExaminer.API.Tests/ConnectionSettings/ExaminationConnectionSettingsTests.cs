using System;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using Xunit;

namespace MedicalExaminer.API.Tests.ConnectionSettings
{
    public class ExaminationConnectionSettingsTests
    {
        [Fact]
        public void TheCorrectCollectionNameIsReturned()
        {
            var uri = new Uri("https://www.methods.co.uk");
            var primaryKey = "primaryKey";
            var databaseId = "databaseId";
            var sut = new ExaminationConnectionSettings(uri, primaryKey, databaseId);

            Assert.Equal("Examinations", sut.Collection);
        }

        [Fact]
        public void NullOrEmptyPrimaryKeyThrowsArgumentNullException()
        {
            var uri = new Uri("https://www.methods.co.uk");
            string primaryKey = null;
            string databaseId = "databaseId";
            Action act = () => new ExaminationConnectionSettings(uri, primaryKey, databaseId);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void NullOrEmptyDatabaseIdThrowsArgumentNullException()
        {
            var uri = new Uri("https://www.methods.co.uk");
            string primaryKey = "primaryKey";
            string databaseId = null;
            Action act = () => new ExaminationConnectionSettings(uri, primaryKey, databaseId);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void NullUriThrowsArgumentNullException()
        {
            Uri uri = null;
            string primaryKey = "primaryKey";
            var databaseId = "databaseId";
            Action act = () => new ExaminationConnectionSettings(uri, primaryKey, databaseId);

            act.Should().Throw<ArgumentNullException>();
        }

    }
}
