/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazor.Database.Extensions;
using Microsoft.Extensions.Configuration;

namespace Blazor.Database.Services
{
    public class FactoryServerDataService<TDbContext> :
        FactoryDataService<TDbContext>,
        IFactoryDataService<TDbContext>
        where TDbContext : DbContext
    {

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
            => await this.DBContext.CreateDbContext().GetRecordListAsync<TRecord>();


        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<TRecord> GetRecordAsync<TRecord>(int id)
            => await this.DBContext.CreateDbContext().GetRecordAsync<TRecord>(id);

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <returns></returns>
        public override async Task<int> GetRecordListCountAsync<TRecord>()
            => await this.DBContext.CreateDbContext().GetRecordListCountAsync<TRecord>();

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> UpdateRecordAsync<TRecord>(TRecord record)
            => await this.DBContext.CreateDbContext().UpdateRecordAsync<TRecord>(record);

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> CreateRecordAsync<TRecord>(TRecord record)
            => await this.DBContext.CreateDbContext().CreateRecordAsync<TRecord>(record);

        /// <summary>
        /// Inherited IDataService Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<DbTaskResult> DeleteRecordAsync<TRecord>(TRecord record)
            => await this.DBContext.CreateDbContext().DeleteRecordAsync<TRecord>(record);

    }
}
