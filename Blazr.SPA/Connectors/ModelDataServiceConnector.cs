/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Brokers;
using Blazr.SPA.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazr.SPA.Connectors
{
    public abstract class ModelDataServiceConnector :
        IDataServiceConnector

    {
        private readonly IDataBroker dataBroker;
        private readonly ILoggingBroker loggingBroker;

        public ModelDataServiceConnector(IDataBroker dataBroker, ILoggingBroker loggingBroker)
        {
            this.dataBroker = dataBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<DbTaskResult> AddRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.InsertRecordAsync<TModel>(model);

        public async ValueTask<List<TModel>> GetAllRecordsAsync<TModel>() where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.SelectAllRecordsAsync<TModel>();

        public async ValueTask<List<TModel>> GetPagedRecordsAsync<TModel>(RecordPagingData pagingData) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.SelectPagedRecordsAsync<TModel>(pagingData);

        public async ValueTask<TModel> GetRecordByIdAsync<TModel>(Guid modelId) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.SelectRecordAsync<TModel>(modelId);

        public async ValueTask<DbTaskResult> ModifyRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.UpdateRecordAsync<TModel>(model);

        public async ValueTask<DbTaskResult> RemoveRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.DeleteRecordAsync<TModel>(model);

        public async ValueTask<int> GetRecordCountAsync<TModel>() where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.SelectRecordListCountAsync<TModel>();
    }
}
