/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Core;
using Blazr.SPA.Data;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blazr.Database.Core
{
    public class EditWeatherForecast : IValidation, IEditRecord<WeatherForecast>
    {
        public Guid ID { get; set; } = Guid.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

        [Required]
        public int TemperatureC { get; set; } = 0;

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [Required]
        public string Summary { get; set; } = string.Empty;

        public string DisplayName => $"Weather Forecast for {this.Date.LocalDateTime.ToShortDateString()} ";

        public WeatherForecast GetRecord() => new WeatherForecast
        {
            ID = this.ID,
            Location = this.Location,
            Date = this.Date,
            TemperatureC = this.TemperatureC,
            Summary = this.Summary
        };

        public void Populate(IDbRecord<WeatherForecast> dbRecord)
        {
            var rec = (WeatherForecast)dbRecord;
            this.Location = rec.Location;
            this.ID = rec.ID;
            this.Date = rec.Date;
            this.TemperatureC = rec.TemperatureC;
            this.Summary = rec.Summary;
        }

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
