using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using TechMoveClient.Data;
using TechMoveClient.Models;
using TechMoveClient.Services;
using TechMoveClient.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// Session configuration (Docker safe, 30 min timeout)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add MVC services
builder.Services.AddControllersWithViews();

// DB Context configuration (SQL Server, connection string from appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// API settings configuration (bind ApiSettings section to ApiSettings class)
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings")
);

// Supabase settings configuration (bind Supabase section to SupabaseSettings class)
builder.Services.Configure<SupabaseSettings>(
    builder.Configuration.GetSection("Supabase"));

// Helper services
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtAuthHandler>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<SupabaseFileService>();

// Single HttpClient instance for API calls, configured with base URL from ApiSettings
builder.Services.AddHttpClient("TechMoveAPI", (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ApiSettings>>().Value;

    if (string.IsNullOrWhiteSpace(options.BaseUrl))
        throw new Exception("ApiSettings:BaseUrl is missing");

    client.BaseAddress = new Uri(options.BaseUrl);
});

// Client API service
builder.Services.AddHttpClient<ClientApiService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

// Contract API service
builder.Services.AddHttpClient<ContractApiService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

// Service Request API service
builder.Services.AddHttpClient<ServiceRequestApiService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

// Data Protection configuration (store keys in /keys directory)
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
    .SetApplicationName("TechMoveClient");

// Build app
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();