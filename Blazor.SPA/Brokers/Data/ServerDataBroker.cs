/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Blazor.SPA.Extensions;
using System.Linq;
using System.Linq.Dynamic.Core;

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
            => await this.DBContext
            .CreateDbContext()
            .GetDbSet<TRecord>()
            .ToListAsync() ?? new List<TRecord>();

        public override async ValueTask<List<TRecord>> SelectPagedRecordsAsync<TRecord>(PaginatorData paginatorData)
        {
            var startpage = paginatorData.Page <= 1
                ? 0
                : (paginatorData.Page - 1) * paginatorData.PageSize;
            var context = this.DBContext.CreateDbContext();
            var dbset = this.DBContext
                .CreateDbContext()
                .GetDbSet<TRecord>();
            var x = typeof(TRecord).GetProperty(paginatorData.SortColumn);
            var isSortable = typeof(TRecord).GetProperty(paginatorData.SortColumn) != null;
            if (isSortable)
            {
                var list = await dbset
                    .OrderBy(paginatorData.SortDescending ? $"{paginatorData.SortColumn} descending" : paginatorData.SortColumn)
                    .Skip(startpage)
                    .Take(paginatorData.PageSize).ToListAsync() ?? new List<TRecord>();
                return list;
            }
            else
            {
                var list = await dbset
                    .Skip(startpage)
                    .Take(paginatorData.PageSize).ToListAsync() ?? new List<TRecord>();
                return list;
            }
        }

        public override async ValueTask<TRecord> SelectRecordAsync<TRecord>(int id)
            => await this.DBContext.
                CreateDbContext().
                GetDbSet<TRecord>().
                FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;

        public override async ValueTask<int> SelectRecordListCountAsync<TRecord>()
            => await this.DBContext.CreateDbContext().GetDbSet<TRecord>().CountAsync();

        public override async ValueTask<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Modified;
            return await this.UpdateContext(context);
        }

        public override async ValueTask<DbTaskResult> InsertRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.GetDbSet<TRecord>().Add(record);
            return await this.UpdateContext(context);
        }

        public override async ValueTask<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Deleted;
            return await this.UpdateContext(context);
        }

        /// Helper method to update the context and return a DBTaskResult
        protected async Task<DbTaskResult> UpdateContext(DbContext context)
            => await context.SaveChangesAsync() > 0 ? DbTaskResult.OK() : DbTaskResult.NotOK();
    }
}
