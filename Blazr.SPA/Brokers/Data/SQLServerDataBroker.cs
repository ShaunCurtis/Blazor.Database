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
    /// Blazor Server Data Broker
    /// Interfaces through EF with SQL Database
    /// </summary>
    public class SQLServerDataBroker<TDbContext> :
        BaseDataBroker,
        IDataBroker
        where TDbContext : DbContext
    {

        protected virtual IDbContextFactory<TDbContext> DBContext { get; set; } = null;

        public SQLServerDataBroker(IConfiguration configuration, IDbContextFactory<TDbContext> dbContext)
            => this.DBContext = dbContext;

        public override async ValueTask<List<TRecord>> SelectAllRecordsAsync<TRecord>()
        {
            using var dbContext = this.DBContext.CreateDbContext();
            var list = await dbContext
            .GetDbSet<TRecord>()
            .ToListAsync() 
            ?? new List<TRecord>();
            return list;
        }

        public override async ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(RecordPagingData pagingData)
        {
            using var dbContext = this.DBContext.CreateDbContext();
            var dbset = dbContext
                .GetDbSet<TRecord>();

            var isSortable = typeof(TRecord).GetProperty(pagingData.SortColumn) != null;
            List<TRecord> list;
            if (pagingData.Sort && isSortable)
            {
                list = await dbset
                    .OrderBy(pagingData.SortDescending ? $"{pagingData.SortColumn} descending" : pagingData.SortColumn)
                    .Skip(pagingData.StartRecord)
                    .Take(pagingData.PageSize)
                    .ToListAsync() 
                    ?? new List<TRecord>();
            }
            else
            {
                list = await dbset
                    .Skip(pagingData.StartRecord)
                    .Take(pagingData.PageSize)
                    .ToListAsync() 
                    ?? new List<TRecord>();
            }
            return list;
        }

        public override async ValueTask<TRecord> SelectRecordAsync<TRecord>(Guid id)
        {
            using var dbContext = this.DBContext.CreateDbContext();
            var list = await dbContext
                .GetDbSet<TRecord>()
                .FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;

            return list;
        }

        public override async ValueTask<int> SelectRecordListCountAsync<TRecord>()
        {
            using var dbContext = this.DBContext.CreateDbContext();
            var count = await dbContext
                .GetDbSet<TRecord>()
                .CountAsync();

            return count;
        }

        public override async ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            using var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Modified;
            var result = await this.UpdateContext(context);
            return result;
        }

        public override async ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record)
        {
            using var context = this.DBContext.CreateDbContext();
            context.GetDbSet<TRecord>().Add(record);
            var result = await this.UpdateContext(context);
            return result;
        }

        public override async ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            using var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Deleted;
            var result = await this.UpdateContext(context);
            return result;
        }

        /// Helper method to update the context and return a DBTaskResult
        protected async Task<DbTaskResult> UpdateContext(DbContext context)
            => await context.SaveChangesAsync() > 0 ? DbTaskResult.OK() : DbTaskResult.NotOK();
    }
}
