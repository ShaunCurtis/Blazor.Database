/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Database.Services
{
    public class WeatherControllerService
    {
        public IWeatherDataService DataService { get; set; }

        public DbTaskResult DbResult { get; private set; }

        public WeatherControllerService(IWeatherDataService weatherDataService)
        {
            this.DataService = weatherDataService;
        }

        public int RecordId => _recordId;

        public WeatherForecast Record { get; set; }

        public List<WeatherForecast> Records { get; set; }


        public event EventHandler RecordChanged;

        public event EventHandler RecordListChanged;

        private int _recordId = -1;

        public bool IsNewRecord => this.RecordId == -1;

        public async Task GetRecordsAsync()
        {
            this.Records = await DataService.GetRecordListAsync();
            this.RecordListChanged?.Invoke(null, EventArgs.Empty);
        }

        public Task GetNewRecordAsync()
        {
            this._recordId = 0;
            this.Record = new WeatherForecast();
            this.RecordChanged?.Invoke(this.Record, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public async Task GetRecordAsync(int id)
        {
            this._recordId = id;
            if (id > 0)
                this.Record = await DataService.GetRecordAsync(id);
            else
                this.Record = new WeatherForecast();
            this.RecordChanged?.Invoke(this.Record, EventArgs.Empty);
        }

        public async Task<int> GetRecordListCountAsync()
            => await DataService.GetRecordListCountAsync();

        public async Task SaveRecordAsync()
        {
            if (this.RecordId == -1)
            {
                this.DbResult = await DataService.CreateRecordAsync(this.Record);
                this._recordId = DbResult.NewID;
            }
            else
                this.DbResult = await DataService.UpdateRecordAsync(this.Record);
            await this.GetRecordsAsync();
        }

        public async Task DeleteRecordAsync()
        {
            this.DbResult = await DataService.DeleteRecordAsync(this.Record);
            this._recordId = -1;
            this.Record = new WeatherForecast();
            this.RecordChanged?.Invoke(null, EventArgs.Empty);
            await this.GetRecordsAsync();
        }
    }
}
