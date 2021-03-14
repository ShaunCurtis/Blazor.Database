/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Database.Extensions
{
    /// <summary>
    /// Class defining extension methods for <see cref="DbContext"/>
    ///  1. Methods use the context to access the database for running stored Procedures
    ///  2. Generic Methods to access to the DbSets
    ///  Many of the Methods rely on:
    ///  1. Correct naming conventions
    ///  2. A <see cref="DbRecordInfo"/> object defined for the Record/Model
    ///  3. Correct <see cref="SPParameterAttribute"/> Attrubute labelling of Record/Model Properties 
    /// </summary>
    public static class DbContextExtensions
    {

        /// <summary>
        /// Generic Method to get a record List from a DbSet
        /// </summary>
        /// <typeparam name="TRecord">Record/Model Type</typeparam>
        /// <param name="context">DBContext</param>
        /// <param name="dbSetName">DbSet Name</param>
        /// <returns></returns>
        public async static Task<List<TRecord>> GetRecordListAsync<TRecord>(this DbContext context, string dbSetName = null) where TRecord : class, IDbRecord<TRecord>, new()
        {
            var dbset = GetDbSet<TRecord>(context, dbSetName);
            return await dbset.ToListAsync() ?? new List<TRecord>();
        }

        /// <summary>
        /// Generic Method to get a Distinct List of a specific field from a Database View 
        /// You must have a DbSet in your DBContext called dbSetName of type <see cref="DbDistinct"/>
        /// </summary>
        /// <typeparam name="TRecord"> Record Type</typeparam>
        /// <param name="context">DBContext</param>
        /// <param name="dbSetName">DbSet Name</param>
        /// <returns></returns>
        public async static Task<List<string>> GetDistinctListAsync<TRecord>(this DbContext context, string fieldName) where TRecord : class, IDbRecord<TRecord>, new()
        {
            // Get the DbSet
            var dbset = context.GetDbSet<TRecord>();
            //var dbset = GetDbSet<TRecord>(context, null);
            // Get an empty list
            var list = new List<string>();
            // Get the filter propertyinfo object
            var x = typeof(TRecord).GetProperty(fieldName);
            if (dbset != null && x != null)
            {
                // we get the full list and then run a distinct because we can't run a distinct directly on the dbSet
                var fulllist = await dbset.Select(item => x.GetValue(item).ToString()).ToListAsync();
                list = fulllist.Distinct().ToList();
                // old way using SQL
                //list = await dbset.FromSqlRaw($"SELECT * FROM vw_{ propertyInfo.Name} WHERE {filter.Key} = {filter.Value}").ToListAsync();
            }
            return list ?? new List<string>();
        }

        /// <summary>
        /// Generic Method to get a record List count from a DbSet
        /// </summary>
        /// <typeparam name="TRecord">Record Type</typeparam>
        /// <param name="context">DbContext</param>
        /// <param name="dbSetName">DbSet Name</param>
        /// <returns></returns>
        public async static Task<int> GetRecordListCountAsync<TRecord>(this DbContext context, string dbSetName = null) where TRecord : class, IDbRecord<TRecord>, new()
        {
            var dbset = GetDbSet<TRecord>(context, dbSetName);
            return await dbset.CountAsync();
        }

        /// <summary>
        /// Generic Method to get a record from a DbSet
        /// </summary>
        /// <typeparam name="TRecord">Record Type</typeparam>
        /// <param name="context">DbContext</param>
        /// <param name="id">record ID to fetch</param>
        /// <param name="dbSetName">DbSet Name</param>
        /// <returns></returns>
        public async static Task<TRecord> GetRecordAsync<TRecord>(this DbContext context, int id, string dbSetName = null) where TRecord : class, IDbRecord<TRecord>, new()
        {
            var dbset = context.GetDbSet<TRecord>();
            //var dbset = GetDbSet<TRecord>(context, dbSetName);
            return await dbset.FirstOrDefaultAsync(item => ((IDbRecord<TRecord>)item).ID == id) ?? default;
        }

        public async static Task<DbTaskResult> CreateRecordAsync<TRecord>(this DbContext context, TRecord record, string dbSetName = null) where TRecord : class, IDbRecord<TRecord>, new()
        {
            var dbset = GetDbSet<TRecord>(context, dbSetName);
            dbset.Add(record);
            var x = await context.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success, NewID = record.ID };
        }

        public async static Task<DbTaskResult> UpdateRecordAsync<TRecord>(this DbContext context, TRecord record, string dbSetName = null) where TRecord : class, IDbRecord<TRecord>, new()
        {
            context.Entry(record).State = EntityState.Modified;
            var x = await context.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success};
        }

        public async static Task<DbTaskResult> DeleteRecordAsync<TRecord>(this DbContext context, TRecord record, string dbSetName = null) where TRecord : class, IDbRecord<TRecord>, new()
        {
            context.Entry(record).State = EntityState.Deleted;
            var x = await context.SaveChangesAsync();
            return new DbTaskResult() { IsOK = true, Type = MessageType.Success };
        }

        /// <summary>
        /// Method to get the DBSet from TRecord or a Name
        /// </summary>
        /// <typeparam name="TRecord">Record Type</typeparam>
        /// <param name="context">DbContext</param>
        /// <param name="dbSetName">DbSet Name</param>
        /// <returns></returns>
        private static DbSet<TRecord> GetDbSet<TRecord>(this DbContext context, string dbSetName = null) where TRecord : class, IDbRecord<TRecord>, new()
        {
            var recname = new TRecord().GetType().Name;
            // Get the property info object for the DbSet 
            var pinfo = context.GetType().GetProperty(dbSetName ?? recname);
            // Get the property DbSet
            var dbSet =  (DbSet<TRecord>)pinfo.GetValue(context);
            Debug.Assert(dbSet != null);
            if (dbSet is null) throw new InvalidOperationException($"{recname} does not have a matching DBset ");
            return dbSet;
        }
    }
}
