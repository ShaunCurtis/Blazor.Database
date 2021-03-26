using Blazor.SPA.Data;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Database.Data
{
    public class WeatherForecast : IValidation, IDbRecord<WeatherForecast>
    {
        [Key] public int ID { get; set; } = -1;

        public DateTime Date { get; set; } = DateTime.Now;

        public int TemperatureC { get; set; } = 0;

        [NotMapped] public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; } = string.Empty;

        [NotMapped] public Guid GUID { get; init; } = Guid.NewGuid();

        [NotMapped] public string DisplayName => $"Weather Forecast for {this.Date.ToShortDateString()} ";

        // A long string field to demo using a max row in a data table
        [NotMapped] public string Description => $"The Weather Forecast for this {this.Date.DayOfWeek}, the {this.Date.Day} of the month {this.Date.Month} in the year of {this.Date.Year} is {this.Summary}.  From the font of all knowledge!";

        public bool Validate(ValidationMessageStore validationMessageStore, string fieldname, object model = null)
        {
            model = model ?? this;
            bool trip = false;

            this.Summary.Validation("Summary", model, validationMessageStore)
                .LongerThan(2, "Your description needs to be a little longer! 3 letters minimum")
                .Validate(ref trip, fieldname);

            this.Date.Validation("Date", model, validationMessageStore)
                .NotDefault("You must select a date")
                .LessThan(DateTime.Now.AddMonths(1), true, "Date can only be up to 1 month ahead")
                .Validate(ref trip, fieldname);

            this.TemperatureC.Validation("TemperatureC", model, validationMessageStore)
                .LessThan(70, "The temperature must be less than 70C")
                .GreaterThan(-60, "The temperature must be greater than -60C")
                .Validate(ref trip, fieldname);

            return !trip;
        }

    }
}
