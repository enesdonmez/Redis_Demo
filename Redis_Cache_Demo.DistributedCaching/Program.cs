using Microsoft.Extensions.Caching.Distributed;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("RedisConn").Value!;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Distributed Cache";
        opt.Theme = ScalarTheme.Mars;
    });
}


app.UseHttpsRedirection();

app.MapGet("/set/{value}", async (string nameValue, IDistributedCache cache, CancellationToken cancellationToken) =>
{
    await cache.SetStringAsync("name", nameValue, cancellationToken);
    return Results.Ok($"Cached {"name"} : {nameValue}");
});

app.MapGet("/set", async (string fullName, IDistributedCache cache, CancellationToken cancellationToken) =>
{
    await cache.SetAsync("fullname", Encoding.UTF8.GetBytes(fullName), cancellationToken);
    return Results.Ok();
});

app.MapGet("get", async (IDistributedCache cache, CancellationToken cancellationToken) =>
{
    var fullname = await cache.GetStringAsync("fullname", cancellationToken);
    var nameBinary = await cache.GetAsync("name", cancellationToken);
    var name = Encoding.UTF8.GetString(nameBinary!);
    return Results.Ok(new
    {
        name,
        fullname
    });
});


app.Run();

