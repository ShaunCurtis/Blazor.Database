/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System;

namespace Blazor.SPA.Data
{
    /// <summary>
    /// Common Interface definition for any DbRecord
    /// </summary>
    public interface IDbRecord<TRecord> 
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        /// <summary>
        /// ID to ensure we have a unique key
        /// </summary>
        public Guid ID { get; }

        /// <summary>
        /// ID to ensure we have a unique key
        /// </summary>
        public Guid GUID { get; }

        /// <summary>
        /// Display name for the Record
        /// Point to the field that you want to use for the dipslay name
        /// </summary>
        public string DisplayName { get; }

    }
}
