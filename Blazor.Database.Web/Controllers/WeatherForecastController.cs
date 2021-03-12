using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC = Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blazor.Database.Services;
using Blazor.Database.Data;

namespace Blazor.Database.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        protected IWeatherDataService DataService { get; set; }

        private readonly ILogger<WeatherForecastController> logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherDataService dataService)
        {
            this.DataService = dataService;
            this.logger = logger;
        }

        [MVC.Route("weatherforecast/list")]
        [HttpGet]
        public async Task<List<WeatherForecast>> GetList() => await DataService.GetRecordListAsync();

        [MVC.Route("weatherforecast/count")]
        [HttpGet]
        public async Task<int> Count() => await DataService.GetRecordListCountAsync();

        [MVC.Route("weatherforecast/get")]
        [HttpGet]
        public async Task<WeatherForecast> GetRec(int id) => await DataService.GetRecordAsync(id);

        [MVC.Route("weatherforecast/read")]
        [HttpPost]
        public async Task<WeatherForecast> Read([FromBody]int id) => await DataService.GetRecordAsync(id);

        [MVC.Route("weatherforecast/update")]
        [HttpPost]
        public async Task<DbTaskResult> Update([FromBody]WeatherForecast record) => await DataService.UpdateRecordAsync(record);

        [MVC.Route("weatherforecast/create")]
        [HttpPost]
        public async Task<DbTaskResult> Create([FromBody]WeatherForecast record) => await DataService.CreateRecordAsync(record);

        [MVC.Route("weatherforecast/delete")]
        [HttpPost]
        public async Task<DbTaskResult> Delete([FromBody] WeatherForecast record) => await DataService.DeleteRecordAsync(record);
    }
}
