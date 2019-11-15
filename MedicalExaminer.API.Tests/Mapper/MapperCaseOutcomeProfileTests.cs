using System;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    public class MapperCaseOutcomeProfileTests
    {
        private readonly IMapper _mapper;

        public MapperCaseOutcomeProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<CaseOutcomeProfile>(); });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Examination_To_PutConfirmationOfScrutinyResponse()
        {
            var scrutinyConfirmedDate = new DateTime(2019, 5, 3);

            var caseOutcome = new CaseOutcome
            {
                ScrutinyConfirmedOn = scrutinyConfirmedDate
            };

            var examination = new Examination
            {
                CaseOutcome = caseOutcome,
                ConfirmationOfScrutinyCompletedAt = scrutinyConfirmedDate
            };

            var result = _mapper.Map<PutConfirmationOfScrutinyResponse>(examination);

            result.ScrutinyConfirmedOn.Should().Be(scrutinyConfirmedDate);
        }

        [Fact]
        public void PutOutstandingCaseItemsRequest_To_CaseOutcome_When_CremationFormStatus_Is_Yes()
        {
            var putOutstandingCaseItemsRequest = new PutOutstandingCaseItemsRequest
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                WaiveFee = true,
                GpNotifiedStatus = GPNotified.GPNotified
            };

            var outstandingCaseItems = _mapper.Map<CaseOutcome>(putOutstandingCaseItemsRequest);

            outstandingCaseItems.MccdIssued.Should().BeTrue();
            outstandingCaseItems.CremationFormStatus.Should().Be(CremationFormStatus.Yes);
            outstandingCaseItems.WaiveFee.Should().BeTrue();
            outstandingCaseItems.GpNotifiedStatus.Should().Be(GPNotified.GPNotified);
        }

        [Fact]
        public void PutOutstandingCaseItemsRequest_To_CaseOutcome_When_CremationFormStatus_Is_No()
        {
            var putOutstandingCaseItemsRequest = new PutOutstandingCaseItemsRequest
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.No,
                WaiveFee = true,
                GpNotifiedStatus = GPNotified.GPNotified
            };

            var outstandingCaseItems = _mapper.Map<CaseOutcome>(putOutstandingCaseItemsRequest);

            outstandingCaseItems.MccdIssued.Should().BeTrue();
            outstandingCaseItems.CremationFormStatus.Should().Be(CremationFormStatus.No);
            outstandingCaseItems.WaiveFee.Should().BeNull();
            outstandingCaseItems.GpNotifiedStatus.Should().Be(GPNotified.GPNotified);
        }

        [Fact]
        public void PutOutstandingCaseItemsRequest_To_CaseOutcome_When_CremationFormStatus_Is_Unknown()
        {
            var putOutstandingCaseItemsRequest = new PutOutstandingCaseItemsRequest
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Unknown,
                WaiveFee = true,
                GpNotifiedStatus = GPNotified.GPNotified
            };

            var outstandingCaseItems = _mapper.Map<CaseOutcome>(putOutstandingCaseItemsRequest);

            outstandingCaseItems.MccdIssued.Should().BeTrue();
            outstandingCaseItems.CremationFormStatus.Should().Be(CremationFormStatus.Unknown);
            outstandingCaseItems.WaiveFee.Should().BeNull();
            outstandingCaseItems.GpNotifiedStatus.Should().Be(GPNotified.GPNotified);
        }
    }
}
