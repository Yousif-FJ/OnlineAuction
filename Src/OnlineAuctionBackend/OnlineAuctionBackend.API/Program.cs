using OnlineAuctionBackend.Application.StartupConfig;
using OnlineAuctionBackend.Identity.StartupConfig;
using OnlineAuctionBackend.Infrastructure.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.CustomConfigAuctionDb(builder.Configuration["DATABASE_URL"]);
builder.Services.CustomConfigIdentityDb(builder.Configuration["DATABASE_URL"]);

builder.Services.AddAppIdentityServices();
builder.Services.AddAppServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
