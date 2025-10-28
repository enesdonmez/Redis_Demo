using Microsoft.Extensions.Caching.Memory;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "In-Memory Cache";
        opt.Theme = ScalarTheme.Laserwave;
    });
}

app.UseHttpsRedirection();

app.MapGet("set", (IMemoryCache _memoryCache, string name) =>
{
    _memoryCache.Set("name", name);

}).WithTags("in-memory");

app.MapGet("get", (IMemoryCache _memoryCache) =>
{
    if (_memoryCache.TryGetValue("name", out string? name))
    {
        return Results.Ok(name);
    }
    return Results.NotFound();

}).WithTags("in-memory");

app.MapGet("remove", (IMemoryCache _memoryCache, string key) =>
{
    if (_memoryCache.TryGetValue("name", out string? name))
    {
        _memoryCache.Remove(key);
        return Results.Ok();
    }
    return Results.NotFound();

}).WithTags("in-memory");


app.MapGet("setDate", (IMemoryCache _memoryCache) =>
{
    _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
    {
        AbsoluteExpiration = DateTime.Now.AddSeconds(30),
        SlidingExpiration = TimeSpan.FromSeconds(5)
    });

}).WithTags("in-memory");

app.MapGet("getDate", (IMemoryCache _memoryCache) =>
{
    return _memoryCache.Get<DateTime>("date");

}).WithTags("in-memory");

app.Run();

