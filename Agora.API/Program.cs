using Agora.API.InputValidation;
using Agora.API.InputValidation.Interfaces;
using Agora.API.Orchestrators;
using Agora.API.Orchestrators.Interfaces;
using Agora.Core.BusinessRules;
using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Interfaces;
using Agora.Infrastructure.Data;
using Agora.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AgoraDbContext>(opt =>
{
    opt.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(10, 11))
    );
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionStatusRepository, TransactionStatusRepository>();

// Validation
builder.Services.AddScoped<IInputValidator, InputValidator>();
builder.Services.AddScoped<IBusinessRulesValidationOrchestrator, BusinessRulesValidationOrchestrator>();
builder.Services.AddScoped<IBusinessRulesValidator, BusinessRulesValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/openapi/v1.json","OpenAPI v1");
    });
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    //TODO complete ! 
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

app.Run();
