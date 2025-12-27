using Carter;
using Microsoft.AspNetCore.OpenApi;
using Mentorly.SearchService.ElasticSearch;
using Mentorly.SearchService.GrpcEndpoint;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.AddElasticSearch().AddElasticSearchConfigurations();

builder.Services.AddCarter();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddGrpc(options =>
{
    options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Fastest; // for example
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    //app.MapScalarApiReference();
}
app.UseSwagger();
app.UseSwaggerUI();

app.MapGrpcService<CreateUserProfileGrpcEndpoint>();
app.UseRouting();
app.MapCarter();
app.MapControllers();
await app.UseElasticSearchAsync();

app.Run();
