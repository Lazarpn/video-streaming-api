using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using NLog.Web;
using SendGrid.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using BrandedGames.Entities;
using BrandedGames.Data;
using BrandedGames.Api.Authentication;
using BrandedGames.Api.ActionResults;
using BrandedGames.Common.Middleware;
using BrandedGames.Api.Controllers;
using BrandedGames.Entities.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using BrandedGames.Api.Helpers;
using BrandedGames.Core;

Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("App: Starting Application");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    logger.Info("App: Configuring identity");
    builder.Services
        .AddIdentity<User, Role>()
        .AddEntityFrameworkStores<BrandedGamesDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Policies.NotConfirmedEmail, policy => policy.Requirements.Add(new NotConfirmedEmailRequirement()));
        options.AddPolicy(Policies.EmailConfirmed, policy => policy.Requirements.Add(new EmailConfirmedRequirement()));
        options.AddPolicy(Policies.RegisteredUser, policy => policy.Requirements.Add(new RegisteredUserRequirement()));
        options.AddPolicy(Policies.AdministratorUser, policy => policy.Requirements.Add(new AdministratorUserRequirement()));
    });

    logger.Info("App: Configuring authorization policies");
    builder.Services.AddScoped<IAuthorizationHandler, NotConfirmedEmailHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, EmailConfirmedHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, RegisteredUserHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, AdministratorUserHandler>();

    logger.Info("App: Configuring services");
    builder.Services.AddScoped<FeatureManager>();
    builder.Services.AddScoped<GameTypeManager>();
    builder.Services.AddScoped<PlatformTypeManager>();
    builder.Services.AddScoped<GameFormManager>();
    builder.Services.AddScoped<CloudinaryFileManager>();

    logger.Info("App: Configuring forwarded headers");
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });


    logger.Info("App: Configuring CORS");
    var allowedDomains = builder.Configuration["allowedDomains"].Split(',');

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            policyBuilder =>
            {
                policyBuilder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("refreshed-token")
                    .AllowCredentials()
                    .WithOrigins(allowedDomains);
                policyBuilder.SetPreflightMaxAge(TimeSpan.FromSeconds(600));
            });
    });

    logger.Info("App: Configuring AutoMapper");
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    logger.Info("App: Configuring DB Connections");
    builder.Services.AddDbContext<BrandedGamesDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("BrandedGamesDb")));

    logger.Info("App: Configuring web api / controllers");
    builder.Services.AddMemoryCache();

    logger.Info("App: Configuring SignalR");


    builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.Culture = new CultureInfo("en-GB");
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

    logger.Info("App: Configuring form options");
    builder.Services.Configure<FormOptions>(options =>
    {
        options.ValueLengthLimit = 52428800; // 50MB
        options.MultipartBodyLengthLimit = 52428800; // 50MB
    });

    logger.Info("App: Configuring validations");
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = _ => new ValidationProblemDetailsResult();
    });

    logger.Info("App: Configuring NLog DI");
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    logger.Info("App: Configuring Swagger");
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "BrandedGames API", Version = "v1" });
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Standard Authorization header using the Bearer scheme. Note: Keyword 'bearer' will be automatically prepended.",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>();
        options.DescribeAllParametersInCamelCase();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    });


    WebApplication app = builder.Build();

    logger.Info("App: Middleware pipeline start");
    if (builder.Environment.IsProduction())
    {
        app.UseHsts();
    }

    var cultureInfo = new CultureInfo("en-GB");
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

    app.UseStatusCodePages();

    app.UseStaticFiles();

    app.UseRouting();
    app.UseCors("CorsPolicy");

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<SessionManagementMiddleware>();

    bool shouldShowSwagger = bool.Parse(builder.Configuration["showSwagger"]);
    if (shouldShowSwagger)
    {
        app.UseSwagger();
        app.UseSwaggerUI(uiOptions =>
        {
            uiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "BrandedGames API V1");
            uiOptions.InjectStylesheet("/swagger-ui/styles.css");
        });
    }

    //StudyStreamDbContext.SeedData(dbContext, userManager, roleManager).Wait();

    if (builder.Environment.IsProduction())
    {
        app.UseHttpsRedirection();
    }

    app.MapControllers().RequireCors("CorsPolicy");

    logger.Info("App: Middleware pipeline end");

    logger.Info("App: Adding SignalR endpoints");

    app.Run();
    logger.Info("App: Running the Application");
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "App: Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
