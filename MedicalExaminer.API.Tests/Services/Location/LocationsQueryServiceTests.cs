using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services.Location;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Location
{
    public class LocationsQueryServiceTests
    {

        [Fact]
        public void LocationsQueryServiceIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<ILocationConnectionSettings>();
            const LocationsRetrievalByQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new LocationsQueryService(dbAccess.Object, connectionSettings.Object);

            // Act
            Action act = () => sut.Handle(query).GetAwaiter().GetResult();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public virtual async Task Handle_ReturnsAllResults_WhenNoFilterApplied()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Location, bool>> predicate = t => true;
            var client = CosmosMocker.CreateDocumentClient(predicate, GetExampleLocations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);
            var dataAccess = new DatabaseAccess(clientFactory.Object);
            var connectionSettings = CosmosMocker.CreateConnectionSettings<LocationConnectionSettings>();
            var sut = new LocationsQueryService(dataAccess, connectionSettings.Object);

            var query = new LocationsRetrievalByQuery(null, null);

            //Act
            var results = (await sut.Handle(query)).ToList();

            //Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(50);
        }

        [Fact]
        public virtual async Task Handle_ReturnsFiltered_WhenFilteredByName()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Location, bool>> predicate = t => true;
            var client = CosmosMocker.CreateDocumentClient(predicate, GetExampleLocations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);
            var dataAccess = new DatabaseAccess(clientFactory.Object);
            var connectionSettings = CosmosMocker.CreateConnectionSettings<LocationConnectionSettings>();
            var sut = new LocationsQueryService(dataAccess, connectionSettings.Object);

            var query = new LocationsRetrievalByQuery("Name2", null);

            //Act
            var results = (await sut.Handle(query)).ToList();

            //Assert
            results.Should().NotBeNull();
            results.Count.Should().Be(2);
        }

        //[Fact]
        //public async virtual Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.ReadyForMEScrutiny;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(MedicalExaminer.Models.Enums.CaseStatus.ReadyForMEScrutiny,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Equal(2, results.Count());
        //}

        //[Fact]
        //public async virtual Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> pred1 = t => t.ReadyForMEScrutiny;
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> pred2 = e => e.MedicalExaminerOfficeResponsible == "a";

        //    var predicate = pred1.And(pred2);

        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(MedicalExaminer.Models.Enums.CaseStatus.ReadyForMEScrutiny,
        //        "a", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();

        //    Assert.Single(results);
        //}

        //[Fact]
        //public async virtual Task EmptyQueryReturnsAllOpenCases()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Completed == false;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Equal(10, results.Count());
        //}

        //[Fact]
        //public async virtual Task EmptyQueryWithOrderByReturnsAllOpenCasesInOrder()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Completed == false;
        //    //   Expression<Func<MedicalExaminer.Models.Examination, bool>> order = t => t.UrgencyScore;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Equal(10, results.Count());
        //}

        //[Fact]
        //public async virtual Task ClosedCasesQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Completed;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        //[Fact]
        //public async virtual Task UrgentQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.UrgencyScore > 0;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);
        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        //[Fact]
        //public async virtual Task AdmissionNotesHaveBeenAddedQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.AdmissionNotesHaveBeenAdded;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        //[Fact]
        //public async virtual Task HaveBeenScrutinisedByMEQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.HaveBeenScrutinisedByME;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        //[Fact]
        //public async virtual Task PendingAdmissionNotesQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.PendingAdmissionNotes;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);
        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        //[Fact]
        //public async virtual Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.PendingDiscussionWithQAP;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);
        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        //[Fact]
        //public async virtual Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.PendingDiscussionWithRepresentative;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        //[Fact]
        //public async virtual Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.HaveFinalCaseOutstandingOutcomes;
        //    var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new Mock<ExaminationsQueryExpressionBuilder>();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder.Object);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        private MedicalExaminer.Models.Location[] GetExampleLocations()
        {
            const int start = 1;
            return Enumerable.Range(start, 50).Select(i => new MedicalExaminer.Models.Location
            {
                Name = $"Name{i}",
                ParentId = i > start ? $"Name{(i-1)}" : null
            }).ToArray();
        }
    }
}