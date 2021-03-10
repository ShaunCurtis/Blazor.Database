using Blazor.Database.Data.Validators;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace Blazor.Database.Data
{
    public class WeatherForecast : IValidation
    {
        public int ID { get; set; } = -1;

        public DateTime Date { get; set; } = DateTime.Now;

        public int TemperatureC { get; set; } = 0;

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; } = string.Empty;

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
