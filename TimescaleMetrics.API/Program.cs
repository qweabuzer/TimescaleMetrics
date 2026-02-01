using TimescaleMetrics.Application.Services;
using TimescaleMetrics.Core.Interfaces;
using TimescaleMetrics.DataAccess;
using TimescaleMetrics.DataAccess.Mapping;
using TimescaleMetrics.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => { },
    typeof(MappingProfile).Assembly);

builder.Services.AddDbContext<TimescaleDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(TimescaleDbContext)));
    });

builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddScoped<IValueService, ValueService>();
builder.Services.AddScoped<IValueRepository, ValueRepository>();
builder.Services.AddScoped<ICsvParserService, CsvParserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
