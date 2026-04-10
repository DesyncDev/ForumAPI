using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.Application.Commom.Interfaces;
using Forum.Application.Commom.Interfaces.Repositories;
using Forum.Infrastructure.Commom.Hasher;
using Forum.Infrastructure.Data;
using Forum.Infrastructure.Data.Seed;
using Forum.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MinhaApi.Services;

namespace Forum.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(Options =>
                Options.UseNpgsql(connectionString)
            );

            // Services
            services.AddScoped<ITokenService, TokenService>();

            // Hasher
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Jwt
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),

                        ValidateIssuer  = true,
                        ValidIssuer     = configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience    = configuration["Jwt:Audience"],

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}