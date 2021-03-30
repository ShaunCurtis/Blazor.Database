/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.SPA.Services
{
    /// <summary>
    /// Interface definition for a Controller Service that uses the FactoryDataService
    /// Holds the working record and the working recordset for TRecord
    /// and all the data associated with CRUD operations for TRecord
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public interface IFactoryControllerService<TRecord>
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        /// <summary>
        /// Unique ID for the Service
        /// Helps in debugging
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Current Record - for CRUD Component Operations
        /// </summary>
        public TRecord Record { get; }

        /// <summary>
        /// Current List of Records - used in Listing Components 
        /// </summary>
        public List<TRecord> Records { get; }

        /// <summary>
        /// Property to get current record count
        /// </summary>
        public int RecordCount => this.Records?.Count ?? 0;

        /// <summary>
        /// Property to expose the Record ID.
        /// should be implemented to return 0 if the record is null
        /// </summary>
        public int RecordId { get; }

        /// <summary>
        /// Property to expose the Record GUID.
        /// should be implemented to return 0 if the record is null
        /// </summary>
        public Guid RecordGUID { get; }

        /// <summary>
        /// Property of DbTaskResult set when a CRUD operation is called
        /// The UI can build an alert/confirmation method from the information provided
        /// </summary>
        public DbTaskResult DbResult { get; }

        /// <summary>
        /// Property for the Paging object that controls paging and interfaces with the UI Paging Control 
        /// </summary>
        public Paginator Paginator { get; }

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
        /// Event raised when the Record has Changed
        /// </summary>
        public event EventHandler RecordHasChanged;

        /// <summary>
        /// Event raised when the List has Changed
        /// </summary>
        public event EventHandler ListHasChanged;

        /// <summary>
        /// Method to Reset the Service to New condition
        /// </summary>
        public Task Reset();

        /// <summary>
        /// Method to reset the record to new
        /// </summary>
        /// <returns></returns>
        public Task ResetRecordAsync();

        /// <summary>
        /// Method to rest the list to empty
        /// </summary>
        /// <returns></returns>
        public Task ResetListAsync();

        public Task GetRecordsAsync() => Task.CompletedTask;

        /// <summary>
        /// Method to Update or Add the Database Record
        /// Builds the record from the RecordCollection if one is not provided
        /// </summary>
        /// <returns></returns>
        public Task<bool> SaveRecordAsync();

        /// <summary>
        /// Method to get a Record from the Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> GetRecordAsync(int id);

        /// <summary>
        /// Method to get a new Record
        /// </summary>
        /// <returns></returns>
        public Task<bool> NewRecordAsync();

        /// <summary>
        /// Method to delete the current Record
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteRecordAsync();
    }
}
