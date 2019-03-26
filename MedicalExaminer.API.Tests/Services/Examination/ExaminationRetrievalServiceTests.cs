﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationRetrievalServiceTests
    {
        [Fact]
        public async virtual Task ExaminationIdFoundReturnsExpectedExamination()
        {
            //Arrange
            var id = "a";
            Expression<Func<MedicalExaminer.Models.Examination, bool>> predicate = t => t.ExaminationId == id;
            var client = CosmosMocker.CreateDocumentClient(predicate, GenerateExaminations().ToArray());
            var clientFactory = CosmosMocker.CreateClientFactory(client);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);
            var sut = new ExaminationRetrievalService(dataAccess, connectionSettings.Object);
            //Act
            
            var result = await sut.Handle(new ExaminationRetrievalQuery(id));

            //Assert
            result.Should().NotBeNull();
            Assert.Equal("a", result.ExaminationId);
        }

        [Fact]
        public async void ExaminationIdNotFoundReturnsNull()
        {
            // Arrange
            var examinationId = "a";
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var mockUser = new Mock<MeUser>();
            var query = new Mock<ExaminationRetrievalQuery>(examinationId, mockUser.Object);
            var dbAccess = new Mock<IDatabaseAccess>();

            dbAccess.Setup(db => db.GetItemAsync(connectionSettings.Object,
                    It.IsAny<Expression<Func<MedicalExaminer.Models.Examination, bool>>>()))
                .Returns(Task.FromResult<MedicalExaminer.Models.Examination>(null)).Verifiable();
            var sut = new ExaminationRetrievalService(dbAccess.Object, connectionSettings.Object);
            var expected = default(MedicalExaminer.Models.Examination);

            var connectionSettings = CosmosMocker.CreateExaminationConnectionSettings();

            var dataAccess = new DatabaseAccess(clientFactory.Object);
            var sut = new ExaminationRetrievalService(dataAccess, connectionSettings.Object);
            
            //Act
            var results = await sut.Handle(new ExaminationRetrievalQuery(examinationId));
            
            //Assert
            results.Should().BeNull();
        }

        [Fact]
        public void ExaminationQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            ExaminationRetrievalQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new ExaminationRetrievalService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query).GetAwaiter().GetResult();
            act.Should().Throw<ArgumentNullException>();
        }

        IEnumerable<MedicalExaminer.Models.Examination> GenerateExaminations()
        {
            var examination1 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "a"
            };
            var examination2 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "b"
            };
            return new []{ examination1, examination2};
        }
        
    }
    
}
