using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MinhaApi.Services;

namespace ForumApi.Tests.Forum.Infrastructure.Services
{
    public class TokenServiceTests
    {
        private static IConfiguration BuildConfig(
        string key = "super-secret-key-for-tests-only-32chars!",
        string issuer = "test-issuer",
        string audience = "test-audience",
        string accessMinutes = "15",
        string refreshDays = "7")
        {
            var inMemory = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = key,
                ["Jwt:Issuer"] = issuer,
                ["Jwt:Audience"] = audience,
                ["Jwt:AccessTokenExpiresMinutes"] = accessMinutes,
                ["Jwt:RefreshTokenExpiresDays"] = refreshDays
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemory)
                .Build();
        }

        private static User BuildUser() => new(
            username: "testuser",
            email: "test@example.com",
            passwordHash: "hash",
            displayName: "Test User",
            bio: "bio"
        );

        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GenerateAccessToken_ShouldReturnNonEmptyToken()
        {
            var service = new TokenService(BuildConfig());
            var user = BuildUser();

            var (token, _) = service.GenerateAccessToken(user);

            Assert.NotEmpty(token);
        }

        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GenerateAccessToken_ShouldReturnCorrectExpiration()
        {
            var service = new TokenService(BuildConfig(accessMinutes: "15"));
            var user = BuildUser();

            var before = DateTime.UtcNow.AddMinutes(15);
            var (_, expires) = service.GenerateAccessToken(user);
            var after = DateTime.UtcNow.AddMinutes(15);

            Assert.InRange(expires, before, after);
        }

        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GenerateAccessToken_ShouldContainCorrectClaims()
        {
            var service = new TokenService(BuildConfig());
            var user = BuildUser();

            var (token, _) = service.GenerateAccessToken(user);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var sub = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var email = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            Assert.Equal(user.Id.ToString(), sub);
            Assert.Equal(user.Email, email);
            Assert.NotEmpty(jti!);
        }


        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GenerateRefreshToken_ShouldReturnNonEmptyToken()
        {
            var service = new TokenService(BuildConfig());

            var refreshToken = service.GenerateRefreshToken(Guid.NewGuid());

            Assert.NotEmpty(refreshToken.Token);
        }

        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GenerateRefreshToken_ShouldSetCorrectUserId()
        {
            var service = new TokenService(BuildConfig());
            var userId = Guid.NewGuid();

            var refreshToken = service.GenerateRefreshToken(userId);

            Assert.Equal(userId, refreshToken.UserId);
        }

        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GenerateRefreshToken_ShouldReturnCorrectExpiration()
        {
            var service = new TokenService(BuildConfig(refreshDays: "7"));

            var before = DateTime.UtcNow.AddDays(7);
            var refreshToken = service.GenerateRefreshToken(Guid.NewGuid());
            var after = DateTime.UtcNow.AddDays(7);

            Assert.InRange(refreshToken.ExpiresAt, before, after);
        }

        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GetPrincipalFromExpiredToken_WhenTokenIsValid_ShouldReturnPrincipal()
        {
            var service = new TokenService(BuildConfig(accessMinutes: "0"));
            var user = BuildUser();

            var (token, _) = service.GenerateAccessToken(user);
            var principal = service.GetPrincipalFromExpiredToken(token);

            Assert.NotNull(principal);

            var sub = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            Assert.Equal(user.Id.ToString(), sub);
        }

        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GetPrincipalFromExpiredToken_WhenTokenIsInvalid_ShouldReturnNull()
        {
            var service = new TokenService(BuildConfig());

            var principal = service.GetPrincipalFromExpiredToken("token.invalido.aqui");

            Assert.Null(principal);
        }

        [Fact]
        [Trait("Infrastructure", "Services")]
        public void GetPrincipalFromExpiredToken_WhenSignedWithDifferentKey_ShouldReturnNull()
        {
            var serviceA = new TokenService(BuildConfig(key: "chave-a-super-secreta-32-caracteres!"));
            var serviceB = new TokenService(BuildConfig(key: "chave-b-super-secreta-32-caracteres!"));
            var user = BuildUser();

            var (token, _) = serviceA.GenerateAccessToken(user);
            var principal = serviceB.GetPrincipalFromExpiredToken(token);

            Assert.Null(principal);
        }
    }
}