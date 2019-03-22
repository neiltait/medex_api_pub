using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        [Fact]
        public async void Create()
        {
            var description = "BBB";
            var expected = new List<MedicalExaminer.Models.Examination> {
            new MedicalExaminer.Models.Examination{ Id = description },
            new MedicalExaminer.Models.Examination{ Id = "ZZZ" },
            new MedicalExaminer.Models.Examination{ Id = "AAA" },
            new MedicalExaminer.Models.Examination{ Id = "CCC" },

        }.AsQueryable();
            var response = new FeedResponse<MedicalExaminer.Models.Examination>(expected);

            var mockDocumentQuery = new Mock<IFakeDocumentQuery<MedicalExaminer.Models.Examination>>();
            mockDocumentQuery
                .SetupSequence(_ => _.HasMoreResults)
                .Returns(true)
                .Returns(false);

            mockDocumentQuery
                .Setup(_ => _.ExecuteNextAsync<MedicalExaminer.Models.Examination>(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(response);

            var client = new Mock<IDocumentClient>();

            client
                .Setup(_ => _.CreateDocumentQuery<MedicalExaminer.Models.Examination>(It.IsAny<Uri>(), It.IsAny<FeedOptions>()))
                .Returns(mockDocumentQuery.Object);

            var cosmosDatabase = string.Empty;

            var clientFactory = new Mock<IDocumentClientFactory>();
            clientFactory.Setup(x => x.CreateClient(It.IsAny<IConnectionSettings>()))
                .Returns(client.Object);

            var connectionSettings = new Mock<ExaminationConnectionSettings>(new Mock<Uri>("https://anything.co.uk").Object, "a", "c").Object;

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            //var provider = new Mock<IQueryProvider>();
            //provider
            //    .Setup(_ => _.CreateQuery<MedicalExaminer.Models.Examination>(It.IsAny<System.Linq.Expressions.Expression>()))
            //    .Returns((Expression expression) => {
            //        if (expression != null)
            //        {
            //            dataSource = dataSource.Provider.CreateQuery<MedicalExaminer.Models.Examination>(expression);
            //        }
            //        mockDocumentQuery.Object;
            //    });

            //mockDocumentQuery.As<IQueryable<MedicalExaminer.Models.Examination>>().Setup(x => x.Provider).Returns(provider.Object);
            //mockDocumentQuery.As<IQueryable<MedicalExaminer.Models.Examination>>().Setup(x => x.Expression).Returns(() => dataSource.Expression);
            //mockDocumentQuery.As<IQueryable<MedicalExaminer.Models.Examination>>().Setup(x => x.ElementType).Returns(() => dataSource.ElementType);
            //mockDocumentQuery.As<IQueryable<MedicalExaminer.Models.Examination>>().Setup(x => x.GetEnumerator()).Returns(() => dataSource.GetEnumerator());


            var qq = client.Object.CreateDocumentQuery<MedicalExaminer.Models.Examination>(
                UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId,
                            connectionSettings.Collection),
                        new FeedOptions { MaxItemCount = -1 })
                    .Where(x => x.Id == "ZZZ")
                    .AsDocumentQuery();
            //Act
            var results = new List<MedicalExaminer.Models.Examination>();
            while (qq.HasMoreResults)
            {
                results.AddRange(await qq.ExecuteNextAsync<MedicalExaminer.Models.Examination>());
            }

            //var actual = documentsRepository.ExecuteQueryAsync(query);

            //Assert
            // actual.Should().BeEquivalentTo(expected);

        }


        [Fact]
        public async virtual Task Test_GetBooksById()
        {
            //Arrange
            var id = "HarryPotter";
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Id == id;
            IQueryable<MedicalExaminer.Models.Examination> dataSource = new List<MedicalExaminer.Models.Examination> {
        new MedicalExaminer.Models.Examination { Id = "HarryPotter"},
        new MedicalExaminer.Models.Examination { Id = "HarryPotter2"}
    }.AsQueryable();

            var expected = dataSource.Where(predicate);

            var response = new FeedResponse<MedicalExaminer.Models.Examination>(expected);

            var mockDocumentQuery = new Mock<IFakeDocumentQuery<MedicalExaminer.Models.Examination>>();

            mockDocumentQuery
                .SetupSequence(_ => _.HasMoreResults)
                .Returns(true)
                .Returns(false);

            mockDocumentQuery
                .Setup(_ => _.ExecuteNextAsync<MedicalExaminer.Models.Examination>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //var wrappedMockQuery = new MockEnumerableQuery(mockDocumentQuery.Object);

            var provider = new Mock<IQueryProvider>();
            provider
                .Setup(_ => _.CreateQuery<MedicalExaminer.Models.Examination>(It.IsAny<Expression>()))
                .Returns((Expression expression) =>
                {
                    // return dataSource.Provider.CreateQuery<MedicalExaminer.Models.Examination>(expression);
                    //}
                    return mockDocumentQuery.Object;
                });


            //var provider = new Mock<IQueryProvider>();
            //provider
            //    .Setup(_ => _.CreateQuery<MedicalExaminer.Models.Examination>(It.IsAny<System.Linq.Expressions.Expression>()))
            //    .Returns((Expression expression) => {
            //        if (expression != null)
            //        {
            //            dataSource = dataSource.Provider.CreateQuery<Book>(expression);
            //        }
            //        mockDocumentQuery.Object;
            //    });


            mockDocumentQuery.As<IQueryable<MedicalExaminer.Models.Examination>>().Setup(x => x.Provider).Returns(provider.Object);
            mockDocumentQuery.As<IQueryable<MedicalExaminer.Models.Examination>>().Setup(x => x.Expression).Returns(() => dataSource.Expression);
            mockDocumentQuery.As<IQueryable<MedicalExaminer.Models.Examination>>().Setup(x => x.ElementType).Returns(() => dataSource.ElementType);
            mockDocumentQuery.As<IQueryable<MedicalExaminer.Models.Examination>>().Setup(x => x.GetEnumerator()).Returns(dataSource.GetEnumerator());

            var client = new Mock<IDocumentClient>();

            client.Setup(_ => _.CreateDocumentQuery<MedicalExaminer.Models.Examination>(It.IsAny<Uri>(), It.IsAny<FeedOptions>()))
                  .Returns(mockDocumentQuery.Object);

            var clientFactory = new Mock<IDocumentClientFactory>();
            clientFactory.Setup(x => x.CreateClient(It.IsAny<IConnectionSettings>()))
                .Returns(client.Object);

            var connectionSettings = new Mock<ExaminationConnectionSettings>(new Mock<Uri>("https://anything.co.uk").Object, "a", "c").Object;

            var dataAccess = new DatabaseAccess(clientFactory.Object);


            var documentsRepository = new DatabaseAccess(clientFactory.Object);

            //Act
            var entities = await dataAccess.GetItemAsync(connectionSettings, predicate);

            //Assert
            //entities.Should()
            //    .NotBeNullOrEmpty()
            //    .And.BeEquivalentTo(expected);
        }

        [Fact]
        public void ExaminationIdNotFoundReturnsNull()
        {
            // Arrange
            var examinationId = "a";
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new Mock<ExaminationRetrievalQuery>(examinationId);
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null)).Verifiable();
            var sut = new ExaminationRetrievalService(dbAccess.Object, connectionSettings.Object);
            var expected = default(MedicalExaminer.Models.Examination);

            // Act
            var result = sut.Handle(query.Object);

            // Assert
            dbAccess.Verify(db => db.GetItemAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
                It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()), Times.Once);

            Assert.Equal(expected, result.Result);

        }

        [Fact]
        public void ExaminationIdIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationRetrievalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new ExaminationRetrievalService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LocationIdFoundReturnsResult()
        {
            //var examinationId = "a";
            //var examination = new MedicalExaminer.Models.Examination();
            //var connectionSettings = new Mock<IExaminationConnectionSettings>();
            //var query = new Mock<ExaminationRetrievalQuery>(examinationId);
            //var documentClientFactory = new Mock<IDocumentClientFactory>();




            //var dbAccess = new DatabaseAccess(documentClientFactory.Object);




            //dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
            //        It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
            //    .Returns(Task.FromResult(examination)).Verifiable();
            //var sut = new ExaminationRetrievalService(dbAccess, connectionSettings.Object);
            //var expected = examination;

            //// Act
            //var result = sut.Handle(query.Object);

            //// Assert
            //dbAccess.Verify(db => db.GetItemAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
            //    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()), Times.Once);
            //Assert.Equal(expected, result.Result);
        }



        

        public interface IFakeDocumentQuery<T> : IDocumentQuery<T>, IOrderedQueryable<T> 
        {

        }


        internal class MockEnumerableQuery : IDocumentQuery<MedicalExaminer.Models.Examination>, IOrderedQueryable<MedicalExaminer.Models.Examination>
        {
            public IQueryable<MedicalExaminer.Models.Examination> List;
            private readonly bool bypassExpressions;


            public MockEnumerableQuery(EnumerableQuery<MedicalExaminer.Models.Examination> List, bool bypassExpressions = true)
            {
                this.List = List;
                this.bypassExpressions = bypassExpressions;
            }


            public IEnumerator<MedicalExaminer.Models.Examination> GetEnumerator()
            {
                return List.GetEnumerator();
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }


            public Expression Expression => List.Expression;

            public Type ElementType => typeof(MedicalExaminer.Models.Examination);


            public IQueryProvider Provider => new MockQueryProvider(this, bypassExpressions);


            public void Dispose()
            {
            }


            public Task<FeedResponse<TResult>> ExecuteNextAsync<TResult>(CancellationToken token = new CancellationToken())
            {
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                FeedResponse<MedicalExaminer.Models.Examination> feed = Activator.CreateInstance(typeof(FeedResponse<MedicalExaminer.Models.Examination>),
                    flags, null, new Object[] { List.Select(j => j), 0, new NameValueCollection(), false, null }, null)
                    as FeedResponse<MedicalExaminer.Models.Examination>;

                return Task.FromResult(feed as FeedResponse<TResult>);
            }


            public Task<FeedResponse<dynamic>> ExecuteNextAsync(CancellationToken token = new CancellationToken())
            {
                throw new NotImplementedException();
            }


            public bool HasMoreResults { get; }
        }

        class MockQueryProvider : IQueryProvider
        {
            private readonly MockEnumerableQuery mockQuery;
            private readonly bool bypassExpressions;

            public MockQueryProvider(MockEnumerableQuery mockQuery, bool byPassExpressions)
            {
                this.mockQuery = mockQuery;
                this.bypassExpressions = byPassExpressions;
            }


            public IQueryable CreateQuery(Expression expression)
            {
                throw new NotImplementedException();
            }


            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                if (!bypassExpressions)
                {
                    mockQuery.List = mockQuery.List.Provider.CreateQuery<TElement>(expression) as IQueryable<MedicalExaminer.Models.Examination>;
                }

                return (IQueryable<TElement>)mockQuery;
            }


            public object Execute(Expression expression)
            {
                throw new NotImplementedException();
            }


            public TResult Execute<TResult>(Expression expression)
            {
                throw new NotImplementedException();
            }
        }
    }



    //public async virtual Task Test_GetEntitiesAsyncBySQL()
    //{
    //    //Arrange
    //    var id = "100";
    //    string queryString = "SELECT * FROM c WHERE c.ID = " + id;
    //    var dataSource = new List<Book> {
    //    new Book { ID = "100", Title = "abc"}
    //}.AsQueryable();

    //    Expression<Func<Book, bool>> predicate = t => t.ID == id;
    //    var expected = dataSource.Where(predicate.Compile());
    //    var response = new FeedResponse<Book>(expected);

    //    var mockDocumentQuery = new Mock<IFakeDocumentQuery<Book>>();

    //    mockDocumentQuery
    //        .SetupSequence(_ => _.HasMoreResults)
    //        .Returns(true)
    //        .Returns(false);

    //    mockDocumentQuery
    //        .Setup(_ => _.ExecuteNextAsync<Book>(It.IsAny<CancellationToken>()))
    //        .ReturnsAsync(response);

    //    //Note the change here
    //    mockDocumentQuery.As<IQueryable<Book>>().Setup(_ => _.Provider).Returns(dataSource.Provider);
    //    mockDocumentQuery.As<IQueryable<Book>>().Setup(_ => _.Expression).Returns(dataSource.Expression);
    //    mockDocumentQuery.As<IQueryable<Book>>().Setup(_ => _.ElementType).Returns(dataSource.ElementType);
    //    mockDocumentQuery.As<IQueryable<Book>>().Setup(_ => _.GetEnumerator()).Returns(() => dataSource.GetEnumerator());

    //    var client = new Mock<IDocumentClient>();

    //    //Note the change here
    //    client
    //        .Setup(_ => _.CreateDocumentQuery<Book>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<FeedOptions>()))
    //        .Returns(mockDocumentQuery.Object);

    //    var documentsRepository = new DocumentDBRepository<Book>(client.Object, "100", "100");

    //    //Act
    //    var entities = await documentsRepository.RunQueryAsync(queryString);

    //    //Assert
    //    entities.Should()
    //        .NotBeNullOrEmpty()
    //        .And.BeEquivalentTo(expected);
    //}

}
