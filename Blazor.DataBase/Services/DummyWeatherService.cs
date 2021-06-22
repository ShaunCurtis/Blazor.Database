using Blazor.Database.Data;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Services
{
    public class DummyWeatherService
    {
        public int FormStep { get; set; } = 1;

        public Weather Record { get; set; } = new Weather { ID = Guid.NewGuid(), Date = DateTimeOffset.Now, TemperatureC = 10, Summary = "Balmy" };

        public ValueTask NewRecordAsync()
            => ValueTask.CompletedTask;
    }
}
