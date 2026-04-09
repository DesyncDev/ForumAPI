using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationEntrypoint).Assembly));   
            
            return services;
        }

        // Referência do mediatr
            public static class ApplicationEntrypoint { }
    }
}