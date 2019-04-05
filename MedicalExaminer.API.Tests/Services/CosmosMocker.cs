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

        public static Mock<T> CreateConnectionSettings<T>()
            where T : class, IConnectionSettings
        {
            return new Mock<T>(
                new Mock<Uri>("https://anything.co.uk").Object, "a", "c");
        }

        public static Mock<IDocumentClient> CreateDocumentClient<T>(T[] collectionDocuments)
        {
            var client = new Mock<IDocumentClient>();
            var mockOrderedQueryable = new Mock<IOrderedQueryable<T>>();
            var provider = new Mock<IQueryProvider>();
            var dataSource = collectionDocuments.AsQueryable();

            provider
                .Setup(_ => _.CreateQuery<T>(It.IsAny<Expression>()))
                .Returns((Expression expression) =>
                {
                    var mockDocumentQuery = new Mock<IFakeDocumentQuery<T>>();
                    var query = new EnumerableQuery<T>(expression);
                    var response = new FeedResponse<T>(query);

                    mockDocumentQuery
                        .SetupSequence(_ => _.HasMoreResults)
                        .Returns(true)
                        .Returns(false);

                    mockDocumentQuery
                        .Setup(_ => _.ExecuteNextAsync<T>(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(response);

                    return mockDocumentQuery.Object;
                });

            mockOrderedQueryable.Setup(x => x.Provider).Returns(provider.Object);
            mockOrderedQueryable.Setup(x => x.Expression).Returns(() => dataSource.Expression);

            client.Setup(c => c.CreateDocumentQuery<T>(It.IsAny<Uri>(), It.IsAny<FeedOptions>()))
                .Returns(mockOrderedQueryable.Object);

            return client;
        }
    }

    public interface IFakeDocumentQuery<T> : IDocumentQuery<T>, IOrderedQueryable<T>
    {
    }
}
