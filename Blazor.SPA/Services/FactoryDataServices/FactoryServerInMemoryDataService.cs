/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System;
using Blazor.SPA.Extensions;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Blazor.SPA.Services
{
    public class FactoryServerInMemoryDataService<TDbContext> :
        FactoryDataService,
        IFactoryDataService
        where TDbContext : DbContext
    {

        protected virtual IDbContextFactory<TDbContext> DBContext { get; set; } = null;

        private DbContext _dbContext;

        public FactoryServerInMemoryDataService(IConfiguration configuration, IDbContextFactory<TDbContext> dbContext) : base(configuration)
        {
            this.DBContext = dbContext;
            _dbContext = this.DBContext.CreateDbContext();
            // Debug.WriteLine($"==> New Instance {this.ToString()} ID:{this.ServiceID.ToString()} ");
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>()
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            return await dbset.ToListAsync() ?? new List<TRecord>();
        }

        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>(Paginator paginator)
        {
            var startpage = paginator.Page <= 1
                ? 0
                : (paginator.Page - 1) * paginator.PageSize;
            var dbset = _dbContext.GetDbSet<TRecord>();
            var isSortable = typeof(TRecord).GetProperty(paginator.SortColumn) != null;
            if (isSortable)
            {
                var list = await dbset
                    .OrderBy(paginator.SortDescending ? $"{paginator.SortColumn} descending" : paginator.SortColumn)
                    .Skip(startpage)
                    .Take(paginator.PageSize).ToListAsync() ?? new List<TRecord>();
                return list;
            }
            else
            {
                var list = await dbset
                    .Skip(startpage)
                    .Take(paginator.PageSize).ToListAsync() ?? new List<TRecord>();
                return list;
            }
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<TRecord> GetRecordAsync<TRecord>(int id)
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            return await dbset.FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;

        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<int> GetRecordListCountAsync<TRecord>()
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            return await dbset.CountAsync();
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            _dbContext.Entry(record).State = EntityState.Modified;
            var x = await _dbContext.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success };
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> CreateRecordAsync<TRecord>(TRecord record)
        {
            var dbset = _dbContext.GetDbSet<TRecord>();
            dbset.Add(record);
            var x = await _dbContext.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success, NewID = record.ID };
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            _dbContext.Entry(record).State = EntityState.Deleted;
            var x = await _dbContext.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success };
        }
    }
}
