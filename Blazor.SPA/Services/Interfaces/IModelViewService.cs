/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.SPA.Services
{
    public interface IModelViewService<TRecord>
        where TRecord : class, new()
    {
        public Guid Id { get; }

        public TRecord Record { get; }

        public List<TRecord> Records { get; }

        public int RecordCount { get; }

        public DbTaskResult DbResult { get; }

        public Paginator Paginator { get; }

        public bool IsRecord { get; }

        public bool HasRecords { get; }

        public bool IsNewRecord { get; }

        public event EventHandler RecordHasChanged;

        public event EventHandler ListHasChanged;

        public ValueTask ResetServiceAsync();

        public ValueTask ResetRecordAsync();

        public ValueTask ResetListAsync();

        public ValueTask GetRecordsAsync();

        public ValueTask<bool> SaveRecordAsync();

        public ValueTask<bool> GetRecordAsync(int id);

        public ValueTask<bool> NewRecordAsync();

        public ValueTask<bool> DeleteRecordAsync();
    }
}
