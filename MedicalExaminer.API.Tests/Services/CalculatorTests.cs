using System;
using FluentAssertions;
using MedicalExaminer.Common.Services;
using Xunit;

namespace MedicalExaminer.API.Tests.Services
{
    public class CalculatorTests
    {
        [Fact]
        public void When_Null_Examination_Is_Passed_Throws_Argument_Null_Exception()
        {
            Action act = () => Calculator.CalculateUrgencyScore(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_No_Urgency_Indicators_Are_Selected_Then_The_Urgency_Score_Is_Zero()
        {
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = false,
                CoronerPriority = false,
                CulturalPriority = false,
                FaithPriority = false,
                OtherPriority = false,
            };
            var result = Calculator.CalculateUrgencyScore(examination);
            Assert.Equal(0, result);
        }

        [Fact]
        public void When_All_Urgency_Indicators_Are_Selected_Then_The_Urgency_Score_Is_Five()
        {
            var examination = new MedicalExaminer.Models.Examination
            {
                ChildPriority = true,
                CoronerPriority = true,
                CulturalPriority = true,
                FaithPriority = true,
                OtherPriority = true,
            };
            var result = Calculator.CalculateUrgencyScore(examination);
            Assert.Equal(5, result);
        }
    }
}
