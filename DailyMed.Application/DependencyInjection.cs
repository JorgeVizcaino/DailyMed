using DailyMed.Application.Services;
using DailyMed.Core.Applications;
using DailyMed.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace DailyMed.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {          
            services.AddTransient<IDrugIndicationsApplication, DrugIndicationsApplication>();
            services.AddTransient<ICopayProgramApplication, CopayProgramApplication>();
            services.AddTransient<ILoginUserApplication, LoginUserApplication>();
            
            return services;
        }

    }
}
