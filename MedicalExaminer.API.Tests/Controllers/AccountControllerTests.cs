using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    // @TODO:
    public class AccountControllerTests
    {
        [Fact]
        public async void ValidateSession_ReturnsSession_WhenSessionValid()
        {
        }

        [Fact]
        public async void ValidateSession_ReturnsBadRequest_WhenNoCurrentUser()
        {
        }
    }
}
