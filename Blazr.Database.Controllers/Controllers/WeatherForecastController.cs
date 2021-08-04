/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazr.Database.Core;
using Blazr.SPA.Brokers;
using Blazr.SPA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVC = Microsoft.AspNetCore.Mvc;

namespace Blazr.Database.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        protected IDataBroker DataService { get; set; }

        private readonly ILogger<WeatherForecastController> logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDataBroker dataService)
        {
            this.DataService = dataService;
            this.logger = logger;
        }

        [MVC.Route("/api/weatherforecast/list")]
        [HttpGet]
        public async Task<List<WeatherForecast>> GetList() => await DataService.SelectAllRecordsAsync<WeatherForecast>();

        [MVC.Route("/api/weatherforecast/listpaged")]
        [HttpPost]
        public async Task<List<WeatherForecast>> Read([FromBody] RecordPagingData data) => await DataService.SelectPagedRecordsAsync<WeatherForecast>(data);

        [MVC.Route("/api/weatherforecast/count")]
        [HttpGet]
        public async Task<int> Count() => await DataService.SelectRecordListCountAsync<WeatherForecast>();

        [MVC.Route("/api/weatherforecast/get")]
        [HttpGet]
        public async Task<WeatherForecast> GetRec(Guid id) => await DataService.SelectRecordAsync<WeatherForecast>(id);

        [MVC.Route("/api/weatherforecast/read")]
        [HttpPost]
        public async Task<WeatherForecast> Read([FromBody] Guid id) => await DataService.SelectRecordAsync<WeatherForecast>(id);

        [MVC.Route("/api/weatherforecast/update")]
        [HttpPost]
        public async Task<DbTaskResult> Update([FromBody] WeatherForecast record) => await DataService.UpdateRecordAsync<WeatherForecast>(record);

        [MVC.Route("/api/weatherforecast/create")]
        [HttpPost]
        public async Task<DbTaskResult> Create([FromBody] WeatherForecast record) => await DataService.InsertRecordAsync<WeatherForecast>(record);

        [MVC.Route("/api/weatherforecast/delete")]
        [HttpPost]
        public async Task<DbTaskResult> Delete([FromBody] WeatherForecast record) => await DataService.DeleteRecordAsync<WeatherForecast>(record);
    }
}
