/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.SPA.Connectors

{
    public interface IDataServiceConnector
    {
        ValueTask<DbTaskResult> AddRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new();

        ValueTask<TModel> GetRecordByIdAsync<TModel>(Guid ModelId) where TModel : class, IDbRecord<TModel>, new();

        ValueTask<DbTaskResult> ModifyRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new();

        ValueTask<DbTaskResult> RemoveRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new();

        ValueTask<int> GetRecordCountAsync<TModel>() where TModel : class, IDbRecord<TModel>, new();

        ValueTask<List<TModel>> GetAllRecordsAsync<TModel>() where TModel : class, IDbRecord<TModel>, new();

        ValueTask<List<TModel>> GetPagedRecordsAsync<TModel>(RecordPagingData paginatorData) where TModel : class, IDbRecord<TModel>, new();
    }
}