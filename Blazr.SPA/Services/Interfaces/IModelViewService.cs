﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazr.SPA.Core
{
    public interface IModelViewService<TRecord>
        where TRecord : class, new()
    {
        public Guid Id { get; }

        public TRecord Record { get; }

        public List<TRecord> Records { get; }

        public int RecordCount { get; }

        public DbTaskResult DbResult { get; }

        public RecordPager RecordPager { get; }

        public bool HasRecord { get; }

        public bool HasRecords { get; }

        public bool IsNewRecord { get; }

        public event EventHandler RecordHasChanged;

        public event EventHandler ListHasChanged;

        public void SetRecord(TRecord record);

        public ValueTask ResetServiceAsync();

        public ValueTask ResetRecordAsync();

        public ValueTask ResetListAsync();

        public ValueTask GetRecordsAsync();

        public ValueTask<bool> SaveRecordAsync(TRecord record);

        public ValueTask<bool> GetRecordAsync(Guid id);

        public ValueTask<bool> NewRecordAsync();

        public ValueTask<bool> DeleteRecordAsync();
    }
}
