using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Commom.Entities;
using Forum.Domain.Commom.Enums;

namespace Forum.Domain.Entities
{
    public class User
    {
        // Constructor
        public User(string username, string email, string passwordHash,
            string displayName)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            DisplayName = displayName;
            Role = UserRoles.Member;
            IsBanned = false;
            CreatedAt = DateTime.UtcNow;
        }

        // Properties
        public Guid Id { get; init; }
        public string Username { get; private set; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string DisplayName { get; private set; } = string.Empty;
        public string? Bio { get; private set; } = string.Empty;
        public UserRoles Role { get; private set; }
        public bool IsBanned { get; private set; }
        public DateTime CreatedAt { get; init; }

        // Collections
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        = new List<RefreshToken>();

        // Methods
        public void ChangeUsername(string username)
        => Username = username;

        public void ChangePasswordHash(string hash)
        => PasswordHash = hash;

        public void ChangeDisplayName(string displayName)
        => DisplayName = displayName;

        public void ChangeBio(string bio)
        => Bio = bio;

        public void SetMemberRole()
        => Role = UserRoles.Member;

        public void SetModeratorRole()
        => Role = UserRoles.Moderator;

        public void SetAdminRole()
        => Role = UserRoles.Admin;

        public void SetBannedTrue()
        => IsBanned = true;

        public void SetBannedFalse()
        => IsBanned = false;
    }
}