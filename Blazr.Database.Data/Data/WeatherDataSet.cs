using Blazor.EditForms.Web.Data;
using Blazr.Database.Core;
using System;
using System.Linq;

namespace Blazr.Database.Data.Data
{
    class WeatherDataSet : InMemoryDataSet<WeatherForecast>
    {
        public override void LoadData()
        {
            var rng = new Random();

            this.records = Enumerable.Range(1, 80).Select(index => new WeatherForecast
            {
                //ID = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList();

        }

        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    }
}
