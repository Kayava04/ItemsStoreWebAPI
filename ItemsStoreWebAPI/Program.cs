using FileToolKit.IO.File.Extensions;
using ItemsStoreWebAPI.DataBase;
using ItemsStoreWebAPI.DataBase.Transactions;
using ItemsStoreWebAPI.Factories;
using ItemsStoreWebAPI.Mappings;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Choose Db Connection String
var provider = configuration["DatabaseProvider"];

var selectedConnectionString = provider switch
{
    "NpgSql" => configuration.GetConnectionString(nameof(PostgresDbContext)),
    "SqlServer" => configuration.GetConnectionString(nameof(SqlServerDbContext)),
    _ => throw new NotSupportedException($"Database provider '{provider}' not supported")
};

// Choose Db Context
switch (provider)
{
    case "NpgSql":
        builder.Services.AddDbContext<BaseDbContext, PostgresDbContext>(options =>
            options.UseNpgsql(selectedConnectionString));
        break;
    case "SqlServer":
        builder.Services.AddDbContext<BaseDbContext, SqlServerDbContext>(options =>
            options.UseSqlServer(selectedConnectionString));
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
builder.Services.AddScoped<IDbTransactionsService<TV>, TvDbTransactionsService>();

// Adding Validators
builder.Services.AddScoped<ITVRequestValidator, TVRequestValidator>();

// Adding ADO.NET TV Transactions
builder.Services.AddScoped<IDbTransactionOperations<TV>>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<TvDbTransactions>>();
    
    return new TvDbTransactions(selectedConnectionString, logger);
});

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