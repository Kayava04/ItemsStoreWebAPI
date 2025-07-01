using FileToolKit.IO.File.Extensions;
using FluentValidation.AspNetCore;
using ItemsStoreWebAPI.DataBase;
using ItemsStoreWebAPI.DataBase.Transactions;
using ItemsStoreWebAPI.Mappings;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Db Connection String
var connectionString = configuration.GetConnectionString(nameof(SqlServerDbContext));

// Adding Db Connection
builder.Services.AddDbContext<BaseDbContext, SqlServerDbContext>(options =>
    options.UseSqlServer(connectionString));

// Adding Repositories
builder.Services.AddScoped<IStorageBase<TV>, TvDbStorage>();
builder.Services.AddScoped<IStorageBase<Mobile>, MobileDbStorage>();

// Adding Services
builder.Services.AddScoped<IServiceBase<TV>, TvService>();
builder.Services.AddScoped<IServiceBase<Mobile>, MobileService>();
builder.Services.AddScoped<IFileService<TV>, TvFileService>();
builder.Services.AddScoped<IFileService<Mobile>, MobileFileService>();
builder.Services.AddScoped<IDbTransactionsService<TV>, TvDbTransactionsService>();

// Adding Validators
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<TvRequestValidator>());

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<MobileRequestValidator>());

// Adding ADO.NET TV Transactions
builder.Services.AddScoped<IDbTransactionOperations<TV>>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<TvDbTransactions>>();
    return new TvDbTransactions(connectionString!, logger);
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