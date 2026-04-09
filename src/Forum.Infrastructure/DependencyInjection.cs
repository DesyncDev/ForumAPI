using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Forum.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(Options =>
                Options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            return services;
        }
    }
}