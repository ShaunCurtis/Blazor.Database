/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System;
using Blazor.Database.Extensions;
using System.Linq;

namespace Blazor.Database.Services
{
    public class FactoryServerDataService<TDbContext> :
        FactoryDataService,
        IFactoryDataService
        where TDbContext : DbContext
    {

        protected virtual IDbContextFactory<TDbContext> DBContext { get; set; } = null;


        public FactoryServerDataService(IConfiguration configuration, IDbContextFactory<TDbContext> dbContext) : base(configuration)
        {
            this.DBContext = dbContext;
            // Debug.WriteLine($"==> New Instance {this.ToString()} ID:{this.ServiceID.ToString()} ");
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>()
        {
            var context = this.DBContext.CreateDbContext();
            var dbset = context.GetDbSet<TRecord>();
            return await dbset.ToListAsync() ?? new List<TRecord>();
        }

        /// <summary>
        /// Method to get the Record List
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>(int page, int pagesize)
        {
            var startpage = 0;
            if (page <=  1) 
                startpage = 0 ;
            else
                startpage = (page - 1) * pagesize;
            var context = this.DBContext.CreateDbContext();
            var dbset = context.GetDbSet<TRecord>();
            //var dbset = this.GetDbSet<TRecord>();
            return await dbset.Skip(startpage).Take(pagesize).ToListAsync() ?? new List<TRecord>();
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<TRecord> GetRecordAsync<TRecord>(int id)
        {
            var dbset = this.DBContext.CreateDbContext().GetDbSet<TRecord>();
            return await dbset.FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;

        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<int> GetRecordListCountAsync<TRecord>()
        {
            var dbset = this.DBContext.CreateDbContext().GetDbSet<TRecord>();
            return await dbset.CountAsync();
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            context.Entry(record).State = EntityState.Modified;
            var x = await context.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success };
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> CreateRecordAsync<TRecord>(TRecord record)
        {
            var context = this.DBContext.CreateDbContext();
            var dbset = context.GetDbSet<TRecord>();
            dbset.Add(record);
            var x = await context.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success, NewID = record.ID };
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
            var x = await context.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success };
        }
    }
}
