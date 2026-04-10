using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Forum.Domain.Commom.Enums;
using Forum.Domain.Entities;
using Xunit;

namespace ForumApi.Tests.Forum.Domain.Commom.Entities
{
    public class UserModelTests
    {
       private readonly Faker _faker = new("pt_BR");

       // Tests
       [Fact]
       [Trait("Domain", "Entities")]
       public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Setup user infos
            var expectedUsername = _faker.Person.FirstName;
            var expectedEmail = _faker.Internet.Email();
            var expectedPasswordhash = Guid.NewGuid().ToString();
            var expectedDisplayName = _faker.Person.FirstName;
            var expectedBio = _faker.Lorem.Lines();

            // Create user with generated infos
            var user = new User(
                expectedUsername,
                expectedEmail,
                expectedPasswordhash,
                expectedDisplayName,
                expectedBio
            );

            // Expected result
            user.Id.Should().NotBeEmpty();
            user.Username.Should().Be(expectedUsername);
            user.Email.Should().Be(expectedEmail);
            user.PasswordHash.Should().Be(expectedPasswordhash);
            user.DisplayName.Should().Be(expectedDisplayName);
            user.Bio.Should().Be(expectedBio);
            user.Role.Should().Be(UserRoles.Member);
            user.IsBanned.Should().BeFalse();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void ChangeUsername_ShouldUpdateUsername()
        {
            var user = CreateValidUser();

            user.ChangeUsername("MyNewUsername");

            user.Username.Should().Be("MyNewUsername");
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void ChangeDisplayName_ShouldUpdateDisplayName()
        {
            var user = CreateValidUser();

            user.ChangeDisplayName("My new displayname");

            user.DisplayName.Should().Be("My new displayname");
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void ChangeBio_ShouldUpdateBio()
        {
            var user = CreateValidUser();

            user.ChangeBio("My New Bio, Vai Corinthians");

            user.Bio.Should().Be("My New Bio, Vai Corinthians");
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void ChangePasswordHash_ShouldUpdatePasswordHash()
        {
            var user = CreateValidUser();

            var newPasswordHash = Guid.NewGuid().ToString();

            user.ChangePasswordHash(newPasswordHash);

            user.PasswordHash.Should().Be(newPasswordHash);
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void SetModeratorRole_ShouldUpdateRole()
        {
            var user = CreateValidUser();

            user.SetModeratorRole();

            user.Role.Should().Be(UserRoles.Moderator);
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void SetAdminRole_ShouldUpdateRole()
        {
            var user = CreateValidUser();

            user.SetAdminRole();

            user.Role.Should().Be(UserRoles.Admin);
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void SetMemberRole_ShouldUpdateRole()
        {
            var user = CreateValidUser();

            user.SetMemberRole();

            user.Role.Should().Be(UserRoles.Member);
        }

        [Fact]
        [Trait("Domain", "Entities")]
        public void SetBannedTrue_ShouldSetIsBannedTrue()
        {
            var user = CreateValidUser();

            user.SetBannedTrue();

            user.IsBanned.Should().BeTrue();
        }
        
        [Fact]
        [Trait("Domain", "Entities")]
        public void SetBannedFalse_ShouldSetIsBannedFalse()
        {
            var user = CreateValidUser();

            user.SetBannedFalse();

            user.IsBanned.Should().BeFalse();
        }

        // Class for create a valid user
        public User CreateValidUser()
        {
            var expectedUsername = _faker.Person.FirstName;
            var expectedEmail = _faker.Internet.Email();
            var expectedPasswordhash = Guid.NewGuid().ToString();
            var expectedDisplayName = _faker.Person.FirstName;
            var expectedBio = _faker.Lorem.Lines();

            var user = new User(
                expectedUsername,
                expectedEmail,
                expectedPasswordhash,
                expectedDisplayName,
                expectedBio
            );

            return user;
        }
    }
}