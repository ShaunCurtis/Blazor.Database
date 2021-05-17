/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace Blazor.SPA.Brokers
{
    /// <summary>
    /// API Data Broker
    /// Calls the server controller
    /// </summary>
    public class APIDataBroker :
        BaseDataBroker,
        IDataBroker
    {

        protected HttpClient HttpClient { get; set; }

        public APIDataBroker(IConfiguration configuration, HttpClient httpClient)
            => this.HttpClient = httpClient;

        public override async ValueTask<List<TRecord>> SelectAllRecordsAsync<TRecord>()
            => await this.HttpClient.GetFromJsonAsync<List<TRecord>>($"/api/{GetRecordName<TRecord>()}/list");

        public override async ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(PaginatorData paginatorData)
        {
            var response = await this.HttpClient.PostAsJsonAsync($"/api/{GetRecordName<TRecord>()}/listpaged", paginatorData);
            return await response.Content.ReadFromJsonAsync<List<TRecord>>();
        }

        public override async ValueTask<TRecord> SelectRecordAsync<TRecord>(int id)
        {
            var response = await this.HttpClient.PostAsJsonAsync($"/api/{GetRecordName<TRecord>()}/read", id);
            var result = await response.Content.ReadFromJsonAsync<TRecord>();
            return result;
        }

        public override async ValueTask<int> SelectRecordListCountAsync<TRecord>()
            => await this.HttpClient.GetFromJsonAsync<int>($"/api/{GetRecordName<TRecord>()}/count");

        public override async ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"/api/{GetRecordName<TRecord>()}/update", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }

        public override async ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"/api/{GetRecordName<TRecord>()}/create", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }

        public override async ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            var response = await this.HttpClient.PostAsJsonAsync<TRecord>($"/api/{GetRecordName<TRecord>()}/update", record);
            var result = await response.Content.ReadFromJsonAsync<DbTaskResult>();
            return result;
        }

        protected string GetRecordName<TRecord>() where TRecord : class, new()
            => new TRecord().GetType().Name;

    }
}
