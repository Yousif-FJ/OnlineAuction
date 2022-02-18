using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using AuctionBackend.Api.MapProfile.V1;
using AuctionBackend.Application.StartupConfig;
using AuctionBackend.Identity.StartupConfig;
using AuctionBackend.Infrastructure.StartupConfig;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.CustomConfigAuthentication(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
          .RequireAuthenticatedUser()
          .Build();
});

builder.Services.AddControllers();

builder.Services.CustomConfigAuctionDb(builder.Configuration["DATABASE_URL"]);
builder.Services.CustomConfigIdentityDb(builder.Configuration["DATABASE_URL"]);

builder.Services.AddAutoMapper(typeof(AuthenticationMapPofile));

builder.Services.AddAppIdentityServices();
builder.Services.AddAppServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Online Auction"
    });

    options.CustomConfigSwagger();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    //Save auth token in browser
    options.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();