/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.SPA.Services
{
    /// <summary>
    /// Base implementation of a Controller Service
    /// Implements IFactoryControllerService interfaces
    /// Holds the working record and the working recordset for TRecord,
    /// all the data associated with CRUD operations for TRecord
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class FactoryControllerService<TRecord> :
        IDisposable,
        IFactoryControllerService<TRecord>
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        /// <summary>
        /// GUID for the instance
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Property for Record of TRecord
        /// Should be used by Create/Read/Update UI Components
        /// </summary>
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

        /// <summary>
        /// Property for List of Record of TRecord
        /// Should be used by List UI Components
        /// </summary>
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


        /// <summary>
        /// Property exposing the ID of the current record
        /// </summary>
        public int RecordId => this.Record?.ID ?? 0;

        /// <summary>
        /// Property exposing the GUID of the current record
        /// </summary>
        public Guid RecordGUID => this.Record?.GUID ?? Guid.Empty;

        /// <summary>
        /// Property exposing the result of the last CRUD operation
        /// Can be used in the UI for notifications
        /// </summary>
        public DbTaskResult DbResult { get; set; } = new DbTaskResult();

        /// <summary>
        /// Property for the Paging object that controls paging and interfaces with the UI Paging Control 
        /// </summary>
        public Paginator Paginator { get; private set; }

        /// <summary>
        /// Boolean Property to check if a record exists
        /// </summary>
        public bool IsRecord => this.Record != null && this.RecordId > -1;

        /// <summary>
        /// Boolean Property to check if a record exists
        /// </summary>
        public bool HasRecords => this.Records != null && this.Records.Count > 0;

        /// <summary>
        /// Boolean Property to check if a New record exists 
        /// </summary>
        public bool IsNewRecord => this.IsRecord && this.RecordId == -1;

        /// <summary>
        /// Data Service for data access
        /// </summary>
        protected IFactoryDataService DataService { get; set; }

        /// <summary>
        /// Event triggered when the Record has changed
        /// </summary>
        public event EventHandler RecordHasChanged;

        /// <summary>
        /// Event triggered when the RecordList has Changed
        /// </summary>
        public event EventHandler ListHasChanged;

        public FactoryControllerService(IFactoryDataService factoryDataService)
        {
            this.DataService = factoryDataService;
            this.Paginator = new Paginator(10, 5);
            this.Paginator.PageChanged += this.OnPageChanged;
        }

        /// <summary>
        /// Method to reset the service
        /// </summary>
        /// <returns></returns>
        public Task Reset()
        {
            this.Record = null;
            this.Records = null;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method to reset the record list
        /// </summary>
        /// <returns></returns>
        public Task ResetListAsync()
        {
            this.Records = null;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method to reset the Record
        /// </summary>
        /// <returns></returns>
        public Task ResetRecordAsync()
        {
            this.Record = null;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method to get a recordset
        /// </summary>
        /// <returns></returns>
        public async Task GetRecordsAsync()
        {
            this.Records = await DataService.GetRecordListAsync<TRecord>(this.Paginator);
            this.Paginator.RecordCount = await GetRecordListCountAsync();
            this.ListHasChanged?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Method to get a record
        /// if id < 1 will create a new record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> GetRecordAsync(int id)
        {
            if (id > 0)
                this.Record = await DataService.GetRecordAsync<TRecord>(id);
            else
                this.Record = new TRecord();
            return this.IsRecord;
        }

        /// <summary>
        /// Method to get the current record count
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecordListCountAsync()
            => await DataService.GetRecordListCountAsync<TRecord>();


        /// <summary>
        /// Method to Save the current record
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveRecordAsync()
        {
            if (this.RecordId == -1)
                this.DbResult = await DataService.CreateRecordAsync<TRecord>(this.Record);
            else
                this.DbResult = await DataService.UpdateRecordAsync(this.Record);
            await this.GetRecordsAsync();
            return this.DbResult.IsOK;
        }

        /// <summary>
        /// Method to delete the current Record
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteRecordAsync()
        {
            this.DbResult = await DataService.DeleteRecordAsync<TRecord>(this.Record);
            return this.DbResult.IsOK;
        }

        /// <summary>
        /// Event Handler for dealing with Page Change Event from Paginator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void OnPageChanged(object sender, EventArgs e)
            => await this.GetRecordsAsync();

        /// <summary>
        /// Method for inherited class to trigger a RecordHasChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NotifyRecordChanged(object sender, EventArgs e)
            => this.RecordHasChanged?.Invoke(sender, e);

        /// <summary>
        /// Method for inherited class to trigger a ListHasChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NotifyListChanged(object sender, EventArgs e)
            => this.ListHasChanged?.Invoke(sender, e);

        /// <summary>
        /// Method to get a New Record
        /// </summary>
        /// <returns></returns>
        public Task<bool> NewRecordAsync()
        {
            this.Record = default(TRecord);
            return Task.FromResult(false);
        }

        /// <summary>
        /// IDisposable Implementation
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
