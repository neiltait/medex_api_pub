using System;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper.Event
{
    public class MapperEventTests
    {
        private static readonly string EventText = "This is a text to test the Event";
        private static readonly EventStatus EventStatus = EventStatus.Final;
        private static readonly string EventId = Guid.NewGuid().ToString();
        private readonly IMapper _mapper;

        public MapperEventTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<OtherEventProfile>(); });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void PostOtherEventRequest_To_EventOther()
        {
            var postOtherEventRequest = new PostOtherEventRequest()
            {
                EventId = EventId,
                EventStatus = EventStatus,
                EventText = EventText
            };

            var response = _mapper.Map<OtherEvent>(postOtherEventRequest);
            response.EventText.Should().Be(EventText);
            response.EventId.Should().Be(EventId);
            response.EventStatus.Should().Be(EventStatus);
        }
    }
}