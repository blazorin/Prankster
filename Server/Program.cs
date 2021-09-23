using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using AspNetCoreRateLimit;
using Microsoft.IdentityModel.Tokens;
using Server.Secure;
using Model.Data;
using Model.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
    {
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(PrivateKeys.JwtSecretKey)
            ),
            RequireExpirationTime = true,
            ValidIssuer = "MauiPServer",
            ValidAudience = "MauiPClient"
        };
    });

// CORS
builder.Services.AddCors();

// Ratelimit
builder.Services.AddMemoryCache();

    // enable IConfiguration
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Add model services
builder.Services.AddMauiPModelServices();


var app = builder.Build();

// Configure the HTTP request pipeline.

using (var db = new MauiPContext())
{
    db.Database.EnsureCreated();
}

// CORS
app.UseCors(builder =>
{
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
    builder.AllowAnyOrigin();
});

// Ratelimit
app.UseIpRateLimiting();

// Handles exceptions and generates a custom response body
app.UseExceptionHandler("/errors/500");

// Handles non-success status codes with empty body
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
