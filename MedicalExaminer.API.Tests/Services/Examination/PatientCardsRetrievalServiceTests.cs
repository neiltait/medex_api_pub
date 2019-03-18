using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class PatientCardsRetrievalServiceTests
    {
        [Fact]
        public void PatientCardService_Null_Parameter_Throws_ArgumentNullException()
        {
            PatientCardsRetrievalQuery patientCardQuery = null;

            var sut = new PatientCardsRetrievalService();

            Action act = () => sut.Handle(patientCardQuery).GetAwaiter().GetResult();

            act.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void PatientCardService_Returns_()
        {
            PatientCardsRetrievalQuery patientCardQuery = new PatientCardsRetrievalQuery();

            var sut = new PatientCardsRetrievalService();

            Action act = () => sut.Handle(patientCardQuery).GetAwaiter().GetResult();

            act.Should().Throw<ArgumentNullException>();

        }
    }
}
