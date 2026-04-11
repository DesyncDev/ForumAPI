using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Forum.Application.Features.Auth.Register.Command;
using Forum.Application.Features.Register.Command;

namespace ForumApi.Tests.Forum.Application.Features.Register
{
    public class RegisterCommandValidationTests
    {
        private readonly RegisterCommandValidator _validator = new();
        private readonly Faker _faker = new("pt_BR");

        // Tests
        [Fact]
        [Trait("Features", "Register")]
        public void Command_WhenValid_ShouldBeValid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedDisplayName = _faker.Person.FullName;
            var expectedEmail = _faker.Internet.Email();
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                expectedUsername,
                expectedDisplayName,
                expectedEmail,
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Username_WhenEmpty_ShouldBeInvalid()
        {
            var expectedDisplayName = _faker.Person.FullName;
            var expectedEmail = _faker.Internet.Email();
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                "",
                expectedDisplayName,
                expectedEmail,
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Username_WhenBelowMinLength_ShouldBeInvalid()
        {
            var expectedDisplayName = _faker.Person.FullName;
            var expectedEmail = _faker.Internet.Email();
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                "test",
                expectedDisplayName,
                expectedEmail,
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Username_WhenAboveMaxLength_ShouldBeInvalid()
        {
            var expectedDisplayName = _faker.Person.FullName;
            var expectedEmail = _faker.Internet.Email();
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                new string('a', 32),
                expectedDisplayName,
                expectedEmail,
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void DisplayName_WhenEmpty_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedEmail = _faker.Internet.Email();
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                expectedUsername,
                "",
                expectedEmail,
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "DisplayName");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void DisplayName_WhenBelowMinLength_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedEmail = _faker.Internet.Email();
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                expectedUsername,
                "aaaa",
                expectedEmail,
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "DisplayName");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void DisplayName_WhenAboveMaxLength_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedEmail = _faker.Internet.Email();
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                expectedUsername,
                new string('a', 82),
                expectedEmail,
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "DisplayName");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Email_WhenEmpty_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedDisplayName = _faker.Person.FullName;
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                expectedUsername,
                expectedDisplayName,
                "",
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Email_WhenBelowMinLength_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedDisplayName = _faker.Person.FullName;
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                expectedUsername,
                expectedDisplayName,
                "test@a.b",
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Email_WhenAboveMaxLength_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedDisplayName = _faker.Person.FullName;
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                expectedUsername,
                expectedDisplayName,
                new string('a', 250) + "@example.com",
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Email_WhenInvalidFormat_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedDisplayName = _faker.Person.FullName;
            var expectedPassword = _faker.Internet.Password();
            var command = new RegisterCommand(
                expectedUsername,
                expectedDisplayName,
                "email.test",
                expectedPassword
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Password_WhenEmpty_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedDisplayName = _faker.Person.FullName;
            var expectedEmail = _faker.Internet.Email();
            var command = new RegisterCommand(
                expectedUsername,
                expectedDisplayName,
                expectedEmail,
                ""
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Password_WhenBelowMinLength_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedDisplayName = _faker.Person.FullName;
            var expectedEmail = _faker.Internet.Email();
            var command = new RegisterCommand(
                expectedUsername,
                expectedDisplayName,
                expectedEmail,
                "1234567"
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        [Trait("Features", "Register")]
        public void Password_WhenAboveMaxLength_ShouldBeInvalid()
        {
            var expectedUsername = _faker.Internet.UserName();
            var expectedDisplayName = _faker.Person.FullName;
            var expectedEmail = _faker.Internet.Email();
            var command = new RegisterCommand(
                expectedUsername,
                expectedDisplayName,
                expectedEmail,
                new string('a', 34)
            );

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }
    }
}