using FileToolKit.IO.File.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using ItemsStoreWebAPI.DataBase;
using ItemsStoreWebAPI.DataBase.Transactions;
using ItemsStoreWebAPI.Mappings;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Get DB ConnectionString
builder.Services.Configure<DbConnectionOptions>(
    configuration.GetSection("ConnectionStrings"));

// Adding DB Connection
builder.Services.AddDbContext<BaseDbContext, SqlServerDbContext>((provider, options) =>
{
    var connectionOptions = provider.GetRequiredService<IOptions<DbConnectionOptions>>();
    var connectionString = connectionOptions.Value.SqlServerDbContext;
    options.UseSqlServer(connectionString);
});

// Adding Repositories
builder.Services.AddScoped<IStorage<TV>, TvDbStorage>();
builder.Services.AddScoped<IStorage<Mobile>>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<MobileDbStorage>>();
    var options = provider.GetRequiredService<IOptions<DbConnectionOptions>>();
    return new MobileDbStorage(
        options.Value.SqlServerDbContext, logger);
});

// Adding Services
builder.Services.AddScoped<IService<TV>, TvService>();
builder.Services.AddScoped<IService<Mobile>, MobileService>();
builder.Services.AddScoped<IFileService<TV>, FileService<TV>>();
builder.Services.AddScoped<IFileService<Mobile>, FileService<Mobile>>();
builder.Services.AddScoped<IDbTransactionsService<TV>, TvDbTransactionsService>();

// Adding FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<TvRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MobileRequestValidator>();

// Adding DB Transactions
builder.Services.AddScoped<IDbTransactionOperations<TV>>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<TvDbTransactions>>();
    var connectionOptions = provider.GetRequiredService<IOptions<DbConnectionOptions>>();
    var connectionString = connectionOptions.Value.SqlServerDbContext;
    return new TvDbTransactions(connectionString, logger);
});

builder.Services.AddScoped<IDbTransactionOperations<Mobile>>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<MobileDbTransactions>>();
    var connectionOptions = provider.GetRequiredService<IOptions<DbConnectionOptions>>();
    var connectionString = connectionOptions.Value.SqlServerDbContext;
    return new MobileDbTransactions(connectionString, logger);
});

// Adding Custom File Lib
builder.Services.AddFileToolKitFor<TV>();
builder.Services.AddFileToolKitFor<Mobile>();

// Adding AutoMapper
builder.Services.AddAutoMapper(typeof(TvProfile));
builder.Services.AddAutoMapper(typeof(MobileProfile));

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