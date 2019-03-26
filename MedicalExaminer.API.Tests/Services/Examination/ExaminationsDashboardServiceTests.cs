using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationsDashboardServiceTests
    {
        [Fact]
        public void ExaminationDashboardQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationsRetrievalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new ExaminationsDashboardService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public async virtual Task UnassignedCasesReturnsCorrectCount()
        {
            //Arrange
            var id = "a";
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Unassigned;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(MedicalExaminer.Models.Enums.CaseStatus.Unassigned,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);
                        
            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public async virtual Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.ReadyForMEScrutiny;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(MedicalExaminer.Models.Enums.CaseStatus.ReadyForMEScrutiny,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(2, results.CountOfReadyForMEScrutiny);
        }

        [Fact]
        public async virtual Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> pred1 = t => t.ReadyForMEScrutiny;
            Expression<Func<MedicalExaminer.Models.Examination, bool>> pred2 = e => e.MedicalExaminerOfficeResponsible == "a";

            var predicate = pred1.And(pred2);

            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(MedicalExaminer.Models.Enums.CaseStatus.ReadyForMEScrutiny,
                "a", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();

            Assert.Equal(1, results.CountOfReadyForMEScrutiny);
        }

        [Fact]
        public async virtual Task EmptyQueryReturnsAllOpenCases()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Completed == false;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.TotalCases);
        }

        [Fact]
        public async virtual Task ClosedCasesQueryReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.Completed;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.TotalCases);
        }

        [Fact]
        public async virtual Task UrgentQueryReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.UrgencyScore>0;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public async virtual Task AdmissionNotesHaveBeenAddedQueryReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.AdmissionNotesHaveBeenAdded;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public async virtual Task HaveBeenScrutinisedByMEQueryReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.HaveBeenScrutinisedByME;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public async virtual Task PendingAdmissionNotesQueryReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.PendingAdmissionNotes;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public async virtual Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.PendingDiscussionWithQAP;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public async virtual Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.PendingDiscussionWithRepresentative;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public async virtual Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        {
            //Arrange
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.HaveFinalCaseOutstandingOutcomes;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);

            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            var sut = new ExaminationsDashboardService(dataAccess, connectionSettings.Object);


            //Act

            var results = await sut.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        private MedicalExaminer.Models.Examination[] GenerateExaminations()
        {
            var examination1 = new MedicalExaminer.Models.Examination()
            {
                Unassigned = true,
                Completed = false
            };

            var examination2 = new MedicalExaminer.Models.Examination()
            {
                ReadyForMEScrutiny = true,
                Completed = false
            };

            var examination3 = new MedicalExaminer.Models.Examination()
            {
                MedicalExaminerOfficeResponsible = "a",
                ReadyForMEScrutiny = true,
                Completed = false
            };

            var examination4 = new MedicalExaminer.Models.Examination()
            {
                Completed = true
            };

            var examination5 = new MedicalExaminer.Models.Examination()
            {
                Completed = false,
                UrgencyScore = 3
            };

            var examination6 = new MedicalExaminer.Models.Examination()
            {
                Completed = false,
                AdmissionNotesHaveBeenAdded = true
            };

            var examination7 = new MedicalExaminer.Models.Examination()
            {
                Completed = false,
                PendingDiscussionWithQAP = true
            };

            var examination8 = new MedicalExaminer.Models.Examination()
            {
                Completed = false,
                PendingDiscussionWithRepresentative = true
            };

            var examination9 = new MedicalExaminer.Models.Examination()
            {
                Completed = false,
                HaveFinalCaseOutstandingOutcomes = true
            };

            var examination10 = new MedicalExaminer.Models.Examination()
            {
                Completed = false,
                HaveBeenScrutinisedByME = true
            };

            var examination11 = new MedicalExaminer.Models.Examination()
            {
                Completed = false,
                PendingAdmissionNotes = true
            };

            return new[] { examination1, examination2, examination3, examination4, examination5,
                           examination6, examination7, examination8, examination9, examination10,
                           examination11};
        }

        
    }
}
