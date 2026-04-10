using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Forum.Application.Features.Auth.Login.Command;
using Forum.Application.Features.Login.Command;

namespace ForumApi.Tests.Forum.Application.Features.Login
{
    public class LoginCommandValidatorTests
    {
        private readonly LoginCommandValidator _validator = new();
        private readonly Faker _faker = new("pt_BR");

        // Tests
        [Fact]
        [Trait("Features", "Login")]
        public void Email_WhenValid_ShouldBeValid()
        {
            var command = new LoginCommand(_faker.Internet.Email(), _faker.Internet.Password());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        [Trait("Features", "Login")]
        public void Email_WhenEmpty_ShouldBeInvalid()
        {
            var command = new LoginCommand("", _faker.Internet.Password());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        [Trait("Features", "Login")]
        public void Email_WhenBelowMinLength_ShouldBeInvalid()
        {
            var command = new LoginCommand("e@e.e", _faker.Internet.Password());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        [Trait("Features", "Login")]
        public void Email_WhenAboveMaxLength_ShouldBeInvalid()
        {
            var longEmail = new string('a', 250) + "@example.com"; // total ~255

            var command = new LoginCommand(longEmail,
             _faker.Internet.Password());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        [Trait("Features", "Login")]
        public void Email_WhenInvalidFormat_ShouldBeInvalid()
        {
            var command = new LoginCommand("email.email",
             _faker.Internet.Password());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        [Trait("Features", "Login")]
        public void Password_WhenValid_ShouldBeValid()
        {
            var command = new LoginCommand(_faker.Internet.Email(), _faker.Internet.Password());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        [Trait("Features", "Login")]
        public void Password_WhenEmpty_ShouldBeInvalid()
        {
            var command = new LoginCommand(_faker.Internet.Email(), "");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        [Trait("Features", "Login")]
        public void Password_WhenBelowMinLength_ShouldBeInvalid()
        {
            var command = new LoginCommand(_faker.Internet.Email(), "1234567");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        [Trait("Features", "Login")]
        public void Password_WhenAboveMaxLength_ShouldBeInvalid()
        {
            var command = new LoginCommand(_faker.Internet.Email(), Guid.NewGuid().ToString());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }
    }
}