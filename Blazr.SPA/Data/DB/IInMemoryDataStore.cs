/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Core;
using System;
using System.Diagnostics;

namespace Blazr.SPA.Data
{
    public interface IInMemoryDataStore
    {

        public InMemoryDataSet<TRecord> GetDataSet<TRecord>() where TRecord : class, IDbRecord<TRecord>, new()
        {
            var dbSetName = new TRecord().GetDbSetName();
            // Get the property info object for the DbSet 
            var pinfo = this.GetType().GetProperty(dbSetName);
            InMemoryDataSet<TRecord> dbSet = null;
            Debug.Assert(pinfo != null);
            // Get the property DbSet
            try
            {
                dbSet = (InMemoryDataSet<TRecord>)pinfo.GetValue(this);
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
