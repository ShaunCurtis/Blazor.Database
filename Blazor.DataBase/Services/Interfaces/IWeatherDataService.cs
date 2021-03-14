/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Database.Services
{
    public interface IWeatherDataService
    {

        public Task<List<WeatherForecast>> GetRecordListAsync();

        public Task<WeatherForecast> GetRecordAsync(int id);

        public Task<int> GetRecordListCountAsync();

        public Task<DbTaskResult> UpdateRecordAsync(WeatherForecast record);

        public Task<DbTaskResult> CreateRecordAsync(WeatherForecast record);

        public Task<DbTaskResult> DeleteRecordAsync(WeatherForecast record);

    }
}
