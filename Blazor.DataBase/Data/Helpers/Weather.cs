/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Database.Data
{
    public class Weather : IDbRecord<Weather>
    {
        [Key] public Guid ID { get; set; } = Guid.Empty;

        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

        public int TemperatureC { get; set; } = 0;

        public string Summary { get; set; } = string.Empty;

        [NotMapped] public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [NotMapped] public Guid GUID { get; init; } = Guid.NewGuid();

        [NotMapped] public string DisplayName => $"Weather Forecast for {this.Date.LocalDateTime.ToShortDateString()} ";

        // A long string field to demo using a max row in a data table
        [NotMapped] public string Description => $"The Weather Forecast for this {this.Date.DayOfWeek}, the {this.Date.Day} of the month {this.Date.Month} in the year of {this.Date.Year} is {this.Summary}.  From the font of all knowledge!";
    }
}
