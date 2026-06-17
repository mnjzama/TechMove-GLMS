using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechMoveAPI.Data;
using TechMoveAPI.Services;
using TechMoveAPI.Domain.Strategy;
using TechMoveAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Configuration for API keys and settings
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings")
);

// Configuration for Supabase storage settings
builder.Services.Configure<SupabaseSettings>(
    builder.Configuration.GetSection("Supabase"));

// Application Services
builder.Services.AddHttpClient<ExchangeRateService>();
builder.Services.AddScoped<StrategyFactory>();
builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<ServiceRequestService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<JwtService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// DB (Docker-ready)
var connectionString = builder.Configuration["TechMoveDB"];
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("TechMoveDB connection string is empty. Check environment variables or appsettings.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// JWT Configuration (Docker-ready)
var jwtKey = builder.Configuration["TechMoveJwtKey"];
var key = Encoding.ASCII.GetBytes(jwtKey!);


/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/Hello-PROG7311/tree/main/12%20-%20Docker%20Compose
Date: [n.d]
Date Accessed: 9 May 2026
*/
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateIssuer = false,
        ValidateAudience = false
    };
});


// Build app
var app = builder.Build();

/*
Author: Microsoft Learn
URL: https://learn.microsoft.com/en-us/aspnet/core/data
Date: [n.d]
Date Accessed: 01 June 2026

Author: Microsoft Learn
URL: https://learn.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/sql
Date: [n.d]
Date Accessed: 01 June 2026

Author: MD Tareq Hassan
Title: ASP.NET Core Seeding Database on Application Startup
URL: https://hovermind.com/aspnet-core/seeding-database-on-application-startup.html
Date: [n.d]
Date Accessed: 01 June 2026
*/
using (var scope = app.Services.CreateScope())
{
    // Attempt to apply migrations and seed the database, with retries for transient failures
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    for (int i = 0; i < 30; i++)
    {
        try
        {
            context.Database.Migrate();
            DbSeeder.Seed(context);
            break;
        }
        catch
        {
            Thread.Sleep(2000); // Wait before retrying
        }
    }
}


// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();