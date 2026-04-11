using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Forum.Application.Commom.Interfaces;
using Forum.Application.Commom.Interfaces.Repositories;
using Forum.Application.Commom.OneOf.Errors;
using Forum.Application.Features.Auth.Register.Command;
using Forum.Application.Features.Register.Command;
using Forum.Domain.Entities;
using NSubstitute;

namespace ForumApi.Tests.Forum.Application.Features.Register
{
    public class RegisterHandlerTests
    {
        // Faker
        private readonly Faker _faker = new("pt_BR");

        // Mocks
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _hasher;
        private readonly IUnitOfWork _uow;
        private readonly RegisterHandler _handler;

        public RegisterHandlerTests()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _hasher = Substitute.For<IPasswordHasher>();
            _uow = Substitute.For<IUnitOfWork>();
            _handler = new RegisterHandler(_userRepo, _hasher, _uow);
        }

        // Tests
        [Fact]
        [Trait("Features", "Register")]
        public async Task Handle_WhenCredentialsAreValid_ReturnRegisterResponse()
        {
            var command = new RegisterCommand(
                _faker.Internet.UserName(),
                _faker.Person.FullName,
                _faker.Internet.Email(),
                _faker.Internet.Password()
            );

            _userRepo.CheckUserByUsernameAsync(command.Username, Arg.Any<CancellationToken>())
                .Returns(false);
            _userRepo.CheckUserByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(false);

            User? capturedUser = null;
            _userRepo.Add(Arg.Do<User>(u => capturedUser = u));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsT0.Should().BeTrue();
            var success = result.AsT0;
            capturedUser!.Email.Should().Be(command.Email.Trim().ToLower());
            success.Id.Should().NotBeEmpty();
            success.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        [Trait("Features", "Register")]
        public async Task Handle_WhenUsernameAlreadyExists_ShouldReturnUsernameAlreadyExistError()
        {
            var command = new RegisterCommand(
                _faker.Internet.UserName(),
                _faker.Person.FullName,
                _faker.Internet.Email(),
                _faker.Internet.Password()
            );

            _userRepo.CheckUserByUsernameAsync(command.Username, Arg.Any<CancellationToken>())
                .Returns(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsT1.Should().BeTrue();
            var error = result.AsT1;
            error.Should().BeOfType<UsernameAlreadyExistError>();
        }

        [Fact]
        [Trait("Features", "Register")]
        public async Task Handle_WhenEmailAlreadyExists_ShouldReturnEmailAlreadyExistError()
        {
            var command = new RegisterCommand(
                _faker.Internet.UserName(),
                _faker.Person.FullName,
                _faker.Internet.Email(),
                _faker.Internet.Password()
            );

            _userRepo.CheckUserByUsernameAsync(command.Username, Arg.Any<CancellationToken>())
                .Returns(false);
            _userRepo.CheckUserByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(true);

            var result = await _handler.Handle(command, CancellationToken.None);
        
            result.IsT1.Should().BeTrue();
            var error = result.AsT1;
            error.Should().BeOfType<EmailAlreadyExistError>();
        }
    }
}