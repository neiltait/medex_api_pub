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
using Microsoft.Extensions.DependencyInjection;
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
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddTransient<ExaminationsQueryExpressionBuilder>();

            var store = CosmosMocker.CreateCosmosStore(GetExamples());
            services.AddTransient<ICosmosStore<MedicalExaminer.Models.Examination>>(s => store.Object);
        }

        [Fact]
        public virtual async Task UnassignedCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.Unassigned,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.ReadyForMEScrutiny,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.Count());
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        {
            // Arrange
            var permissedLocations = new[] { "expectedLocation" };
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                permissedLocations,
                CaseStatus.ReadyForMEScrutiny,
                "expectedLocation",
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = (await Service.Handle(examinationsDashboardQuery)).ToList();

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task EmptyQueryReturnsAllOpenCases()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task EmptyQueryWithOrderByReturnsAllOpenCasesInOrder()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task EmptyQueryWithOrderByUrgency_ReturnsAllOpenCasesInOrder()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                PermissedLocations(),
                null,
                "",
                ExaminationsOrderBy.Urgency,
                1,
                10,
                "",
                true);

            // Act
            var results = (await Service.Handle(examinationsDashboardQuery)).ToList();

            // Assert
            results.Should().NotBeNull();
            results.Count().Should().Be(9);

            results.ElementAt(0).ExaminationId.Should().Be("examination9");
            results.ElementAt(1).ExaminationId.Should().Be("examination8");
            results.ElementAt(2).ExaminationId.Should().Be("examination7");
            results.ElementAt(3).ExaminationId.Should().Be("examination6");
            results.ElementAt(4).ExaminationId.Should().Be("examination5");
            results.ElementAt(5).ExaminationId.Should().Be("examination11");
            results.ElementAt(6).ExaminationId.Should().Be("examination10");
            results.ElementAt(7).ExaminationId.Should().Be("examination2");
            results.ElementAt(8).ExaminationId.Should().Be("examination1");
        }

        [Fact]
        public virtual async Task ClosedCasesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 1, 10, "", false);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingAdditionalDetailsQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.PendingAdditionalDetails,
                "", null, 1, 10, "", true);

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
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.HaveBeenScrutinisedByME,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task HaveUnknownBasicDetailsQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.HaveUnknownBasicDetails,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.PendingDiscussionWithQAP,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.PendingDiscussionWithRepresentative,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.HaveFinalCaseOutstandingOutcomes,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task FilterByLocationsReturnsCorrectCases()
        {
            // Arrange
            const string expectedLocationId = "expectedLocation";
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                PermissedLocations(),
                null,
                expectedLocationId,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = (await Service.Handle(examinationsDashboardQuery)).ToList();

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(4, results.Count());
        }

        [Fact]
        public virtual async Task FilterByLocationsReturnsCorrectCasesWhenNoFilter()
        {
            // Arrange
            const string expectedLocationId = "";
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                PermissedLocations(),
                null,
                expectedLocationId,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = (await Service.Handle(examinationsDashboardQuery)).ToList();

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        /// <inheritdoc/>
        protected override MedicalExaminer.Models.Examination[] GetExamples()
        {
            var dateTimeNow = DateTime.Now;

            var examination1 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination1",
                Unassigned = true,
                CaseCompleted = false,
                NationalLocationId = "expectedLocation",
                CreatedAt = dateTimeNow,
                PendingAdditionalDetails = false,
            };

            var examination2 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination2",
                ReadyForMEScrutiny = true,
                CaseCompleted = false,
                RegionLocationId = "expectedLocation",
                CreatedAt = dateTimeNow.Subtract(TimeSpan.FromDays(1)),
                PendingAdditionalDetails = false,
            };

            var examination4 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination4",
                CaseCompleted = true,
                PendingAdditionalDetails = false,
            };

            var examination5 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination5",
                CaseCompleted = false,
                TrustLocationId = "expectedLocation",
                CreatedAt = dateTimeNow,
                OtherPriority = true,
                PendingAdditionalDetails = false,
            };

            var examination6 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination6",
                CaseCompleted = false,
                PendingAdditionalDetails = true,
                SiteLocationId = "expectedLocation",
                OtherPriority = true,
                CreatedAt = dateTimeNow.Subtract(TimeSpan.FromDays(1)),
            };

            var examination7 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination7",
                CaseCompleted = false,
                PendingDiscussionWithQAP = true,
                OtherPriority = true,
                CreatedAt = dateTimeNow.Subtract(TimeSpan.FromDays(6)),
                PendingAdditionalDetails = false,
            };

            var examination8 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination8",
                CaseCompleted = false,
                PendingDiscussionWithRepresentative = true,
                OtherPriority = true,
                CreatedAt = dateTimeNow.Subtract(TimeSpan.FromDays(7)),
                PendingAdditionalDetails = false,
            };

            var examination9 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination9",
                CaseCompleted = false,
                HaveFinalCaseOutcomesOutstanding = true,
                OtherPriority = true,
                CreatedAt = dateTimeNow.Subtract(TimeSpan.FromDays(8)),
                PendingAdditionalDetails = false,
            };

            var examination10 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination10",
                CaseCompleted = false,
                HaveBeenScrutinisedByME = true,
                CreatedAt = dateTimeNow.Subtract(TimeSpan.FromDays(2)),
                PendingAdditionalDetails = false,
            };

            var examination11 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination11",
                CaseCompleted = false,
                PendingAdmissionNotes = true,
                CreatedAt = dateTimeNow.Subtract(TimeSpan.FromDays(3)),
                HaveUnknownBasicDetails = true,
                PendingAdditionalDetails = false,
            };

            var examinations = new[]
            {
                examination1,
                examination2,
                examination4,
                examination5,
                examination6,
                examination7,
                examination8,
                examination9,
                examination10,
                examination11
            };

            SetSiteLocationIdOnExaminations(examinations);
            UpdateUrgencySortOnExaminations(examinations);

            return examinations;
        }

        private static void SetSiteLocationIdOnExaminations(MedicalExaminer.Models.Examination[] examinations)
        {
            foreach (var examination in examinations)
            {
                if (examination.SiteLocationId == null)
                {
                    examination.SiteLocationId = "site1";
                }
            }
        }

        private static void UpdateUrgencySortOnExaminations(MedicalExaminer.Models.Examination[] examinations)
        {
            foreach (var examination in examinations)
            {
                examination.UpdateCaseUrgencySort(1);
            }
        }

        private IEnumerable<string> PermissedLocations()
        {
            return new[] { "site1", "expectedLocation" };
        }
    }
}