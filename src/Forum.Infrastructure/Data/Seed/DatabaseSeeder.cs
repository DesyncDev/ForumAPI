// Forum.Infrastructure/Data/Seed/DatabaseSeeder.cs
using Forum.Application.Commom.Interfaces;
using Forum.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        if (await context.users.AnyAsync())
            return;

        // Usuário Admin
        var admin = new User(
            username: "admin",
            email: "admin@forum.com",
            passwordHash: passwordHasher.Hash("Admin123"),
            displayName: "Administrator"
        );
        admin.SetAdminRole(); // Muda para Admin

        // Usuário Moderador
        var moderator = new User(
            username: "moderator",
            email: "moderator@forum.com",
            passwordHash: passwordHasher.Hash("Moderator123"),
            displayName: "Moderator"
        );
        moderator.SetModeratorRole();

        // Usuário Comum
        var user = new User(
            username: "john_doe",
            email: "john@example.com",
            passwordHash: passwordHasher.Hash("User123"),
            displayName: "John Doe"
        );

        await context.users.AddRangeAsync(admin, moderator, user);
        await context.SaveChangesAsync();
    }
}