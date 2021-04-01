/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC = Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blazor.Database.Services;
using Blazor.Database.Data;
using Blazor.SPA.Services;
using Blazor.SPA.Data;

namespace Blazor.Database.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        protected IFactoryDataService DataService { get; set; }

        private readonly ILogger<WeatherForecastController> logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFactoryDataService dataService)
        {
            this.DataService = dataService;
            this.logger = logger;
        }

        [MVC.Route("/api/weatherforecast/list")]
        [HttpGet]
        public async Task<List<WeatherForecast>> GetList() => await DataService.GetRecordListAsync<WeatherForecast>();

        [MVC.Route("/api/weatherforecast/listpaged")]
        [HttpPost]
        public async Task<List<WeatherForecast>> Read([FromBody] PaginatorData data) => await DataService.GetRecordListAsync<WeatherForecast>(data);

        [MVC.Route("/api/weatherforecast/count")]
        [HttpGet]
        public async Task<int> Count() => await DataService.GetRecordListCountAsync<WeatherForecast>();

        [MVC.Route("/api/weatherforecast/get")]
        [HttpGet]
        public async Task<WeatherForecast> GetRec(int id) => await DataService.GetRecordAsync<WeatherForecast>(id);

        [MVC.Route("/api/weatherforecast/read")]
        [HttpPost]
        public async Task<WeatherForecast> Read([FromBody]int id) => await DataService.GetRecordAsync<WeatherForecast>(id);

        [MVC.Route("/api/weatherforecast/update")]
        [HttpPost]
        public async Task<DbTaskResult> Update([FromBody]WeatherForecast record) => await DataService.UpdateRecordAsync<WeatherForecast>(record);

        [MVC.Route("/api/weatherforecast/create")]
        [HttpPost]
        public async Task<DbTaskResult> Create([FromBody]WeatherForecast record) => await DataService.CreateRecordAsync<WeatherForecast>(record);

        [MVC.Route("/api/weatherforecast/delete")]
        [HttpPost]
        public async Task<DbTaskResult> Delete([FromBody] WeatherForecast record) => await DataService.DeleteRecordAsync<WeatherForecast>(record);
    }
}
