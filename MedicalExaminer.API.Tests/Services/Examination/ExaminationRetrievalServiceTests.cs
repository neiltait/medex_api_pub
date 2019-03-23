using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationRetrievalServiceTests
    {
        private Mock<IDocumentClientFactory> CreateClientFactory(Mock<IDocumentClient> client)
        {
            var clientFactory = new Mock<IDocumentClientFactory>();
            clientFactory.Setup(x => x.CreateClient(It.IsAny<IConnectionSettings>()))
                .Returns(client.Object);

            return clientFactory;
        }

        private Mock<ExaminationConnectionSettings> CreateConnectionSettings()
        {
            var uri = new Uri("https://www.it-is-any.com");
            return new Mock<ExaminationConnectionSettings>(uri, "a", "b");
        }

        private Mock<IDocumentClient> CreateDocumentClient<T>(Expression<Func<T, bool>> predicate, params T[] collectionDocuments)
        {
            IQueryable<T> dataSource = collectionDocuments.AsQueryable();

            var expected = dataSource.Where(predicate);

            var response = new FeedResponse<T>(expected);

            var mockDocumentQuery = new Mock<IFakeDocumentQuery<T>>();

            mockDocumentQuery
                .SetupSequence(_ => _.HasMoreResults)
                .Returns(true)
                .Returns(false);

            mockDocumentQuery
                .Setup(_ => _.ExecuteNextAsync<T>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var provider = new Mock<IQueryProvider>();
            provider
                .Setup(_ => _.CreateQuery<T>(It.IsAny<Expression>()))
                .Returns((Expression expression) =>
                {
                    return mockDocumentQuery.Object;
                });

            mockDocumentQuery.As<IQueryable<T>>().Setup(x => x.Provider).Returns(provider.Object);
            mockDocumentQuery.As<IQueryable<T>>().Setup(x => x.Expression).Returns(() => dataSource.Expression);
            mockDocumentQuery.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(() => dataSource.ElementType);
            mockDocumentQuery.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(dataSource.GetEnumerator());

            var client = new Mock<IDocumentClient>();

            client.Setup(_ => _.CreateDocumentQuery<T>(It.IsAny<Uri>(), It.IsAny<FeedOptions>()))
                  .Returns(mockDocumentQuery.Object);
            return client;
        }


        [Fact]
        public async virtual Task ExaminationIdFoundReturnsExpectedExamination()
        {
            //Arrange
            var id = "a";
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Id == id;
            var client = CreateDocumentClient(predicate, GenerateExaminations().ToArray());
            var clientFactory = CreateClientFactory(client);

            var connectionSettings = CreateConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            //Act
            var results = await dataAccess.GetItemAsync(connectionSettings.Object, predicate);

            //Assert
            results.Should().NotBeNull();
            results.Id.Should().Equals("a");
        }

        

        [Fact]
        public async void ExaminationIdNotFoundReturnsNull()
        {
            // Arrange
            var examinationId = "c";
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Id == examinationId;
            var client = CreateDocumentClient(predicate, GenerateExaminations().ToArray());
            var clientFactory = CreateClientFactory(client);

            var connectionSettings = CreateConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            //Act
            var results = await dataAccess.GetItemAsync(connectionSettings.Object, predicate);

            //Assert
            results.Should().BeNull();
        }

        [Fact]
        public void ExaminationQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationRetrievalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new ExaminationRetrievalService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }

        
        IEnumerable<MedicalExaminer.Models.Examination> GenerateExaminations()
        {
            var examination1 = new MedicalExaminer.Models.Examination()
            {
                Id = "a"
            };
            var examination2 = new MedicalExaminer.Models.Examination()
            {
                Id = "b"
            };
            return new []{ examination1, examination2};
        }
        
    }
    public interface IFakeDocumentQuery<T> : IDocumentQuery<T>, IOrderedQueryable<T>
    {

    }
}
