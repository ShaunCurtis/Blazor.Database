/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Database.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazr.Database.Data
{
    public class SQLiteWeatherDbContext : DbContext
    {
        /// <summary>
        /// New Method - creates a guid in case we need to track it
        /// </summary>
        /// <param name="options"></param>
        public SQLiteWeatherDbContext(DbContextOptions<SQLiteWeatherDbContext> options)
            : base(options)
            => this.BuildInMemoryDatabase();

        /// <summary>
        /// DbSet for the <see cref="DbWeatherForecast"/> record
        /// </summary>
        public DbSet<WeatherForecast> WeatherForecast { get; set; }

        private void BuildInMemoryDatabase()
        {
            var conn = this.Database.GetDbConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "CREATE TABLE [WeatherForecast]([ID] UNIQUEIDENTIFIER PRIMARY KEY, [Date] [smalldatetime] NOT NULL, [TemperatureC] [int] NOT NULL, [Summary] [varchar](255) NULL)";
            cmd.ExecuteNonQuery();
            foreach (var forecast in this.NewForecasts)
            {
                cmd.CommandText = $"INSERT INTO WeatherForecast([ID], [Date], [TemperatureC], [Summary]) VALUES({Guid.NewGuid()} ,'{forecast.Date.LocalDateTime.ToLongDateString()}', {forecast.TemperatureC}, '{forecast.Summary}')";
                cmd.ExecuteNonQuery();
            }
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private List<WeatherForecast> NewForecasts
        {
            get
            {
                var rng = new Random();

                return Enumerable.Range(1, 80).Select(index => new WeatherForecast
                {
                    //ID = index,
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                }).ToList();
            }
        }
    }
}
