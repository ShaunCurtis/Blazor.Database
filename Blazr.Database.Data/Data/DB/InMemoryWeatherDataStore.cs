/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Database.Core;
using Blazr.SPA.Core;
using Blazr.SPA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Blazr.Database.Data
{
    public class InMemoryWeatherDataStore : IInMemoryDataStore
    {
        public InMemoryDataSet<WeatherForecast> WeatherForecast { get; set; }

        public InMemoryWeatherDataStore()
        {
            this.WeatherForecast = new InMemoryDataSet<WeatherForecast>();
            WeatherForecast.LoadData(LoadWeatherForecastData());
        }

        public List<WeatherForecast> LoadWeatherForecastData()
        {
            var summaries = new[] {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            var rng = new Random();

            return Enumerable.Range(1, 80).Select(index => new WeatherForecast
            {
                Location = "Australia",
                ID = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summaries[rng.Next(summaries.Length)]
            }).ToList();
        }

        public InMemoryDataSet<TRecord> GetDataSet<TRecord>() where TRecord : class, IDbRecord<TRecord>, new()
        {
            var dbSetName = new TRecord().GetDbSetName();
            // Get the property info object for the DbSet 
            var pinfo = this.GetType().GetProperty(dbSetName);
            InMemoryDataSet<TRecord> dbSet = null;
            Debug.Assert(pinfo != null);
            // Get the property DbSet
            try
            {
                dbSet = (InMemoryDataSet<TRecord>)pinfo.GetValue(this);
            }
            catch
            {
                throw new InvalidOperationException($"{dbSetName} does not have a matching DBset ");
            }
            Debug.Assert(dbSet != null);
            return dbSet;
        }
    }
}
