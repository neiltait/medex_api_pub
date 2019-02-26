﻿using AutoMapper;
using FluentAssertions;
using Medical_Examiner_API.Extensions.Data;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Users;
using Xunit;

namespace Medical_Examiner_API_Tests.Mapper
{
    /// <summary>
    /// Mapper Users Tests
    /// </summary>
    public class MapperUsersProfileTests
    {
        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Setup.
        /// </summary>
        public MapperUsersProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UsersProfile>();
            });

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Test Mapping MeUser to UserItem.
        /// </summary>
        [Fact]
        public void TestUserItem()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser()
            {
                UserId = expectedUserId,
            };

            var response = _mapper.Map<UserItem>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test Mapping MeUser to GetUserResponse.
        /// </summary>
        [Fact]
        public void TestGetUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser()
            {
                UserId = expectedUserId,
            };

            var response = _mapper.Map<GetUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test Mapping MeUser to PutUserResponse.
        /// </summary>
        [Fact]
        public void TestPutUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser()
            {
                UserId = expectedUserId,
            };

            var response = _mapper.Map<PutUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test Mapping MeUser to PostUserResponse.
        /// </summary>
        [Fact]
        public void TestPostUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser()
            {
                UserId = expectedUserId,
            };

            var response = _mapper.Map<PostUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test Mapping PostUserRequest to MeUser.
        /// </summary>
        [Fact]
        public void TestPostUserRequest()
        {
            var expectedFirstName = "expectedFirstName";

            var examination = new PostUserRequest()
            {
                FirstName = expectedFirstName,
            };

            var response = _mapper.Map<MeUser>(examination);

            response.FirstName.Should().Be(expectedFirstName);
        }

        /// <summary>
        /// Test Mapping PutUserRequest to MeUser.
        /// </summary>
        [Fact]
        public void TestPutUserRequest()
        {
            var expectedUserId = "expectedUserId";
            var expectedFirstName = "expectedFirstName";

            var examination = new PutUserRequest()
            {
                UserId = expectedUserId,
                FirstName = expectedFirstName,
            };

            var response = _mapper.Map<MeUser>(examination);

            response.UserId.Should().Be(expectedUserId);
            response.FirstName.Should().Be(expectedFirstName);
        }
    }

}