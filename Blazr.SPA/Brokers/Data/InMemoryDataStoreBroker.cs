/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Blazr.SPA.Data
{
    /// <summary>
    /// Blazor Server Data Broker
    /// Interfaces through EF with SQL Database
    /// </summary>
    public class InMemoryDataStoreBroker<TContext> :
        BaseDataBroker,
        IDataBroker
        where TContext : IInMemoryDataStore
    {
        protected virtual IInMemoryDataStore DataContext { get; set; } = null;

        public InMemoryDataStoreBroker(IInMemoryDataStore dataContext)
            => this.DataContext = dataContext;

        public override ValueTask<List<TRecord>> SelectAllRecordsAsync<TRecord>()
            => ValueTask.FromResult<List<TRecord>>(DataContext
            .GetDataSet<TRecord>()
            .ToList());

        public override ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(RecordPagingData pagingData)
        {
            var dbSet = DataContext
                .GetDataSet<TRecord>()
                .ToList();
            if (pagingData.Sort)
            {
                dbSet = dbSet
                    .AsQueryable()
                    .OrderBy(pagingData.SortDescending ? $"{pagingData.SortColumn} descending" : pagingData.SortColumn)
                    .ToList();
            }
            return ValueTask.FromResult ( dbSet
                .Skip(pagingData.StartRecord)
                .Take(pagingData.PageSize)
                .ToList()
                );
        }


        public override ValueTask<TRecord> SelectRecordAsync<TRecord>(Guid id)
            => ValueTask.FromResult<TRecord>(DataContext
                .GetDataSet<TRecord>()
                .FirstOrDefault(item => ((IDbRecord<TRecord>)item).ID == id));

        public override ValueTask<int> SelectRecordListCountAsync<TRecord>()
            => ValueTask.FromResult<int>(DataContext
                .GetDataSet<TRecord>()
                .Count());

        public override ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            var result = this.DataContext.GetDataSet<TRecord>().Update(record);
            var dbResult = new DbTaskResult() { IsOK = result, Message = "Record Updated" };
            return ValueTask.FromResult<DbTaskResult>(dbResult);
        }

        public override ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record)
        {
            var result = this.DataContext.GetDataSet<TRecord>().Insert(record);
            var dbResult = new DbTaskResult() { IsOK = result, Message = "Record Added" };
            return ValueTask.FromResult<DbTaskResult>(dbResult);
        }

        public override ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            var result = this.DataContext.GetDataSet<TRecord>().Delete(record);
            var dbResult = new DbTaskResult() { IsOK = result, Message = "Record Deleted" };
            return ValueTask.FromResult<DbTaskResult>(dbResult);
        }
    }
}
