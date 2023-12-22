
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using ShopV2.Contexts;
using ShopV2.Interfaces;
using ShopV2.Middleware;
using ShopV2.Repository;
using ShopV2.Services;
using System.Data;

namespace ShopV2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string? dbConnectionString = builder.Configuration.GetConnectionString("PostgreConnection");
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //inject IDbConnection
        builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(dbConnectionString));

        //inject DataContext
        //        builder.Services.AddDbContext<DataContext>(sp => sp.UseInMemoryDatabase("MyDatabase"));
        builder.Services.AddDbContext<DataContext>(sp => sp.UseNpgsql(new NpgsqlConnection(dbConnectionString)));

        //inject Services
        builder.Services.AddScoped<IItemService, ItemService>();

        //inject Repository
        //        builder.Services.AddScoped<IItemRepository, ItemRepositoryEFInMemory>();
        builder.Services.AddScoped<IItemRepository, ItemRepositoryPostgre>();

        //change logger
        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
        builder.Logging.AddSerilog(logger);

        var app = builder.Build();

        //custom middleware
        app.UseMiddleware<ErrorChecking>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
