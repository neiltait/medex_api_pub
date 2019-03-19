using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class ExaminationsRetrievalServiceTests
    {
        private const string id = "expectedExaminationId";
        private const string AltLink = "altLink";
        private const bool AnyImplants = true;
        private const bool AnyPersonalEffects = true;
        private const bool ChildPriority = true;
        private const bool Completed = true;
        private const bool CoronerPriority = true;
        private CoronerStatus CoronerStatus = CoronerStatus.SentAwaitingConfirm;
        private const string County = "Cheshire";
        private const string Country = "England";
        private const bool CulturalPriority = true;
        private DateTime DateOfBirth = new DateTime(1990, 2, 24);
        private DateTime DateOfDeath = new DateTime(2019, 2, 24);
        private const string FuneralDirectors = "funeralDirectors";
        private const bool FaithPriority = true;
        private const string GivenNames = "givenNames";
        private ExaminationGender Gender = ExaminationGender.Male;
        private const string GenderDetails = "genderDetails";
        private const string HospitalNumber_1 = "hospitalNumber_1";
        private const string HospitalNumber_2 = "hospitalNumber_2";
        private const string HospitalNumber_3 = "hospitalNumber_3";
        private const string HouseNameNumber = "houseNameNumber";
        private const string ImplantDetails = "implantDetails";
        private const string LastOccupation = "lastOccupation";
        private const string MedicalExaminerOfficeResponsible = "medicalExaminerOfficeResponsible";
        private ModeOfDisposal ModeOfDisposal = ModeOfDisposal.BuriedAtSea;
        private const string NhsNumber = "123456789";
        private const string OrganisationCareBeforeDeathLocationId = "organisationCareBeforeDeathLocationId";
        private const bool OtherPriority = true;
        private const bool OutOfHours = true;
        private const string PersonalEffectDetails = "personalEffectDetails";
        private const string Postcode = "postcode";
        private const string PlaceDeathOccured = "placeDeathOccured";
        private const string PriorityDetails = "priorityDetails";
        private const string Surname = "surname";
        private const string Street = "street";
        private const string Town = "town";
        private const int UrgencyScore = 4;
        private DateTime CaseCreated = new DateTime(2019, 3, 15);
        private DateTime LastAdmission = new DateTime(2019, 1, 15);
        private TimeSpan TimeOfDeath = new TimeSpan(11, 30, 00);


        [Fact]
        public void NoExaminationsFoundReturnsNull()
        {
            IEnumerable<MedicalExaminer.Models.Examination> examinations = null;
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var examinationQueryBuilder = new Mock<ExaminationQueryBuilder>();
            var query = new Mock<ExaminationsRetrievalQuery>().Object;
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.GetItemsAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
                    x=>true))
                .Returns(Task.FromResult(examinations)).Verifiable();
            var sut = new ExaminationsRetrievalService(dbAccess.Object, connectionSettings.Object, examinationQueryBuilder.Object);
            var expected = default(IEnumerable<MedicalExaminer.Models.Examination>);

            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.GetItemsAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, 
                x=> true), Times.Once);
            Assert.Equal(expected, result.Result);
        }

        [Fact]
        public void ExaminationsQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationsRetrievalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var examinationQueryBuilder = new Mock<ExaminationQueryBuilder>();
            var sut = new ExaminationsRetrievalService(dbAccess.Object, connectionSettings.Object, examinationQueryBuilder.Object);

            Action act = () => sut.Handle(query);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Examinations_Filter_AdmissionNotesHaveBeenAdded_ReturnsCorrectResult()
        {
            var examinations = GenerateExaminations();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var examinationQueryBuilder = new Mock<ExaminationQueryBuilder>();

            var query = new ExaminationsRetrievalQuery(CaseStatus.AdmissionNotesHaveBeenAdded,
                null,
                null,
                1,
                20,
                null,
                true);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.GetItemsAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object, 
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult(examinations)).Verifiable();
            var sut = new ExaminationsRetrievalService(dbAccess.Object, connectionSettings.Object, examinationQueryBuilder.Object);
            
            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.GetItemsAsync<MedicalExaminer.Models.Examination>(connectionSettings.Object,
                It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()), Times.Once);
            
            Assert.Equal(1, result.Result.Count());
            //Assert.Equal(expected, result.Result);
        }

        private IEnumerable<MedicalExaminer.Models.Examination> GenerateExaminations()
        {
            var examination1 = new MedicalExaminer.Models.Examination()
            {
                id = "a",
                TimeOfDeath = TimeOfDeath,
                UrgencyScore = UrgencyScore,
                CaseCreated = CaseCreated,
                HaveBeenScrutinisedByME = false,
                AdmissionNotesHaveBeenAdded = false,
                ReadyForMEScrutiny = false,
                Assigned = false,
                HaveFinalCaseOutstandingOutcomes = false,
                PendingAdmissionNotes = false,
                PendingDiscussionWithQAP = false,
                PendingDiscussionWithRepresentative = false,
            };

            var examination2 = new MedicalExaminer.Models.Examination()
            {
                id = "b",
                TimeOfDeath = TimeOfDeath,
                UrgencyScore = UrgencyScore,
                CaseCreated = CaseCreated,
                HaveBeenScrutinisedByME = true,
                AdmissionNotesHaveBeenAdded = true,
                ReadyForMEScrutiny = true,
                Assigned = true,
                HaveFinalCaseOutstandingOutcomes = true,
                PendingAdmissionNotes = true,
                PendingDiscussionWithQAP = true,
                PendingDiscussionWithRepresentative = true,
            };

            return new []{ examination1, examination2};
        }
    }
}
