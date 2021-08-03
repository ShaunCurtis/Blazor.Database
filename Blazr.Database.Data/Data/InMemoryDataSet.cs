/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.EditForms.Web.Data
{
    public abstract class InMemoryDataSet<TRecord> : IEnumerable<TRecord>
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        protected List<TRecord> records = new List<TRecord>();

        private List<TRecord> enumrecords
        {
            get
            {
                var list = new List<TRecord>();
                records.ForEach(item => list.Add((TRecord)item.Copy()));
                list.OrderBy(item => item.ID);
                return list;
            }
        }

        public TRecord Get(Guid id)
            => enumrecords.FirstOrDefault(item => item.ID == id);

        public bool Update(TRecord record)
        {
            if (record != null)
            {
                var rec = records.FirstOrDefault(item => item.ID == record.ID);
                if (rec != null)
                    records.Remove(rec);
                records.Add(record);
            }
            return record != null;
        }

        public bool Insert(TRecord record)
        {
            if (record != null)
                records.Add(record);
            return record != null;
        }

        public bool Delete(TRecord record)
        {
            if (record != null)
            {
                var rec = records.FirstOrDefault(item => item.ID == record.ID);
                if (rec != null)
                    records.Remove(rec);
            }
            return record != null;
        }

        public InMemoryDataSet()
        {
            LoadData();
        }

        public abstract void LoadData();

        public IEnumerator GetEnumerator()
        {
            foreach (TRecord record in enumrecords)
                yield return record;
        }

        IEnumerator<TRecord> IEnumerable<TRecord>.GetEnumerator()
            => ((IEnumerable<TRecord>)enumrecords).GetEnumerator();
    }
}
