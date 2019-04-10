using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models;
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
        /// <inheritdoc/>
        /// <remarks>Overrides to pass extra constructor parameter.</remarks>
        protected override ExaminationsDashboardService GetService(
            IDatabaseAccess databaseAccess,
            ExaminationConnectionSettings connectionSettings)
        {
            var examinationQueryBuilder = new ExaminationsQueryExpressionBuilder();

            return new ExaminationsDashboardService(
                databaseAccess,
                connectionSettings,
                examinationQueryBuilder);
        }

        [Fact]
        public virtual async Task UnassignedCasesReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(MedicalExaminer.Models.Enums.CaseStatus.Unassigned,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(MedicalExaminer.Models.Enums.CaseStatus.ReadyForMEScrutiny,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfReadyForMEScrutiny);
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(MedicalExaminer.Models.Enums.CaseStatus.ReadyForMEScrutiny,
                "a", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfReadyForMEScrutiny);
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
            Assert.Equal(9, results.TotalCases);
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
            Assert.Equal(1, results.TotalCases);
        }

        // TODO: Urgency not queryable from current interface.
        //[Fact]
        //public virtual async Task UrgentQueryReturnsCorrectCount()
        //{
        //    //Arrange
        //    var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
        //        "", null, 0, 0, "", true);

        //    //Act
        //    var results = await Service.Handle(examinationsDashboardQuery);

        //    //Assert
        //    results.Should().NotBeNull();
        //    Assert.Equal(1, results.CountOfUrgentCases);
        //}

        [Fact]
        public virtual async Task AdmissionNotesHaveBeenAddedQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task HaveBeenScrutinisedByMEQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task PendingAdmissionNotesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

        [Fact]
        public virtual async Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 0, 0, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.CountOfUrgentCases);
        }

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
