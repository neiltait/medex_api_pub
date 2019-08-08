using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    /// <summary>
    /// Mapper Users Tests.
    /// </summary>
    public class MapperUsersProfileTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperUsersProfileTests"/> class.
        /// </summary>
        public MapperUsersProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<UsersProfile>(); });
            _mapper = config.CreateMapper();
        }

        /// <summary>
        ///     Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        ///     Test Mapping UserToCreate to GetUserResponse.
        /// </summary>
        [Fact]
        public void TestGetUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser
            {
                UserId = expectedUserId
            };

            var response = _mapper.Map<GetUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test Mapping PostUserRequest to UserToCreate.
        /// </summary>
        [Fact]
        public void TestPostUserRequest()
        {
            var expectedEmail = "testing@methods.co.uk";

            var examination = new PostUserRequest
            {
                Email = expectedEmail
            };

            var response = _mapper.Map<MeUser>(examination);

            response.Email.Should().Be(expectedEmail);
        }

        /// <summary>
        ///     Test Mapping UserToCreate to PostUserResponse.
        /// </summary>
        [Fact]
        public void TestPostUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser
            {
                UserId = expectedUserId
            };

            var response = _mapper.Map<PostUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        ///     Test Mapping PutUserRequest to UserToCreate.
        /// </summary>
        [Fact]
        public void TestPutUserRequest()
        {
            var expectedEmail = "test@methods.co.uk";

            var user = new PutUserRequest
            {
                Email = expectedEmail
            };

            var response = _mapper.Map<MeUser>(user);

            response.Email.Should().Be(expectedEmail);
        }

        /// <summary>
        ///     Test Mapping UserToCreate to PutUserResponse.
        /// </summary>
        [Fact]
        public void TestPutUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser
            {
                UserId = expectedUserId
            };

            var response = _mapper.Map<PutUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        ///     Test Mapping UserToCreate to UserItem.
        /// </summary>
        [Fact]
        public void TestUserItem()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser
            {
                UserId = expectedUserId
            };

            var response = _mapper.Map<UserItem>(examination);

            response.UserId.Should().Be(expectedUserId);
        }
    }
}