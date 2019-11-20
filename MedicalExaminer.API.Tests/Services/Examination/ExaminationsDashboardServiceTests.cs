using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmonaut;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationsDashboardServiceTests : ServiceTestsBase<
        ExaminationsRetrievalQuery,
        ExaminationConnectionSettings,
        ExaminationsOverview,
        MedicalExaminer.Models.Examination,
        ExaminationsDashboardService>
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ExaminationsQueryExpressionBuilder>();

            var store = CosmosMocker.CreateCosmosStore(GetExamples());
            services.AddTransient<ICosmosStore<MedicalExaminer.Models.Examination>>(s => store.Object);

            base.ConfigureServices(services);
        }

        [Fact]
        public virtual async Task UnassignedCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), MedicalExaminer.Models.Enums.CaseStatus.Unassigned,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                PermissedLocations(),
                MedicalExaminer.Models.Enums.CaseStatus.ReadyForMEScrutiny,
                string.Empty,
                null,
                0,
                0,
                string.Empty,
                OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfReadyForMEScrutiny);
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        {
            // Arrange
            var permissedLocations = new[] { "expectedLocation" };
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                permissedLocations,
                MedicalExaminer.Models.Enums.CaseStatus.ReadyForMEScrutiny,
                "expectedLocation",
                null,
                0,
                0,
                string.Empty,
                OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfReadyForMEScrutiny);
        }

        [Fact]
        public virtual async Task EmptyQueryReturnsAllOpenCases()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.TotalCases);
        }

        [Fact]
        public virtual async Task ClosedCasesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.ClosedOrVoid);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(3, results.TotalCases);
        }

        [Fact]
        public virtual async Task UrgentQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task AdmissionNotesHaveBeenAddedQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task HaveBeenScrutinisedByMEQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task PendingAdmissionNotesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 0, 0, "", OpenClosedCases.Open);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
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
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination2 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination2",
                ReadyForMEScrutiny = true,
                CaseCompleted = false,
                SiteLocationId = "expectedLocation",
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination4 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination4",
                CaseCompleted = true,
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination5 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination5",
                CaseCompleted = false,
                CreatedAt = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(5)),
                IsVoid = false,
            };

            var examination6 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination6",
                CaseCompleted = false,
                AdmissionNotesHaveBeenAdded = true,
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination7 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination7",
                CaseCompleted = false,
                PendingDiscussionWithQAP = true,
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination8 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination8",
                CaseCompleted = false,
                PendingDiscussionWithRepresentative = true,
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination9 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination9",
                CaseCompleted = false,
                HaveFinalCaseOutcomesOutstanding = true,
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination10 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination10",
                CaseCompleted = false,
                HaveBeenScrutinisedByME = true,
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination11 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination11",
                CaseCompleted = false,
                PendingAdmissionNotes = true,
                CreatedAt = dateTimeNow,
                IsVoid = false,
            };

            var examination12 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination12",
                CaseCompleted = false,
                PendingAdmissionNotes = true,
                CreatedAt = dateTimeNow,
                IsVoid = true,
            };

            var examination13 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination13",
                CaseCompleted = true,
                PendingAdmissionNotes = true,
                CreatedAt = dateTimeNow,
                IsVoid = true,
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
                examination11,
                examination12,
                examination13,
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
