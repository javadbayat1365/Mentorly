using Carter;
using Mentorly.ProfileService.Extensions;
using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;
//for develope branch
//test for master branch
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureMongoDb().ConfigureMongoDbEntities();
builder.Services.AddCarter();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   //app.MapOpenApi();
   app.MapScalarApiReference();
}
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapCarter();
app.MapScalarApiReference();
await app.UseMongoDbEntitiesAsync();
app.Run();
