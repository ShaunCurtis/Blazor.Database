using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Blazor.Database.Data;

namespace Blazor.Database.Services
{
    public class WeatherWASMDataService 
        : IWeatherDataService
    {
        protected HttpClient HttpClient { get; set; }

        public WeatherWASMDataService(HttpClient httpClient)
            => this.HttpClient = httpClient;

        /// <summary>
        /// Gets a record from the API
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WeatherForecast> GetRecordAsync(int id)
        {
            //return await this.HttpClient.GetFromJsonAsync<DbWeatherForecast>($"weatherforecast/getrec?id={id}");
            var response = await this.HttpClient.PostAsJsonAsync($"WeatherForecast/read", id);
            var result = await response.Content.ReadFromJsonAsync<WeatherForecast>();
            return result;
        }

        /// <summary>
        /// Gets a record list from the API
        /// </summary>
        /// <returns></returns>
        public async Task<List<WeatherForecast>> GetRecordListAsync() => await this.HttpClient.GetFromJsonAsync<List<WeatherForecast>>($"WeatherForecast/list");

        /// <summary>
        /// Gets a record list from the API
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecordListCountAsync() => await this.HttpClient.GetFromJsonAsync<int>($"WeatherForecast/count");

        /// <summary>
        /// uPDATES a record through the API
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task<DbTaskResult> UpdateRecordAsync(WeatherForecast record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<WeatherForecast>($"WeatherForecast/update", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }

        /// <summary>
        /// Creates a record through the API
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task<DbTaskResult> CreateRecordAsync(WeatherForecast record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<WeatherForecast>($"WeatherForecast/create", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }

        /// <summary>
        /// Deletes a record through the API
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DbTaskResult> DeleteRecordAsync(WeatherForecast record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<WeatherForecast> ($"WeatherForecast/update", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }
    }
}
