using Carter;
using Mentorly.ProfileService.Extensions;
using Mentorly.SearchService.ElasticSearch;
using Scalar.AspNetCore;
//for develope branch
//for develope branch
//test for master branch
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddElasticSearch().AddElasticSearchConfigurations();
builder.ConfigureMongoDb().ConfigureMongoDbEntities();
builder.Services.AddCarter();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
   //app.MapScalarApiReference();//به جای swagger
}
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapCarter();
await app.UseMongoDbEntitiesAsync();
await app.UseElasticSearchAsync();
app.Run();
