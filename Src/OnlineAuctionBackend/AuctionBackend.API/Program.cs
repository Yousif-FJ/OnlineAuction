using Microsoft.AspNetCore.Authorization;
using AuctionBackend.Api.MapProfile.V1;
using AuctionBackend.Application.StartupConfig;
using AuctionBackend.Identity.StartupConfig;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using AuctionBackend.Application.Actions.Items;
using CloudinaryDotNet;
using AuctionBackend.Application.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.CustomConfigAuthentication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
          .RequireAuthenticatedUser()
          .Build();
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.CustomConfigAuctionDb(builder.Configuration["DATABASE_URL"]);
builder.Services.CustomConfigIdentityDb(builder.Configuration["DATABASE_URL"]);

builder.Services.AddAutoMapper(typeof(AuthenticationMapPofile));

builder.Services.AddScoped<IAuctionUserManager, AuctionUserManager>();
builder.Services.AddScoped<IPictureProvider, PictureProvider>();
builder.Services.AddSingleton<Cloudinary>(
    new Cloudinary(builder.Configuration["CLOUDINARY_URL"]));

builder.Services.AddAppIdentityServices();

builder.Services.AddMediatR(typeof(AddItemHandler));
builder.Services.AddFluentValidation(new[] { typeof(AddItemHandler).Assembly });

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
    //Save auth token in browser for swaggerUI
    options.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
