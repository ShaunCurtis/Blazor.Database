/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazr.SPA.Data
{
    /// <summary>
    /// Abstract base class for a DataBroker implmeneting IDataBroker 
    /// </summary>
    public abstract class BaseDataBroker : IDataBroker
    {
        public virtual ValueTask<List<TRecord>> SelectAllRecordsAsync<TRecord>() where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(new List<TRecord>());

        public virtual ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(RecordPagingData paginatorData) where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(new List<TRecord>());

        public virtual ValueTask<TRecord> SelectRecordAsync<TRecord>(Guid id) where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(new TRecord());

        public virtual ValueTask<int> SelectRecordListCountAsync<TRecord>() where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(0);

        public virtual ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(new DbTaskResult() { IsOK = false, Type = MessageType.NotImplemented, Message = "Method not implemented" });

        public virtual ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(new DbTaskResult() { IsOK = false, Type = MessageType.NotImplemented, Message = "Method not implemented" });

        public virtual ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(new DbTaskResult() { IsOK = false, Type = MessageType.NotImplemented, Message = "Method not implemented" });
    }
}
