/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System;

namespace Blazr.SPA.Data
{
    /// <summary>
    /// Common Interface definition for any EditRecord
    /// </summary>
    public interface IEditRecord<TRecord>
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        public Guid ID { get; }

        public void Populate(IDbRecord<TRecord> dbRecord);

        public TRecord GetRecord();
    }
}
