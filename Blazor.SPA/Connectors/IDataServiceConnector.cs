/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.SPA.Connectors

{
    public interface IDataServiceConnector 
    {
        ValueTask<DbTaskResult> AddRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new();

        ValueTask<TModel> GetRecordByIdAsync<TModel>(int ModelId) where TModel : class, IDbRecord<TModel>, new();

        ValueTask<DbTaskResult> ModifyRecordAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new();

        ValueTask<DbTaskResult> RemoveRecordByIdAsync<TModel>(TModel model) where TModel : class, IDbRecord<TModel>, new();

        ValueTask<int> GetRecordCountAsync<TModel>() where TModel : class, IDbRecord<TModel>, new();

        ValueTask<List<TModel>> GetAllRecordsAsync<TModel>() where TModel : class, IDbRecord<TModel>, new();

        ValueTask<List<TModel>> GetPagedRecordsAsync<TModel>(PaginatorData paginatorData) where TModel : class, IDbRecord<TModel>, new();
    }
}