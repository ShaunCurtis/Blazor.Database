/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace Blazor.SPA.Extensions
{
    /// <summary>
    /// Class defining extension methods for <see cref="DbContext"/>
    /// </summary>
    public static class DbContextExtensions
    {

        public static DbSet<TRecord> GetDbSet<TRecord>(this DbContext context, string dbSetName = null) where TRecord : class, new()
        {
            var recname = new TRecord().GetType().Name;
            // Get the property info object for the DbSet 
            var pinfo = context.GetType().GetProperty(dbSetName ?? recname);
            DbSet<TRecord> dbSet = null;
            // Get the property DbSet
            try
            {
                dbSet = (DbSet<TRecord>)pinfo.GetValue(context);
            }
            catch
            {
                throw new InvalidOperationException($"{recname} does not have a matching DBset ");
            }
            Debug.Assert(dbSet != null);
            return dbSet;
        }
    }
}
