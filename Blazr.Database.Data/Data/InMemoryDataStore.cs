using Blazor.EditForms.Web.Data;
using Blazr.Database.Core;
using Blazr.SPA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazr.Database.Data.Data
{
    class InMemoryDataStore
    {

        public WeatherDataSet WeatherForecast { get; set; }


        public InMemoryDataStore()
        {
            WeatherForecast = new WeatherDataSet();
        }

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
