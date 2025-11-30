using Carter;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.OpenApi;
using Mentorly.SearchService.ElasticSearch;


var builder = WebApplication.CreateBuilder(args);
builder
    .AddElasticSearch()
    .AddElasticSearchConfigurations();
// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();
app.MapCarter();
await app.UseElasticSearchAsync();
//app.MapControllers();

await app.RunAsync();
