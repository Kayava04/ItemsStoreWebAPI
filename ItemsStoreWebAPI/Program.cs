using ItemsStoreWebAPI.Factories;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection("StorageSettings"));
builder.Services.AddSingleton<ITVStorageFactory, TVStorageFactory>();
builder.Services.AddSingleton<TVListStorage>();
builder.Services.AddSingleton<TVDictionaryStorage>();
builder.Services.AddScoped<ITVService, TVService>();
builder.Services.AddScoped<ITVRequestValidator, TVRequestValidator>();
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