using DailyMed.Core.Interfaces;
using DailyMed.Infrastructure.Data;
using DailyMed.Infrastructure.FDA;
using DailyMed.Infrastructure.StaticFiles;
using DailyMed.Infrastructure.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services)
        {          
            services.AddTransient<IIndicationsParser, IndicationsParser>();
            services.AddTransient<ICopayParserJson, CopayParserJson>();
            services.AddTransient<IFileReader, FileReader>();
            services.AddTransient<IDrugIndicationsRepository, DrugIndicationsRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddHttpClientApiUrl(this IServiceCollection services, IConfiguration configuration)
        {
            // Register a named HttpClient for DailyMed
            services.AddHttpClient("DailyMed", client =>
            {
                client.BaseAddress = new Uri("https://dailymed.nlm.nih.gov/dailymed/services/v2/");
            });

            // Register a named HttpClient for fdaApi
            services.AddHttpClient("fdaApi", client =>
            {
                client.BaseAddress = new Uri("https://api.fda.gov/drug/");
            });


            // Register a named HttpClient for OpenAI
            services.AddHttpClient("APIOpenAI", client =>
            {
                client.BaseAddress = new Uri("https://api.openai.com/v1/");
            });

            return services;

        }
    }
}
