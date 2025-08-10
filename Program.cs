using Account_Service.Exceptions;
using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.AccrueInterest.BackgroundJobs;
using Account_Service.Features.Transactions;
using Account_Service.Features.Users;
using Account_Service.Infrastructure;
using Account_Service.Infrastructure.Db;
using Account_Service.Infrastructure.Db.Hangfire;
using Account_Service.Infrastructure.Repositories;
using Account_Service.PipelineBehavior;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;

namespace Account_Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Введите JWT токен авторизации.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
            builder.Services.AddDbContext<ApplicationContext>();
            builder.Services.Configure<HangfireDbSettings>(builder.Configuration.GetSection("HangfireDbSettings"));
            builder.Services.AddDbContext<HangfireContext>();

            builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
            builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IAccountService, AccountsService>();
            builder.Services.AddScoped<ITransactionsService, TransactionsService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddScoped<DailyAccrueInterestJobScheduler>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            // Validation
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddProblemDetails();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["JWT_AUTHORITY"];
                    options.Audience = "account-service-api";
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuers = new List<string>
                        {
                            "http://localhost:7080/realms/account-service",
                            "http://keycloak:7080/realms/account-service"
                        }
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                            await context.Response.WriteAsJsonAsync(new MbResult<string>
                            {
                                Status = HttpStatusCode.Unauthorized,
                                MbError = [$"{context.Error}: {context.ErrorDescription}"]
                            });
                        }
                    };
                });
            builder.Services.AddAuthorizationBuilder();

            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(builder.Configuration["HangfireDbSettings:ConnectionString"])));
            builder.Services.AddHangfireServer();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HangfireContext>();
                dbContext.Database.Migrate();
            }

            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyMethod();
                policyBuilder.AllowAnyHeader();
            });

            using (var scope = app.Services.CreateScope())
            {
                var jobScheduler = scope.ServiceProvider.GetRequiredService<DailyAccrueInterestJobScheduler>();
                jobScheduler.ScheduleJob();
            }

            app.MapGet("/hangfire", () => "")
                .WithSummary("Веб-интерфейс Hangfire")
                .WithDescription("Предназначен для более удобного обзора и управления фоновыми задачами");
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = [new DashboardAuthorizationFilter()]
            });

            app.Run();
        }
    }
}