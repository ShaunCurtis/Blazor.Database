/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazr.Database.Core
{
    public record WeatherForecast : IDbRecord<WeatherForecast>
    {
        [Key] public Guid ID { get; init; } = Guid.Empty;

        public DateTimeOffset Date { get; init; } = DateTimeOffset.Now;

        public int TemperatureC { get; init; } = 0;

        public string Summary { get; init; } = string.Empty;

        [NotMapped] public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [NotMapped] public string DisplayName => $"Weather Forecast for {this.Date.LocalDateTime.ToShortDateString()} ";

        // A long string field to demo using a max row in a data table
        [NotMapped] public string Description => $"The Weather Forecast for this {this.Date.DayOfWeek}, the {this.Date.Day} of the month {this.Date.Month} in the year of {this.Date.Year} is {this.Summary}.  From the font of all knowledge!";

    }
}
