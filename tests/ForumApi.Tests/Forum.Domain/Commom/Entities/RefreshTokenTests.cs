using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Forum.Domain.Commom.Entities;

namespace ForumApi.Tests.Forum.Domain.Commom.Entities
{
    public class RefreshTokenTests
    {
        private readonly Faker _faker = new("pt_BR");

        // Tests
        [Fact]
        [Trait("Domain", "Entities")]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Setup RT infos
            var expectedToken = Guid.NewGuid().ToString();
            var expectedExpiration = _faker.Date.Future();
            var expectedUserId = Guid.NewGuid();

            // Create refreshToken
            var refreshToken = new RefreshToken(
                expectedToken,
                expectedExpiration,
                expectedUserId
            );

            // Expected Result
            refreshToken.Id.Should().NotBeEmpty();
            refreshToken.Token.Should().Be(expectedToken);
            refreshToken.ExpiresAt.Should().Be(expectedExpiration);
            refreshToken.IsRevoked.Should().BeFalse();
            refreshToken.UserId.Should().Be(expectedUserId);
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void IsExpired_WhenExpiresInFuture_ShouldBeFalse()
        {
            var refreshToken = new RefreshToken(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddMinutes(10),
                Guid.NewGuid()
            );

            refreshToken.IsExpired.Should().BeFalse();
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void IsExpired_WhenExpiresInPast_ShouldBeTrue()
        {
            var refreshToken = new RefreshToken(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddMinutes(-10),
                Guid.NewGuid()
            );

            refreshToken.IsExpired.Should().BeTrue();
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void IsActive_WhenNotRevokedAndNotExpired_ShouldBeTrue()
        {
            var refreshToken = new RefreshToken(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddMinutes(10),
                Guid.NewGuid()
            );

            refreshToken.IsActive.Should().BeTrue();
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void IsActive_WhenRevoked_ShouldBeFalse()
        {
            var refreshToken = new RefreshToken(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddMinutes(10),
                Guid.NewGuid()
            );

            refreshToken.IsRevoked = true;

            refreshToken.IsActive.Should().BeFalse();
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void IsActive_WhenExpired_ShouldBeFalse()
        {
            var refreshToken = new RefreshToken(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddMinutes(-10),
                Guid.NewGuid()
            );

            refreshToken.IsActive.Should().BeFalse();
        }
    }
}