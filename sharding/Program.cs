using Microsoft.EntityFrameworkCore;
using sharding.Models;
using sharding.service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PostgresContext>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("pgshard1"));
});

builder.Services.AddDbContext<Pgshard2Context>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("pgshard2"));
});

builder.Services.AddDbContext<Pgshard3Context>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("pgshard3"));
});

builder.Services.AddScoped<Repository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
