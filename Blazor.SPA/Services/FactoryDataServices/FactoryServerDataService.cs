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

namespace Blazor.SPA.Services
{
    public class FactoryServerDataService<TDbContext> :
        FactoryDataService
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
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>(int page, int pagesize)
        {
            var startpage = page <= 1
                ? 0
                : (page - 1) * pagesize;
            var context = this.DBContext.CreateDbContext();
            var dbset = this.DBContext
                .CreateDbContext()
                .GetDbSet<TRecord>();
            return await dbset.Skip(startpage).Take(pagesize).ToListAsync() ?? new List<TRecord>();
        }

        /// <summary>
        /// Method to get the Record List
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>(int page, int pagesize, Sortor sorter, Filtor filter)
        {
            var startpage = page <= 1
                ? 0
                : (page - 1) * pagesize;
            var context = this.DBContext.CreateDbContext();
            var dbset = this.DBContext
                .CreateDbContext()
                .GetDbSet<TRecord>();
            if (sorter.Descending)
            {
                var list = await dbset
                    .OrderByDescending(item => item.GetType().GetProperty(sorter.SortColumn).GetValue(item, null))
                    .Skip(startpage)
                    .Take(pagesize).ToListAsync() ?? new List<TRecord>();
                return list;
            }
            else
            {
                var list = await dbset
                    .OrderBy(item => item.GetType().GetProperty(sorter.SortColumn).GetValue(item, null))
                    .Skip(startpage)
                    .Take(pagesize).ToListAsync() ?? new List<TRecord>();
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
