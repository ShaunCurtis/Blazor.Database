/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
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

        public ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(PaginatorData paginatorData) where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<TRecord> SelectRecordAsync<TRecord>(int id) where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<int> SelectRecordListCountAsync<TRecord>() where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new();

        public ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record) where TRecord : class, IDbRecord<TRecord>, new();

    }
}
