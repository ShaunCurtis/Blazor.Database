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
    public class WeatherServerDataService
        : IWeatherDataService
    {
        public InMemoryWeatherDbContext _dbContext { get; set; } = null;

        public WeatherServerDataService(IConfiguration configuration, InMemoryWeatherDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public Task<List<WeatherForecast>> GetRecordListAsync()
            => Task.FromResult(this._dbContext.WeatherForecast.ToList());

        public Task<WeatherForecast> GetRecordAsync(int id)
            => Task.FromResult(this._dbContext.WeatherForecast.FirstOrDefault(item => item.ID == id));

        public Task<int> GetRecordListCountAsync()
            => Task.FromResult(this._dbContext.WeatherForecast.Count());

        public Task<DbTaskResult> UpdateRecordAsync(WeatherForecast record)
        {
            var result = new DbTaskResult();
            var ret = this._dbContext.SaveChanges();
            if (ret == 1)
            {
                result.IsOK = true;
                result.Message = "Record Update";
                result.Type = MessageType.Success;
            }
            else
            {
                result.IsOK = false;
                result.Message = "Record Not Update";
                result.Type = MessageType.Danger;
            }
            return Task.FromResult(result);
        }

        public Task<DbTaskResult> CreateRecordAsync(WeatherForecast record)
        {
            var max = this._dbContext.WeatherForecast.Max(item => item.ID);
            record.ID = max + 1;
            this._dbContext.WeatherForecast.Add(record);
            var result = new DbTaskResult();
            var ret = this._dbContext.SaveChanges();
            if (ret == 1)
            {
                result.NewID = max + 1;
                result.IsOK = true;
                result.Message = "Record Created";
                result.Type = MessageType.Success;
            }
            else
            {
                result.IsOK = false;
                result.Message = "Record Not Created";
                result.Type = MessageType.Danger;
            }
            return Task.FromResult(result);
        }

        public Task<DbTaskResult> DeleteRecordAsync(WeatherForecast record)
        {
            this._dbContext.WeatherForecast.Remove(record);
            var result = new DbTaskResult();
            var ret = this._dbContext.SaveChanges();
            if (ret == 1)
            {
                result.IsOK = true;
                result.Message = "Record Deleted";
                result.Type = MessageType.Success;
            }
            else
            {
                result.IsOK = false;
                result.Message = "Record Not Deleted";
                result.Type = MessageType.Danger;
            }
            return Task.FromResult(result);
        }
    }
}
