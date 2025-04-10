using FileToolKit.IO.File.Extensions;
using ItemsStoreWebAPI.Factories;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<StorageSettings>(configuration.GetSection("StorageSettings"));

builder.Services.AddSingleton<ITVStorageFactory, TVStorageFactory>();
builder.Services.AddSingleton<TVListStorage>();
builder.Services.AddSingleton<TVDictionaryStorage>();
builder.Services.AddScoped<ITVService, TVService>();
builder.Services.AddScoped<ITVRequestValidator, TVRequestValidator>();
builder.Services.AddScoped<IFileService<TV>, TVFileService>();

builder.Services.AddFileToolKitFor<TV>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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