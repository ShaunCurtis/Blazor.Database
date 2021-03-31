/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace Blazor.SPA.Services
{
    public class FactoryWASMDataService :
        FactoryDataService,
        IFactoryDataService
    {

        protected HttpClient HttpClient { get; set; }

        public FactoryWASMDataService(IConfiguration configuration, HttpClient httpClient) : base(configuration)
            => this.HttpClient = httpClient;

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>()
            => await this.HttpClient.GetFromJsonAsync<List<TRecord>>($"{GetRecordName<TRecord>()}/list");

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>(Paginator paginator)
        {
            var response = await this.HttpClient.PostAsJsonAsync($"{GetRecordName<TRecord>()}/listpaged", paginator);
            return await response.Content.ReadFromJsonAsync<List<TRecord>>();
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<TRecord> GetRecordAsync<TRecord>(int id)
        {
            var response = await this.HttpClient.PostAsJsonAsync($"{GetRecordName<TRecord>()}/read", id);
            var result = await response.Content.ReadFromJsonAsync<TRecord>();
            return result;
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<int> GetRecordListCountAsync<TRecord>()
            => await this.HttpClient.GetFromJsonAsync<int>($"{GetRecordName<TRecord>()}/count");

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"{GetRecordName<TRecord>()}/update", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> CreateRecordAsync<TRecord>(TRecord record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"{GetRecordName<TRecord>()}/create", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"{GetRecordName<TRecord>()}/update", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }

        protected string GetRecordName<TRecord>() where TRecord : class, IDbRecord<TRecord>, new()
            => new TRecord().GetType().Name;

    }
}
