using IdentityProvider;
using IdentityProvider.Data;
using IdentityProvider.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfiguredSwaggerGen();
builder.Services.AddJwtAuth(builder.Configuration);

var connectionString = builder.Configuration["IdentityDbConnectionString"]
    ?? throw new InvalidOperationException("IdentityDbConnectionString must be specified.");
builder.Services.AddDbContext<IdentityDbContext>(
    options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run();
