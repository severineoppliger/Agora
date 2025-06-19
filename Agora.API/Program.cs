using System.Text.Json.Serialization;
using Agora.API.Filters;
using Agora.API.Validation;
using Agora.API.Validation.Interfaces;
using Agora.Core.DomainServices;
using Agora.Core.Interfaces;
using Agora.Core.Interfaces.DomainServices;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.Entities;
using Agora.Core.Settings;
using Agora.Core.Validation;
using Agora.Core.Validation.Interfaces;
using Agora.Infrastructure.Data;
using Agora.Infrastructure.Repositories;
using Agora.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Initialize Serilog for early logging before the app is fully built
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    // Configure Serilog with full settings
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

    builder.Host.UseSerilog();
    
    /*  ====================== 
       | Service registration |
        ====================== */

    // Custom action filter for logging, for controllers methods
    builder.Services.AddTransient<LogActionFilter>();
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<LogActionFilter>();
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    
    // Inactivate automatic model validation when entering an action method of a controller
    // This will be taken in charge by LogActionFilter.
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
    
    // OpenAPI/Swagger generation using Scalar
    builder.Services.AddOpenApi();
    
    // DbContext for DB connection, using MySQL provider (MariaDB is MySQL-compatible)
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
    builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<AgoraDbContext>());
    
    // AutoMapper for DTO-model mapping
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
    // Configuration settings
    builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("UserSettings"));
    
    /*  --------------
       | Repositories |
        -------------- */
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPostRepository, PostRepository>();
    builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
    builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
    builder.Services.AddScoped<ITransactionStatusRepository, TransactionStatusRepository>();

    /*  -----------------
       | Data validation |
        ----------------- */
    builder.Services.AddScoped<IInputValidator, InputValidator>();
    builder.Services.AddScoped<IBusinessRulesValidator, BusinessRulesValidator>();
    builder.Services.AddScoped<IAuthorizationValidator, AuthorizationValidator>();

    /*  -------------------
       | Business services  |
        -------------------- */
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IPostCategoryService, PostCategoryService>();
    builder.Services.AddScoped<IPostService, PostService>();
    builder.Services.AddScoped<ITransactionService, TransactionService>();
    builder.Services.AddScoped<ITransactionStatusService, TransactionStatusService>();
    
    /*  ---------------------------------
      | Authentication and authorization |
        --------------------------------- */
    // Add ASP.NET Core authorization
    builder.Services.AddAuthorization();
    
    // Identity system with roles and store
    builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = ValidationConstants.User.PasswordMinLength;
                options.Password.RequiredUniqueChars = 1;
            }
        )
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AgoraDbContext>();
    
    // Expose Identity API endpoints (e.g. /login, /register, etc.)
    builder.Services.AddIdentityApiEndpoints<User>(); 
    
    // Identity management services
    builder.Services.AddScoped<RoleManager<IdentityRole>>();
    builder.Services.AddScoped<UserManager<User>>();
    builder.Services.AddScoped<SignInManager<User>>(); // For manual logins
    builder.Services.AddScoped<IUserStore<User>, UserStore<User>>();
    builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>();
    
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IUserContextService, UserContextService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    
    /* --------------------------------------
     | Cross-origin ressource sharing (CORS) |
       -------------------------------------- */
    builder.Services.AddCors();

/* ============================================================================================================ */
    var app = builder.Build();

    /*  ======================
       | Middleware pipeline  |
        ====================== */
    
    // Seed the user roles
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await AgoraDbContextSeed.SeedRolesAsync(roleManager);
    }
    
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        
        // Enable Swagger UI and Scalar API for DEV inspection
        app.MapOpenApi();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "Agora API v1"); });
        app.MapScalarApiReference();

        // Seed the development database
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AgoraDbContext>();
            await context.Database.MigrateAsync();
            
            var userManager = services.GetRequiredService<UserManager<User>>();
            
            await AgoraDbContextSeed.SeedDevelopmentDataAsync(context, userManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error while populating the database in development environment.");
        }
    }

    // Log each HTTP request
    app.UseSerilogRequestLogging(); 
    
    app.UseHttpsRedirection();

    // CORS config: Allows the Angular DEV front-end 
    app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
        .WithOrigins("http://localhost:4200", "https://localhost:4200"));
    
    app.UseAuthorization();

    app.MapControllers();
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed: {message}", ex.Message);
}
finally
{
    // Ensure logs are written before exit
    Log.CloseAndFlush();
}