using Carter;
using Mentorly.ProfileService.Extensions;
using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureMongoDb().ConfigureMongoDbEntities();
builder.Services.AddCarter();
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   // app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.UseMongoDbEntitiesAsync();
app.MapCarter();
app.Run();
