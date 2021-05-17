/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Blazor.SPA.Brokers
{
    /// <summary>
    /// Abstract base class for a DataBroker implmeneting IDataBroker 
    /// </summary>
    public abstract class BaseDataBroker: IDataBroker
    {
        public virtual ValueTask<List<TRecord>> SelectAllRecordsAsync<TRecord>() where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(new List<TRecord>());

        public virtual ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(PaginatorData paginatorData) where TRecord : class, IDbRecord<TRecord>, new()
            => ValueTask.FromResult(new List<TRecord>());

        public virtual ValueTask<TRecord> SelectRecordAsync<TRecord>(int id) where TRecord : class, IDbRecord<TRecord>, new()
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
