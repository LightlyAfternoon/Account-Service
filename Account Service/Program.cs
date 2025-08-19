using System.Net;
using System.Reflection;
using Account_Service.Exceptions;
using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.AccrueInterest.BackgroundJobs;
using Account_Service.Features.RabbitMQ;
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
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using RabbitMQ.Client;

namespace Account_Service
// ReSharper disable once ArrangeNamespaceBody
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
                        new List<string>()
                    }
                });
            });

            builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
            builder.Services.AddDbContext<ApplicationContext>();
            builder.Services.AddDbContext<HangfireContext>();

            builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
            builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();

            builder.Services.AddScoped<IAccountsService, AccountsService>();
            builder.Services.AddScoped<ITransactionsService, TransactionsService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddScoped<DailyAccrueInterestJobScheduler>();
            builder.Services.AddScoped<RabbitMqDispatcherJobScheduler>();

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

                            await context.Response.WriteAsJsonAsync(
                                new MbResult<string>(status: HttpStatusCode.Unauthorized)
                                {
                                    MbError = context is { Error: not null, ErrorDescription: not null }
                                        ? [$"{context.Error}: {context.ErrorDescription}"]
                                        : ["validation_error: нет JWT токена или он не валиден"]
                                });
                        }
                    };
                });
            builder.Services.AddAuthorizationBuilder();


            builder.Services.AddHangfire(configuration =>
            {
                try
                {
                    configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UsePostgreSqlStorage(options =>
                            options.UseNpgsqlConnection(builder.Configuration["DbSettings:ConnectionString"]));
                }
                catch (PostgresException e)
                {
                    Console.WriteLine(e);
                }
            });
            builder.Services.AddHangfireServer();

            builder.Services.Configure<RabbitMqSettings>(settings =>
            {
                settings.RabbitMqDefaultUser = builder.Configuration["RABBITMQ_DEFAULT_USER"] ?? "guest";
                settings.RabbitMqDefaultPass = builder.Configuration["RABBITMQ_DEFAULT_PASS"] ?? "guest";
            });
            builder.Services.AddSingleton<IRabbitMqService>(sp =>
            {
                var endpoints = new List<AmqpTcpEndpoint>
                {
                    new("rabbitmq", portOrMinusOne: Convert.ToInt32(builder.Configuration["RABBITMQ_DEFAULT_PORT"])),
                    new("localhost", portOrMinusOne: Convert.ToInt32(builder.Configuration["RABBITMQ_DEFAULT_PORT"]))
                };

                var factory = new ConnectionFactory
                {
                    UserName = builder.Configuration["RABBITMQ_DEFAULT_USER"] ?? "guest",
                    Password = builder.Configuration["RABBITMQ_DEFAULT_PASS"] ?? "guest",
                    ClientProvidedName = "app:audit component:event-consumer"
                };

                var rabbitMqConnection =
                    new RabbitMqService(factory, endpoints, sp.GetRequiredService<ILogger<RabbitMqService>>(),
                        sp.GetRequiredService<IServiceScopeFactory>());

                rabbitMqConnection.Connect();

                return rabbitMqConnection;
            });

            builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.Response;
            });

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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Service");
                    c.RoutePrefix = "";
                });
            }

            app.UseCors(policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyMethod();
                policyBuilder.AllowAnyHeader();
            });

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var jobScheduler = scope.ServiceProvider.GetRequiredService<DailyAccrueInterestJobScheduler>();
                    jobScheduler.ScheduleJob();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var jobScheduler = scope.ServiceProvider.GetRequiredService<RabbitMqDispatcherJobScheduler>();
                    jobScheduler.ScheduleJob();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
                }
            }

            app.MapGet("/hangfire", () => "")
                .WithSummary("Hangfire")
                .WithDescription("Hangfire Dashboard");
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = [new DashboardAuthorizationFilter()]
            });

            app.MapGet("/health/live", () =>
                {
                    var logger = app.Services.GetRequiredService<ILogger<Program>>();
                    var connect = app.Services.GetRequiredService<IRabbitMqService>().Connect();

                    if (connect is { IsOpen: true })
                        logger.LogInformation("RabbitMQ connection is OK");
                    else
                        logger.LogError("RabbitMQ connection is not OK");
                }).WithSummary("RabbitMQ Health")
                .WithDescription("Checking RabbitMQ connection");

            app.MapGet("/health/ready", async () =>
                {
                    using var scope = app.Services.CreateScope();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

                    if ((await outboxRepository.FindAllNotProcessed()).Count > 100)
                        logger.LogWarning("More than 100 not processed messages in outbox!");
                    else
                        logger.LogInformation("Less or equal than 100 not processed messages in outbox");
                }).WithSummary("RabbitMQ Outbox")
                .WithDescription("Checking RabbitMQ unprocessed messages in outbox table");

            app.UseHttpLogging();

            app.Run();
        }
    }
}