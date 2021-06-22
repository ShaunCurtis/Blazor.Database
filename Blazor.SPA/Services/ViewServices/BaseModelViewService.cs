/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Connectors;
using Blazor.SPA.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.SPA.Services
{
    public abstract class BaseModelViewService<TRecord> :
        IDisposable,
        IModelViewService<TRecord>
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        public Guid Id { get; } = Guid.NewGuid();

        public TRecord Record
        {
            get => _record;
            private set
            {
                this._record = value;
                this.RecordHasChanged?.Invoke(value, EventArgs.Empty);
            }
        }
        private TRecord _record = null;

        public List<TRecord> Records
        {
            get => _records;
            private set
            {
                this._records = value;
                this.ListHasChanged?.Invoke(value, EventArgs.Empty);
            }
        }
        private List<TRecord> _records = null;

        public DbTaskResult DbResult { get; set; } = new DbTaskResult();

        public Paginator Paginator { get; private set; }

        public bool IsRecord => this.Record != null;

        public bool HasRecords => this.Records != null && this.Records.Count > 0;

        public bool IsNewRecord { get; protected set; } = true;

        protected IDataServiceConnector DataServiceConnector { get; set; }

        public int RecordCount => throw new NotImplementedException();

        public event EventHandler RecordHasChanged;

        public event EventHandler ListHasChanged;

        public BaseModelViewService(IDataServiceConnector dataServiceConnector)
        {
            this.DataServiceConnector = dataServiceConnector;
            this.Paginator = new Paginator(10, 5);
            this.Paginator.PageChanged += this.OnPageChanged;
        }

        public async ValueTask ResetServiceAsync()
        {
            await this.ResetListAsync();
            await this.ResetRecordAsync();
        }

        public ValueTask ResetListAsync()
        {
            this.Records = null;
            return ValueTask.CompletedTask;
        }

        public ValueTask ResetRecordAsync()
        {
            this.Record = null;
            this.IsNewRecord = false;
            return ValueTask.CompletedTask;
        }

        public async ValueTask GetRecordsAsync()
        {
            this.Records = await DataServiceConnector.GetPagedRecordsAsync<TRecord>(this.Paginator.GetData);
            this.Paginator.RecordCount = await GetRecordListCountAsync();
            this.ListHasChanged?.Invoke(null, EventArgs.Empty);
        }

        public async ValueTask<bool> GetRecordAsync(Guid id)
        {
            if (!id.Equals(Guid.Empty))
            {
                this.IsNewRecord = false;
                this.Record = await DataServiceConnector.GetRecordByIdAsync<TRecord>(id);
            }
            else
            {
                this.Record = new TRecord();
                this.IsNewRecord = true;
            }
            return this.IsRecord;
        }

        public async ValueTask<int> GetRecordListCountAsync()
            => await DataServiceConnector.GetRecordCountAsync<TRecord>();

        public async ValueTask<bool> SaveRecordAsync(TRecord record)
        {
            if (this.IsNewRecord)
                this.DbResult = await DataServiceConnector.AddRecordAsync<TRecord>(record);
            else
                this.DbResult = await DataServiceConnector.ModifyRecordAsync(record);
            await this.GetRecordsAsync();
            this.IsNewRecord = false;
            return this.DbResult.IsOK;
        }

        public async ValueTask<bool> DeleteRecordAsync()
        {
            this.DbResult = await DataServiceConnector.RemoveRecordAsync<TRecord>(this.Record);
            return this.DbResult.IsOK;
        }

        protected async void OnPageChanged(object sender, EventArgs e)
            => await this.GetRecordsAsync();

        protected void NotifyRecordChanged(object sender, EventArgs e)
            => this.RecordHasChanged?.Invoke(sender, e);

        protected void NotifyListChanged(object sender, EventArgs e)
            => this.ListHasChanged?.Invoke(sender, e);

        public ValueTask<bool> NewRecordAsync()
        {
            this.Record = new TRecord();
            this.IsNewRecord = true;
            return ValueTask.FromResult(false);
        }

        public virtual void Dispose()
        {
        }
    }
}
