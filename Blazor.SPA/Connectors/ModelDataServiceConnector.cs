/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Brokers;
using Blazor.SPA.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.SPA.Connectors
{
    public class ModelDataServiceConnector :
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

        public async ValueTask<List<TModel>> GetPagedRecordsAsync<TModel>(PaginatorData paginatorData) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.SelectPagedRecordsAsync<TModel>(paginatorData);

        public async ValueTask<TModel> GetRecordByIdAsync<TModel>(int modelId) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.SelectRecordAsync<TModel>(modelId);

        public async ValueTask<DbTaskResult> ModifyRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.UpdateRecordAsync<TModel>(model);

        public async ValueTask<DbTaskResult> RemoveRecordByIdAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.DeleteRecordAsync<TModel>(model);

        public async ValueTask<int> GetRecordCountAsync<TModel>() where TModel : class, IDbRecord<TModel>, new()
            => await this.dataBroker.SelectRecordListCountAsync<TModel>();
    }
}
