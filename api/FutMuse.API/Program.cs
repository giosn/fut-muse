using FutMuse.API.Helpers;
using FutMuse.API.Repositories;

namespace FutMuse.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(
                    "http://localhost:4200",
                    "https://witty-cliff-09384fd10.3.azurestaticapps.net",
                    "https://witty-stone-05f3f3210.3.azurestaticapps.net",
                    "https://www.futmuse.com"
                );
            });
        });

        // Dependencies
        builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
        builder.Services.AddScoped<ISearchRepository, SearchRepository>();
        builder.Services.AddScoped<HtmlDocumentNode>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

