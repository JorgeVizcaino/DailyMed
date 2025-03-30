using DailyMed.Core.Interfaces;
using Microsoft.OpenApi.Models;
using DailyMed.Infrastructure.Data;
using DailyMed.Infrastructure.AI;
using DailyMed.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace DailyMed.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var service = builder.Services.BuildServiceProvider();
            var configuration = service.GetService<IConfiguration>();

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<ICopayProgramRepository, CopayProgramRepository>();
            builder.Services.AddScoped<IDailyMedRepository, DailyMedRepository>();

            builder.Services.AddScoped<IGenerativeAIService, OpenAIApiService>();

            builder.Services.AddHttpClientApiUrl(configuration);
            builder.Services.AddInfraestructure();
            builder.Services.AddApplication();

            // 1. Read from config
            var jwtSection = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSection.GetValue<string>("SecretKey");

            // 3. Add Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // dev only
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),                    
                    RoleClaimType = ClaimTypes.Role
                };
            });


            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                // a) Add the "Bearer" security definition
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter ‘Bearer’ [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer abc123\""
                });

                // b) Add a global security requirement
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
