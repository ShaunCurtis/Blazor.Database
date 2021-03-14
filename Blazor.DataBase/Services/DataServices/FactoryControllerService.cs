/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Database.Services
{
    /// <summary>
    /// Base implementation of a Controller Service
    /// Implements IFactoryControllerService interfaces
    /// Holds the working record and the working recordset for TRecord,
    /// all the data associated with CRUD operations for TRecord
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract partial class FactoryControllerService<TRecord, TDbContext> :
        IDisposable,
        IFactoryControllerService<TRecord, TDbContext>
        where TRecord : class, IDbRecord<TRecord>, new()
        where TDbContext : DbContext
    {
        public Guid Id { get; } = Guid.NewGuid();

        public TRecord Record
        {
            get => _record;
            private set
            {
                this._record = value;
                this.RecordHasChanged?.Invoke(value, EventArgs.Empty);
            }
        }

        private TRecord _record = null;

        public List<TRecord> Records
        {
            get => _records;
            private set
            {
                this._records = value;
                this.ListHasChanged?.Invoke(value, EventArgs.Empty);
            }
        }

        private List<TRecord> _records = null;


        public int RecordId => this.Record?.ID ?? 0;

        public Guid RecordGUID => this.Record?.GUID ?? Guid.Empty;

        public DbTaskResult DbResult { get; set; } = new DbTaskResult();

        protected IFactoryDataService<TDbContext> DataService { get; set; }

        public event EventHandler RecordHasChanged;
        public event EventHandler ListHasChanged;

        public virtual void Dispose()
        {
        }

        public Task Reset()
        {
            this.Record = null;
            this.Records = null;
            return Task.CompletedTask;
        }

        public Task ResetListAsync()
        {
            this.Records = null;
            return Task.CompletedTask;
        }

        public Task ResetRecordAsync()
        {
            this.Record = null;
            return Task.CompletedTask;
        }

        public async Task GetRecordsAsync()
        {
            this.Records = await DataService.GetRecordListAsync<TRecord>();
            this.ListHasChanged?.Invoke(null, EventArgs.Empty);
        }

        public Task GetNewRecordAsync()
        {
            this.Record = new TRecord();
            this.RecordHasChanged?.Invoke(this.Record, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public async Task GetRecordAsync(int id)
        {
            if (id > 0)
                this.Record = await DataService.GetRecordAsync<TRecord>(id);
            else
                this.Record = new TRecord();
            this.RecordHasChanged?.Invoke(this.Record, EventArgs.Empty);
        }

        public async Task<int> GetRecordListCountAsync()
            => await DataService.GetRecordListCountAsync<TRecord>();


        public async Task<bool> SaveRecordAsync()
        {
            if (this.RecordId == -1)
                this.DbResult = await DataService.CreateRecordAsync<TRecord>(this.Record);
            else
                this.DbResult = await DataService.UpdateRecordAsync(this.Record);
            await this.GetRecordsAsync();
            return this.DbResult.IsOK;
        }
    }
}
