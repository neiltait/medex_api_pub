using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.Azure.Documents.Client;
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
            ExaminationConnectionSettings connectionSettings,
            ICosmosStore<MedicalExaminer.Models.Examination> cosmosStore = null)
        {
            var store = CosmosMocker.CreateCosmosStore(GetExamples());
            var examinationQueryBuilder = new ExaminationsQueryExpressionBuilder();

            return new ExaminationsRetrievalService(
                databaseAccess,
                connectionSettings,
                examinationQueryBuilder,
                store.Object);
        }

        [Fact]
        public virtual async Task UnassignedCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.Unassigned,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.ReadyForMEScrutiny,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.ReadyForMEScrutiny,
                "a",
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task EmptyQueryReturnsAllOpenCases()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                null,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public virtual async Task EmptyQueryWithOrderByReturnsAllOpenCasesInOrder()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                null,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public virtual async Task ClosedCasesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                null,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                false);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task AdmissionNotesHaveBeenAddedQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.AdmissionNotesHaveBeenAdded,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task HaveBeenScrutinisedByMEQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.HaveBeenScrutinisedByME,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingAdmissionNotesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.PendingAdmissionNotes,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public virtual async Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.PendingDiscussionWithQAP,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public virtual async Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.PendingDiscussionWithRepresentative,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public virtual async Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                CaseStatus.HaveFinalCaseOutstandingOutcomes,
                string.Empty,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        /// <inheritdoc/>
        protected override MedicalExaminer.Models.Examination[] GetExamples()
        {
            var examination1 = new MedicalExaminer.Models.Examination()
            {
                Unassigned = true,
                CaseCompleted = false
            };

            var examination2 = new MedicalExaminer.Models.Examination()
            {
                ReadyForMEScrutiny = true,
                CaseCompleted = false
            };

            var examination3 = new MedicalExaminer.Models.Examination()
            {
                MedicalExaminerOfficeResponsible = "a",
                ReadyForMEScrutiny = true,
                CaseCompleted = false
            };

            var examination4 = new MedicalExaminer.Models.Examination()
            {
                CaseCompleted = true
            };

            var examination5 = new MedicalExaminer.Models.Examination()
            {
                CaseCompleted = false,
                UrgencyScore = 3
            };

            var examination6 = new MedicalExaminer.Models.Examination()
            {
                CaseCompleted = false,
                AdmissionNotesHaveBeenAdded = true
            };

            var examination7 = new MedicalExaminer.Models.Examination()
            {
                CaseCompleted = false,
                PendingDiscussionWithQAP = true
            };

            var examination8 = new MedicalExaminer.Models.Examination()
            {
                CaseCompleted = false,
                PendingDiscussionWithRepresentative = true
            };

            var examination9 = new MedicalExaminer.Models.Examination()
            {
                CaseCompleted = false,
                HaveFinalCaseOutcomesOutstanding = true
            };

            var examination10 = new MedicalExaminer.Models.Examination()
            {
                CaseCompleted = false,
                HaveBeenScrutinisedByME = true
            };

            var examination11 = new MedicalExaminer.Models.Examination()
            {
                CaseCompleted = false,
                PendingAdmissionNotes = true
            };

            return new[]
            {
                examination1,
                examination2,
                examination3,
                examination4,
                examination5,
                examination6,
                examination7,
                examination8,
                examination9,
                examination10,
                examination11
            };
        }
    }
}