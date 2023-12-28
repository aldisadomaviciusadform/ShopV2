
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Clients;
using Infrastructure.Contexts;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using System.Data;
using WebAPI.Middleware;

namespace WebAPI;

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
        builder.Services.AddScoped<ItemService>();
        builder.Services.AddScoped<UserClientService>();
        builder.Services.AddScoped<ShopService>();

        //inject Repository
        //        builder.Services.AddScoped<IItemRepository, ItemRepositoryEFInMemory>();
        builder.Services.AddScoped<IItemRepository, ItemRepositoryPostgre>();
        builder.Services.AddScoped<IShopRepository, ShopRepository>();

        //inject client
        builder.Services.AddScoped<IGeneralClient, UserClient>();

        //change logger
        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
        builder.Logging.AddSerilog(logger);

        builder.Services.AddHttpClient();

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
