using Microsoft.EntityFrameworkCore;
using MvcApi.Interface;
using MvcApi.Models;
using MvcApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Ambil connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContextPool<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<ProductInterface, ProductRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/db-ping", async (AppDbContext db) =>
{
    try
    {
        var can = await db.Database.CanConnectAsync();

        // buka koneksi dan jalankan SELECT 1
        await db.Database.OpenConnectionAsync();
        using var cmd = db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "SELECT 1";
        var scalar = await cmd.ExecuteScalarAsync();

        // versi server (opsional)
        cmd.CommandText = "SELECT VERSION()";
        var version = (string?)await cmd.ExecuteScalarAsync();

        return Results.Ok(new { ok = can, select1 = scalar, serverVersion = version });
    }
    catch (Exception ex)
    {
        return Results.Problem($"DB connection failed: {ex.Message}");
    }
    finally
    {
        await db.Database.CloseConnectionAsync();
    }
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
