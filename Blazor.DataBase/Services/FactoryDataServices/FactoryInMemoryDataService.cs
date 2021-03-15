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

namespace Blazor.Database.Services
{
    public class FactoryInMemoryDataService<TDbContext> :
        FactoryDataService,
        IFactoryDataService
        where TDbContext : DbContext
    {

        protected virtual TDbContext DBContext { get; set; } = null;


        public FactoryInMemoryDataService(IConfiguration configuration, IDbContextFactory<TDbContext> dbContext) : base(configuration)
        {
            this.DBContext = dbContext.CreateDbContext();
            // Debug.WriteLine($"==> New Instance {this.ToString()} ID:{this.ServiceID.ToString()} ");
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<List<TRecord>> GetRecordListAsync<TRecord>()
        {
            var dbset = this.DBContext.GetDbSet<TRecord>();
            return await dbset.ToListAsync() ?? new List<TRecord>();
        }


        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<TRecord> GetRecordAsync<TRecord>(int id)
        {
            var dbset = this.DBContext.GetDbSet<TRecord>();
            return await dbset.FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;

        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<int> GetRecordListCountAsync<TRecord>()
        {
            var dbset = this.DBContext.GetDbSet<TRecord>();
            return await dbset.CountAsync();
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override Task<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
        {
            this.DBContext.Entry(record).State = EntityState.Modified;
            var x = this.DBContext.SaveChanges();
            return Task.FromResult( new DbTaskResult() { IsOK = true, Type = MessageType.Success });
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override Task<DbTaskResult> CreateRecordAsync<TRecord>(TRecord record)
        {
            var dbset = this.DBContext.GetDbSet<TRecord>();
            dbset.Add(record);
            var x = this.DBContext.SaveChanges();
            return Task.FromResult(new DbTaskResult() { IsOK = true, Type = MessageType.Success, NewID = record.ID });
        }

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Task<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
        {
            this.DBContext.Entry(record).State = EntityState.Deleted;
            var x = this.DBContext.SaveChanges();
            return Task.FromResult(new DbTaskResult() { IsOK = true, Type = MessageType.Success });
        }
    }
}
