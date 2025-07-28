using Carter;
using Mentorly.ProfileService.Extensions;
using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;
//for develope branch
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureMongoDb().ConfigureMongoDbEntities();
//builder.Services.AddCarter();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new()
    {
        Title = "My API",
        Version = "v1"
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   //app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
    //app.MapScalarApiReference();
}
//app.MapCarter();
await app.UseMongoDbEntitiesAsync();
app.Run();
