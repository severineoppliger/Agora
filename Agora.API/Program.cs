using Agora.API.Filters;
using Agora.API.InputValidation;
using Agora.API.InputValidation.Interfaces;
using Agora.API.Orchestrators;
using Agora.API.Orchestrators.Interfaces;
using Agora.Core.BusinessRules;
using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Agora.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
// Configure Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

    builder.Host.UseSerilog();

// Add services to the container.
    builder.Services.AddTransient<LogActionFilter>();
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<LogActionFilter>();
    });
    builder.Services.AddOpenApi();
    builder.Services.AddDbContext<AgoraDbContext>((serviceProvider,opt) =>
    {
        opt.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(10, 11)));
        IHostEnvironment env = serviceProvider.GetRequiredService<IHostEnvironment>();
        if (env.IsDevelopment())
        {
            opt.EnableSensitiveDataLogging();
        }
    });
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
    // Inactivate automatic model validation when entering an action method of a controller
    // This will be taken in charge by LogActionFilter.
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // Repositories
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPostRepository, PostRepository>();
    builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
    builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
    builder.Services.AddScoped<ITransactionStatusRepository, TransactionStatusRepository>();

    // Data validation
    builder.Services.AddScoped<IInputValidator, InputValidator>();
    builder.Services.AddScoped<IBusinessRulesValidationOrchestrator, BusinessRulesValidationOrchestrator>();
    builder.Services.AddScoped<IBusinessRulesValidator, BusinessRulesValidator>();

    // Authentication and authorization
    builder.Services.AddAuthorization();
    builder.Services.AddIdentityApiEndpoints<AppUser>()
        .AddEntityFrameworkStores<AgoraDbContext>();
    
    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1"); });
        app.MapScalarApiReference();

        // Populate the database
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AgoraDbContext>();
            await context.Database.MigrateAsync();
            await AgoraDbContextSeed.SeedDevelopmentDataAsync(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error while populating the database in development environment.");
        }
    }

    app.UseSerilogRequestLogging(); 
    
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    app.MapIdentityApi<AppUser>();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed: {message}", ex.Message);
}
finally
{
    Log.CloseAndFlush();
}