using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Entities;

namespace Forum.Domain.Commom.Entities
{
    public class RefreshToken
    {
        // Constructor
        public RefreshToken(string token, DateTime expires, Guid userId)
        {
            Id = Guid.NewGuid();
            Token = token;
            ExpiresAt = expires;
            UserId = userId;
            IsRevoked = false;
            CreatedAt = DateTime.UtcNow;
        }

        private RefreshToken()
        {}

        // Properties
        public Guid Id { get; init; }
        public string Token { get; private set; } = string.Empty;

        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; init; }

         public bool IsRevoked { get; set; }

         public Guid UserId { get; set; }
         public User User { get; set; } = null!;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive  => !IsRevoked && !IsExpired;
    }
}