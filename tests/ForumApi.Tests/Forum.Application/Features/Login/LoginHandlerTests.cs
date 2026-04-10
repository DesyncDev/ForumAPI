using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Forum.Application.Commom.Interfaces;
using Forum.Application.Commom.Interfaces.Repositories;
using Forum.Application.Commom.OneOf;
using Forum.Application.Commom.OneOf.Errors;
using Forum.Application.Features.Auth.Login.Command;
using Forum.Application.Features.Login.Command;
using Forum.Domain.Commom.Entities;
using Forum.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ForumApi.Tests.Forum.Application.Features.Login
{
    public class LoginHandlerTests
    {
        // Faker
        private readonly Faker _faker = new("pt_BR");

        // Mocks
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _rtRepo;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _token;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _userRepo = Substitute.For<IUserRepository>();
            _rtRepo = Substitute.For<IRefreshTokenRepository>();
            _hasher = Substitute.For<IPasswordHasher>();
            _token = Substitute.For<ITokenService>();
            _handler = new LoginHandler(
                _uow,
                _userRepo,
                _hasher,
                _token,
                _rtRepo
            );
        }

        // Tests
        [Fact]
        [Trait("Features", "Login")]
        public async Task Handle_WhenCredentialsAreValid_ShouldReturnLoginResponse()
        {
            // Arrange
            var expectedEmail = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var expectedToken = _faker.Random.AlphaNumeric(32);
            var expectedExpires = DateTime.UtcNow.AddMinutes(15);


            var command = new LoginCommand(
                expectedEmail,
                password
            );

            var user = new User(
                _faker.Internet.UserName(),
                expectedEmail,
                Guid.NewGuid().ToString(),
                _faker.Person.FullName,
                _faker.Lorem.Lines()
            );

            var expectedRefreshToken = new RefreshToken(_faker.Random.AlphaNumeric(64),
            DateTime.UtcNow.AddDays(7), user.Id);

            _userRepo.GetUserByEmailAsync(expectedEmail, Arg.Any<CancellationToken>())
                .Returns(user);

            _hasher.Verify(password, user.PasswordHash)
                .Returns(true);

            _token.GenerateAccessToken(user)
                .Returns((expectedToken, expectedExpires));

            _token.GenerateRefreshToken(user.Id)
                .Returns(expectedRefreshToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsT0.Should().BeTrue();
            var success = result.AsT0;
            success.Should().BeOfType<LoginResponse>();
            success.AccessToken.Should().Be(expectedToken);
            success.RefreshToken.Should().Be(expectedRefreshToken.Token);
            success.AccessTokenExpires.Should().Be(expectedExpires);
        }

        [Fact]
        [Trait("Features", "Login")]
        public async Task Handle_WhenUserNotFound_ShouldReturnInvalidCredentialsError()
        {
            // Arrange
            var expectedEmail = _faker.Internet.Email();
            var password = _faker.Internet.Password();

            var command = new LoginCommand(
                expectedEmail,
                password
            );

            _userRepo.GetUserByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            var result =await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsT1.Should().BeTrue();
            var error = result.AsT1;
            error.Should().BeOfType<InvalidCredentialsError>();
        }

        [Fact]
        [Trait("Features", "Login")]
        public async Task Handle_WhenPasswordIsWrong_ShouldReturnInvalidCredentialsError()
        {
            // Arrange
            var expectedEmail = _faker.Internet.Email();
            var password = _faker.Internet.Password();

            var command = new LoginCommand(
                expectedEmail,
                password
            );

            var user = new User(
                _faker.Internet.UserName(),
                expectedEmail,
                Guid.NewGuid().ToString(),
                _faker.Person.FullName,
                _faker.Lorem.Lines()
            );

            _userRepo.GetUserByEmailAsync(expectedEmail, Arg.Any<CancellationToken>())
                .Returns(user);

            _hasher.Verify(password, user.PasswordHash)
                .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
        
            // Assert
            result.IsT1.Should().BeTrue();
            var error = result.AsT1;
            error.Should().BeOfType<InvalidCredentialsError>();
        }
    }
}