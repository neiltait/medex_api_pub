using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace MedicalExaminer.API.Tests.Services
{
    public static class CosmosMocker
    {
        public static Mock<IDocumentClientFactory> CreateClientFactory(Mock<IDocumentClient> client)
        {
            var clientFactory = new Mock<IDocumentClientFactory>();
            clientFactory.Setup(x => x.CreateClient(It.IsAny<IConnectionSettings>()))
                .Returns(client.Object);

            return clientFactory;
        }

        public static Mock<ExaminationConnectionSettings> CreateExaminationConnectionSettings() =>
            CreateConnectionSettings<ExaminationConnectionSettings>();

        public static Mock<T> CreateConnectionSettings<T>()
            where T : class, IConnectionSettings
        {
            return new Mock<T>(
                new Mock<Uri>("https://anything.co.uk").Object, "a", "c");
        }

        public static Mock<IDocumentClient> CreateDocumentClient<T>(Expression<Func<T, bool>> predicate, T[] collectionDocuments)
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

        public static Mock<IDocumentClient> CreateDocumentClient<T>(Expression<Func<T, bool>> predicate, T[] collectionDocuments, Expression<Func<T, bool>> orderBy)
        {
            IQueryable<T> dataSource = collectionDocuments.AsQueryable();

            var expected = dataSource.Where(predicate).OrderBy(orderBy);

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


    }

    public interface IFakeDocumentQuery<T> : IDocumentQuery<T>, IOrderedQueryable<T>
    {

    }
}
