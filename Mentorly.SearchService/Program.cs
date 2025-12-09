using Carter;
using Microsoft.AspNetCore.OpenApi;
using Mentorly.SearchService.ElasticSearch;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddElasticSearch().AddElasticSearchConfigurations();
builder.Services.AddCarter();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    //app.MapScalarApiReference();
}
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseRouting();
app.MapCarter();
app.MapControllers();
await app.UseElasticSearchAsync();

app.Run();
