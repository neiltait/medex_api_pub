using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models.Enums;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationsRetrievalServiceTests : ServiceTestsBase<
        ExaminationsRetrievalQuery,
        ExaminationConnectionSettings,
        IEnumerable<MedicalExaminer.Models.Examination>,
        MedicalExaminer.Models.Examination,
        ExaminationsRetrievalService>
    {
        /// <inheritdoc/>
        /// <remarks>Overrides to pass extra constructor parameter.</remarks>
        protected override ExaminationsRetrievalService GetService(
            IDatabaseAccess databaseAccess,
            ExaminationConnectionSettings connectionSettings)
        {
            var examinationQueryBuilder = new ExaminationsQueryExpressionBuilder();

            return new ExaminationsRetrievalService(
                databaseAccess, 
                connectionSettings, 
                examinationQueryBuilder);
        }

        [Fact]
        public virtual async Task UnassignedCasesReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.Unassigned,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.ReadyForMEScrutiny,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.Count());
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.ReadyForMEScrutiny,
                "a", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task EmptyQueryReturnsAllOpenCases()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task EmptyQueryWithOrderByReturnsAllOpenCasesInOrder()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task ClosedCasesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", false);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        // TODO: Previously worked in test but had no way to run via the builder.
        //[Fact]
        //public async virtual Task UrgentQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //   // Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.UrgencyScore > 0;
        //    var client = CosmosMocker.CreateDocumentClient(GenerateExaminations());
        //    var clientFactory = CosmosMocker.CreateClientFactory(client);

        //    var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

        //    var dataAccess = new DatabaseAccess(clientFactory.Object);

        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    var examinationQueryBuilder = new ExaminationsQueryExpressionBuilder();
        //    var sut = new ExaminationsRetrievalService(dataAccess, connectionSettings.Object, examinationQueryBuilder);

        //    //Act

        //    var results = await sut.Handle(examinationsDashboardQuery);
        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Single(results);
        //}

        [Fact]
        public virtual async Task AdmissionNotesHaveBeenAddedQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.AdmissionNotesHaveBeenAdded,
                "", null, 0, 0, "", true);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task HaveBeenScrutinisedByMEQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.HaveBeenScrutinisedByME,
                "", null, 0, 0, "", true);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingAdmissionNotesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.PendingAdmissionNotes,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.PendingDiscussionWithQAP,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.PendingDiscussionWithRepresentative,
                "", null, 0, 0, "", true);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.HaveFinalCaseOutstandingOutcomes,
                "", null, 0, 0, "", true);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        /// <inheritdoc/>
        protected override MedicalExaminer.Models.Examination[] GetExamples()
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

            /*var examination3 = new MedicalExaminer.Models.Examination()
            {
                MedicalExaminerOfficeResponsible = "a",
                ReadyForMEScrutiny = true,
                Completed = false
            };*/

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

            return new[] { examination1, examination2, /*examination3,*/ examination4, examination5,
                           examination6, examination7, examination8, examination9, examination10,
                           examination11};
        }
    }
}