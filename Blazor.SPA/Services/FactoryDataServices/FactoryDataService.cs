/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.Extensions.Configuration;
using Blazor.SPA.Data;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Blazor.SPA.Services
{
    /// <summary>
    /// Abstract base class for a Dactory Data Service implmenting IFactoryDataService 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class FactoryDataService: IFactoryDataService
    {
        /// <summary>
        /// Guid for Service
        /// </summary>
        public Guid ServiceID { get; } = Guid.NewGuid();

        /// <summary>
        /// Access to the Application Configuration data
        /// </summary>
        public IConfiguration AppConfiguration { get; set; }

        public FactoryDataService(IConfiguration configuration) => this.AppConfiguration = configuration;

        /// <summary>
        /// Method to get the Record List
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TRecord>> GetRecordListAsync<TRecord>() where TRecord : class, IDbRecord<TRecord>, new()
            => Task.FromResult(new List<TRecord>());

        /// <summary>
        /// Method to get the Record List
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TRecord>> GetRecordListAsync<TRecord>(int page, int pagesize) where TRecord : class, IDbRecord<TRecord>, new()
            => Task.FromResult(new List<TRecord>());

        /// <summary>
        /// Method to get the Record List
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TRecord>> GetRecordListAsync<TRecord>(int page, int pagesize, Sortor sorter, Filtor filter) where TRecord : class, IDbRecord<TRecord>, new()
            => Task.FromResult(new List<TRecord>());

        /// <summary>
        /// Method to get a Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<TRecord> GetRecordAsync<TRecord>(int id) where TRecord : class, IDbRecord<TRecord>, new()
            => Task.FromResult(new TRecord());

        /// <summary>
        /// Method to get the current record count
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> GetRecordListCountAsync<TRecord>() where TRecord : class, IDbRecord<TRecord>, new()
            => Task.FromResult(0);

        /// <summary>
        /// Method to update a record
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public virtual Task<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new()
            => Task.FromResult(new DbTaskResult() { IsOK = false, Type = MessageType.NotImplemented, Message = "Method not implemented" });

        /// <summary>
        /// method to add a record
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public virtual Task<DbTaskResult> CreateRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new()
            => Task.FromResult(new DbTaskResult() { IsOK = false, Type = MessageType.NotImplemented, Message = "Method not implemented" });

        /// <summary>
        /// Method to delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new()
            => Task.FromResult(new DbTaskResult() { IsOK = false, Type = MessageType.NotImplemented, Message = "Method not implemented" });
    }
}
