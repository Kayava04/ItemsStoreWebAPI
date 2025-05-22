using FileToolKit.IO.File.Extensions;
using ItemsStoreWebAPI.Core;
using ItemsStoreWebAPI.Factories;
using ItemsStoreWebAPI.Mappings;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Db Connection
var provider = configuration["DatabaseProvider"];

switch (provider)
{
    case "NpgSql":
        builder.Services.AddDbContext<BaseDbContext, PostgresDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(nameof(PostgresDbContext))));
        break;
    case "SqlServer":
        builder.Services.AddDbContext<BaseDbContext, SqlServerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(nameof(SqlServerDbContext))));
        break;
    default:
        throw new NotSupportedException($"Database provider '{provider}' not supported");
}

// Set Type of Repository
builder.Services.Configure<StorageSettings>(configuration.GetSection("StorageSettings"));

// Adding Repositories
builder.Services.AddSingleton<ITVStorageFactory, TVStorageFactory>();
builder.Services.AddSingleton<TVListStorage>();
builder.Services.AddSingleton<TVDictionaryStorage>();
builder.Services.AddScoped<TvDbStorage>();

// Adding Services
builder.Services.AddScoped<ITVService, TVService>();
builder.Services.AddScoped<IFileService<TV>, TVFileService>();

// Adding Validators
builder.Services.AddScoped<ITVRequestValidator, TVRequestValidator>();

// Adding Custom File Lib
builder.Services.AddFileToolKitFor<TV>();

// Adding AutoMapper
builder.Services.AddAutoMapper(typeof(TvProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Db Initialization
using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
var context = services.GetRequiredService<BaseDbContext>();
DbInitializer.Initialize(context);

// Adding Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();