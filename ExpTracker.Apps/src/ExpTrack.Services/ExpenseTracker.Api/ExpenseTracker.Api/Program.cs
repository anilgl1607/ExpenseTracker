using ExpTrack.AppBal.Adapters;
using ExpTrack.AppBal.Contracts;
using ExpTrack.AppModels.Mappings;
using ExpTrack.DbAccess.Contracts;
using ExpTrack.EfCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register the DbContext with dependency injection
builder.Services.AddSingleton<IConfigurationConnectionString, ConnectionStringConfig>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfiles>());
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
