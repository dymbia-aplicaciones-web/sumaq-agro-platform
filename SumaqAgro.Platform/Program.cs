using SumaqAgro.Platform.Farming.Domain.Repositories;
using Cortex.Mediator.Commands;
using SumaqAgro.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using SumaqAgro.Platform.Shared.Infrastructure.Interfaces.AspNetCore.OpenApi;
using SumaqAgro.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using SumaqAgro.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using SumaqAgro.Platform.Iam.Infrastructure.Hashing.BCrypt;
using SumaqAgro.Platform.Subscriptions.Domain.Repositories;
using SumaqAgro.Platform.Certification.Domain.Repositories;
using SumaqAgro.Platform.Crop.Domain.Repositories;
using SumaqAgro.Platform.Subscriptions.Application.Internal.QueryServices;
using SumaqAgro.Platform.Subscriptions.Application.QueryServices;
using SumaqAgro.Platform.Subscriptions.Application.CommandServices;
using SumaqAgro.Platform.Certification.Application.Internal.QueryServices;
using SumaqAgro.Platform.Certification.Application.QueryServices;
using SumaqAgro.Platform.Certification.Application.CommandServices;
using SumaqAgro.Platform.Crop.Application.Internal.QueryServices;
using SumaqAgro.Platform.Crop.Application.QueryServices;
using SumaqAgro.Platform.Crop.Application.CommandServices;
using SumaqAgro.Platform.Farming.Application.Internal.QueryServices;
using SumaqAgro.Platform.Farming.Application.QueryServices;
using SumaqAgro.Platform.Farming.Application.CommandServices;
using SumaqAgro.Platform.Profiles.Application.QueryServices;
using SumaqAgro.Platform.Profiles.Application.CommandServices;
using SumaqAgro.Platform.Iam.Application.CommandServices;
using SumaqAgro.Platform.Certification.Application.Internal.CommandServices;
using SumaqAgro.Platform.Certification.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Subscriptions.Application.Internal.CommandServices;
using SumaqAgro.Platform.Subscriptions.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Crop.Application.Internal.CommandServices;
using SumaqAgro.Platform.Crop.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Farming.Application.Internal.CommandServices;
using SumaqAgro.Platform.Farming.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Farming.Infrastructure.AgroMonitoring;
using SumaqAgro.Platform.Farming.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Profiles.Application.Acl;
using SumaqAgro.Platform.Profiles.Application.Internal.CommandServices;
using SumaqAgro.Platform.Profiles.Application.Internal.QueryServices;
using SumaqAgro.Platform.Profiles.Domain.Repositories;
using SumaqAgro.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Profiles.Interfaces.Acl;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Iam.Domain.Repositories;
using SumaqAgro.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Iam.Infrastructure.Tokens.Jwt;
using SumaqAgro.Platform.Iam.Application.Internal.CommandServices;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SumaqAgro.Platform.Shared.Domain.Repositories;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SumaqAgro.Platform.Shared.Interfaces.Rest.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddLocalization();
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();
builder.Services.AddGlobalExceptionHandler();
builder.Services.AddSingleton<ProblemDetailsFactory>();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>()
    .AddOperationTransformer<BearerOperationTransformer>().AddDocumentTransformer((document, _, _) =>
{
    document.Info.Title = "SumaqAgro API";
    return Task.CompletedTask;
}));
builder.Services.AddCortexMediator([typeof(Program)]);
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(ExceptionHandlingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(TransactionCommandBehavior<>));
builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

builder.Services.AddDbContext<AppDbContext>((services, options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Configure ConnectionStrings:DefaultConnection to use MySQL.");

    options.UseMySQL(connectionString).AddInterceptors(services.GetRequiredService<DispatchDomainEventsInterceptor>());
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IHashingService, BCryptHashingService>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<ISignInService, SignInService>();
builder.Services.AddScoped<IProfilesRepository, ProfilesRepository>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();
builder.Services.AddScoped<IProfilesCommandService, ProfilesCommandService>();
builder.Services.AddScoped<IProfilesQueryService, ProfilesQueryService>();
builder.Services.AddScoped<IFarmingRepository, FarmingRepository>();
builder.Services.AddScoped<IFarmingCommandService, FarmingCommandService>();
builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<ICropCommandService, CropCommandService>();
builder.Services.AddScoped<ICropMonitoringService, CropMonitoringService>();
builder.Services.AddScoped<ICertificationRepository, CertificationRepository>();
builder.Services.AddScoped<ICertificationService, CertificationService>();
builder.Services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddHttpClient<IPolygonMonitoring, PolygonMonitoring>(client =>
{
    var baseUrl = builder.Configuration["AgroMonitoring:BaseUrl"] ?? "https://api.agromonitoring.com/agro/1.0/";
    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(10);
}).RemoveAllLoggers();

var configuredKey = builder.Configuration["Jwt:SigningKey"];
var keyBytes = string.IsNullOrWhiteSpace(configuredKey) && builder.Environment.IsDevelopment()
    ? RandomNumberGenerator.GetBytes(32)
    : Convert.FromBase64String(configuredKey ?? throw new InvalidOperationException("Configure Jwt:SigningKey."));
if (keyBytes.Length < 32)
    throw new InvalidOperationException("Jwt:SigningKey must contain at least 32 bytes encoded as Base64.");

var signingKey = new SymmetricSecurityKey(keyBytes);
var expirationMinutes = builder.Configuration.GetValue("Jwt:ExpirationMinutes", 30);
if (expirationMinutes <= 0)
    throw new InvalidOperationException("Jwt:ExpirationMinutes must be positive.");
builder.Services.AddSingleton<ITokenService>(new TokenService(signingKey, expirationMinutes));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = signingKey,
        ValidateIssuerSigningKey = true,
        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IFarmingQueryService, FarmingQueryService>();

builder.Services.AddScoped<ICropQueryService, CropQueryService>();

builder.Services.AddScoped<ICertificationQueryService, CertificationQueryService>();
builder.Services.AddScoped<ITraceabilityQueryService, TraceabilityQueryService>();

builder.Services.AddScoped<ISubscriptionsQueryService, SubscriptionsQueryService>();

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("en").AddSupportedCultures("en", "es").AddSupportedUICultures("en", "es"));

if (app.Environment.IsDevelopment() &&
    !string.IsNullOrWhiteSpace(app.Configuration.GetConnectionString("DefaultConnection")))
{
    await using var scope = app.Services.CreateAsyncScope();
    var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await database.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference().AllowAnonymous();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();
app.Run();
