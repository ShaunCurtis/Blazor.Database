/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace Blazor.Database.Services
{
    public class FactoryWASMDataService :
        FactoryDataService,
        IFactoryDataService
    {

        protected HttpClient HttpClient { get; set; }

        public FactoryWASMDataService(IConfiguration configuration, HttpClient httpClient) : base(configuration)
        {
            this.HttpClient = httpClient;

            // Debug.WriteLine($"==> New Instance {this.ToString()} ID:{this.ServiceID.ToString()} ");
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>()
        {
            var recname = new TRecord().GetType().Name;
            return await this.HttpClient.GetFromJsonAsync<List<TRecord>>($"{recname}/list");
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>(int page, int pagesize)
        {
            var paging = new Paginator(page, pagesize);
            var recname = new TRecord().GetType().Name;
            var response = await this.HttpClient.PostAsJsonAsync($"{recname}/listpaged", paging );
            var result = await response.Content.ReadFromJsonAsync<List<TRecord>>();
            return result;
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<TRecord> GetRecordAsync<TRecord>(int id)
        {
            var recname = new TRecord().GetType().Name;
            var response = await this.HttpClient.PostAsJsonAsync($"{recname}/read", id);
            var result = await response.Content.ReadFromJsonAsync<TRecord>();
            return result;
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<int> GetRecordListCountAsync<TRecord>()
        {
            var recname = new TRecord().GetType().Name;
            return await this.HttpClient.GetFromJsonAsync<int>($"{recname}/count");
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            var recname = new TRecord().GetType().Name;
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"{recname}/update", record);
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
            var recname = new TRecord().GetType().Name;
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"{recname}/create", record);
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
            var recname = new TRecord().GetType().Name;
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"{recname}/update", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }
    }
}
