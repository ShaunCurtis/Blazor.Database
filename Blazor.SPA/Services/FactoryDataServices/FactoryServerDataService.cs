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

namespace Blazor.SPA.Services
{
    public class FactoryServerDataService<TDbContext> :
        FactoryDataService,
        IFactoryDataService
        where TDbContext : DbContext
    {

        protected virtual IDbContextFactory<TDbContext> DBContext { get; set; } = null;

        public FactoryServerDataService(IConfiguration configuration, IDbContextFactory<TDbContext> dbContext) : base(configuration)
            => this.DBContext = dbContext;

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>()
            => await this.DBContext
            .CreateDbContext()
            .GetDbSet<TRecord>()
            .ToListAsync() ?? new List<TRecord>();

        /// <summary>
        /// Method to get the Record List
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>(PaginatorData paginatorData)
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

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<TRecord> GetRecordAsync<TRecord>(int id)
            => await this.DBContext.
                CreateDbContext().
                GetDbSet<TRecord>().
                FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<int> GetRecordListCountAsync<TRecord>()
            => await this.DBContext.CreateDbContext().GetDbSet<TRecord>().CountAsync();

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Modified;
            return await this.UpdateContext(context);
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> CreateRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.GetDbSet<TRecord>().Add(record);
            return await this.UpdateContext(context);
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Deleted;
            return await this.UpdateContext(context);
        }

        /// <summary>
        /// Helper method to update the context and return a DBTaskResult
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected async Task<DbTaskResult> UpdateContext(DbContext context)
            => await context.SaveChangesAsync() > 0 ? DbTaskResult.OK() : DbTaskResult.NotOK();
    }
}
