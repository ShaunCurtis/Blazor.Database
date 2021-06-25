/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.SPA.Brokers
{
    /// <summary>
    /// Main database broker interface 
    /// </summary>
    public interface IDataBroker
    {

        public ValueTask<List<TRecord>> SelectAllRecordsAsync<TRecord>() where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(RecordPagingData pagingData) where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<TRecord> SelectRecordAsync<TRecord>(Guid id) where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<int> SelectRecordListCountAsync<TRecord>() where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new();

    }
}
