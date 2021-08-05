/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System;

namespace Blazr.SPA.Core
{
    /// <summary>
    /// Common Interface definition for any DbRecord
    /// </summary>
    public interface IDbRecord<TRecord>
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        public Guid ID { get; }

        public string DisplayName { get; }

        public string GetDbSetName()
            => new TRecord().GetType().Name;

        public TRecord Copy()
        {
            var rec = new TRecord();
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.CanWrite)
                {
                    var value = prop.GetValue(this);
                    prop.SetValue(rec, value);
                }
            }
            return rec;
        }

    }
}
