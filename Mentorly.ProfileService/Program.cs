using Carter;
using Mentorly.ProfileService.Extensions;
using Mentorly.SearchService.ElasticSearch;
using Mentorly.ProfileService.SearchServices.Interfaces;
using Refit;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddElasticSearch().AddElasticSearchConfigurations();
builder.ConfigureMongoDb().ConfigureMongoDbEntities();
builder.Services.AddCarter();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddServiceDiscovery( );
builder.Services.AddRefitClient<ISearchService>().ConfigureHttpClient(
    client => client.BaseAddress =  new Uri("http://SearchService"));

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
