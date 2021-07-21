/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Data;
using Blazor.SPA.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Blazor.SPA.Brokers
{
    /// <summary>
    /// Blazor Server Data Broker
    /// Interfaces through EF with SQL Database
    /// </summary>
    public class ServerDataBroker<TDbContext> :
        BaseDataBroker,
        IDataBroker
        where TDbContext : DbContext
    {

        protected virtual IDbContextFactory<TDbContext> DBContext { get; set; } = null;

        public ServerDataBroker(IConfiguration configuration, IDbContextFactory<TDbContext> dbContext)
            => this.DBContext = dbContext;

        public override async ValueTask<List<TRecord>> SelectAllRecordsAsync<TRecord>()
        {
            var dbContext = this.DBContext.CreateDbContext();
            var list = await dbContext
            .GetDbSet<TRecord>()
            .ToListAsync() ?? new List<TRecord>();
            dbContext?.Dispose();
            return list;
        }

        public override async ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(RecordPagingData paginatorData)
        {
            var dbContext = this.DBContext.CreateDbContext();
            var startpage = paginatorData.Page <= 1
                ? 0
                : (paginatorData.Page - 1) * paginatorData.PageSize;
            
            var dbset = dbContext
                .GetDbSet<TRecord>();
            
            var isSortable = typeof(TRecord).GetProperty(paginatorData.SortColumn) != null;
            List<TRecord> list;
            if (isSortable)
            {
                list = await dbset
                    .OrderBy(paginatorData.SortDescending ? $"{paginatorData.SortColumn} descending" : paginatorData.SortColumn)
                    .Skip(startpage)
                    .Take(paginatorData.PageSize).ToListAsync() ?? new List<TRecord>();
            }
            else
            {
                list = await dbset
                    .Skip(startpage)
                    .Take(paginatorData.PageSize).ToListAsync() ?? new List<TRecord>();
            }
            dbContext?.Dispose();
            return list;
        }

        public override async ValueTask<TRecord> SelectRecordAsync<TRecord>(Guid id)
        {
            var dbContext = this.DBContext.CreateDbContext();
            var list = await dbContext
                .GetDbSet<TRecord>()
                .FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;
            
            dbContext?.Dispose();
            return list;
        }

        public override async ValueTask<int> SelectRecordListCountAsync<TRecord>()
        {
            var dbContext = this.DBContext.CreateDbContext();
            var count = await dbContext
                .GetDbSet<TRecord>()
                .CountAsync();

            dbContext?.Dispose();
            return count;
        }

        public override async ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Modified;
            var result =  await this.UpdateContext(context);
            context?.Dispose();
            return result;
        }

        public override async ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.GetDbSet<TRecord>().Add(record);
            var result = await this.UpdateContext(context);
            context?.Dispose();
            return result;
        }

        public override async ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Deleted;
            var result =  await this.UpdateContext(context);
            context?.Dispose();
            return result;
        }

        /// Helper method to update the context and return a DBTaskResult
        protected async Task<DbTaskResult> UpdateContext(DbContext context)
            => await context.SaveChangesAsync() > 0 ? DbTaskResult.OK() : DbTaskResult.NotOK();
    }
}
