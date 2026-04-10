using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.Interfaces;

namespace Forum.Infrastructure.Commom.Hasher
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}