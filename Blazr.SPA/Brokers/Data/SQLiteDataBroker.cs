/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Blazr.SPA.Data
{
    /// <summary>
    /// Blazor In-Memory SQLite Server Data Broker
    /// Interfaces through EF with SQL Database
    /// </summary>
    public class SQLiteDataBroker<TDbContext> :
        BaseDataBroker,
        IDataBroker
        where TDbContext : DbContext
    {

        protected virtual IDbContextFactory<TDbContext> DBContext { get; set; } = null;

        private DbContext _dbContext;

        public SQLiteDataBroker(IConfiguration configuration, IDbContextFactory<TDbContext> dbContext)
        {
            this.DBContext = dbContext;
            _dbContext = this.DBContext.CreateDbContext();
            // Debug.WriteLine($"==> New Instance {this.ToString()} ID:{this.ServiceID.ToString()} ");
        }

        public override async ValueTask<List<TRecord>> SelectAllRecordsAsync<TRecord>()
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            return await dbset.ToListAsync() ?? new List<TRecord>();
        }

        public override async ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(RecordPagingData pagingData)
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            var isSortable = typeof(TRecord).GetProperty(pagingData.SortColumn) != null;
            if (pagingData.Sort && isSortable)
            {
                var list = await dbset
                    .OrderBy(pagingData.SortDescending ? $"{pagingData.SortColumn} descending" : pagingData.SortColumn)
                    .Skip(pagingData.StartRecord)
                    .Take(pagingData.PageSize).ToListAsync() ?? new List<TRecord>();
                return list;
            }
            else
            {
                var list = await dbset
                    .Skip(pagingData.StartRecord)
                    .Take(pagingData.PageSize).ToListAsync() ?? new List<TRecord>();
                return list;
            }
        }

        public override async ValueTask<TRecord> SelectRecordAsync<TRecord>(Guid id)
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            return await dbset.FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;
        }

        public override async ValueTask<int> SelectRecordListCountAsync<TRecord>()
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            return await dbset.CountAsync();
        }

        public override async ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            _dbContext.Entry(record).State = EntityState.Modified;
            var x = await _dbContext.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success };
        }

        public override async ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record)
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            dbset.Add(record);
            var x = await _dbContext.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success };
        }

        public override async ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            _dbContext.Entry(record).State = EntityState.Deleted;
            var x = await _dbContext.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success };
        }
    }
}
