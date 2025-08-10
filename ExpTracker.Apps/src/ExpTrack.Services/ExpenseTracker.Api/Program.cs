using ExpTrack.DbAccess.Contracts;
using ExpTrack.EfCore.Repositories;
using AutoMapper;
using ExpTrack.AppModels.Mappings;
using Microsoft.Extensions.DependencyInjection;
using AppBal.Contracts;
using AppBal.Adapters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register the DbContext with dependency injection
builder.Services.AddSingleton<IConfigurationConnectionString, ConnectionStringConfig>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfiles>());
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

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
