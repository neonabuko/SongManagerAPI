﻿using Microsoft.EntityFrameworkCore;
using ScoreHubAPI.Repositories;
using ScoreHubAPI.Service;
using ScoreHubAPI.Entities;

namespace ScoreHubAPI.Data;

public static class ServiceConfiguration
{

    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ScoreHubContext>();
        await dbContext.Database.MigrateAsync();
    } 

    public static IServiceCollection ConfigureServices(this IServiceCollection services,
    IConfiguration configuration) 
    {
        var connectionString = configuration.GetConnectionString("ScoreHubContext");
        services.AddSingleton(configuration);
        services.AddSqlServer<ScoreHubContext>(connectionString);
        
        services.AddScoped<SongRepository>();
        services.AddScoped<ScoreRepository>();
        services.AddScoped<IMusicRepository<Song>, SongRepository>();
        services.AddScoped<IMusicRepository<Score>, ScoreRepository>();
        services.AddScoped<SongService>();
        services.AddScoped<ScoreService>();
        // services.AddScoped<ChunkService>(); not scoped anymore because it's being manually instantiated

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

}
