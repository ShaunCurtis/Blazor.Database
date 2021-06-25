/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using Blazor.SPA.Data;

namespace Blazor.SPA.Extensions
{
    /// <summary>
    /// Class defining extension methods for <see cref="DbContext"/>
    /// </summary>
    public static class DbContextExtensions
    {
        public static DbSet<TRecord> GetDbSet<TRecord>(this DbContext context) where TRecord : class, IDbRecord<TRecord>, new()
        {
            var dbSetName = new TRecord().GetDbSetName();
            // Get the property info object for the DbSet 
            var pinfo = context.GetType().GetProperty(dbSetName);
            DbSet<TRecord> dbSet = null;
            Debug.Assert(pinfo != null);
            // Get the property DbSet
            try
            {
                dbSet = (DbSet<TRecord>)pinfo.GetValue(context);
            }
            catch
            {
                throw new InvalidOperationException($"{dbSetName} does not have a matching DBset ");
            }
            Debug.Assert(dbSet != null);
            return dbSet;
        }
    }
}
