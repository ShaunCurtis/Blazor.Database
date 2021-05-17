/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using Blazor.SPA.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.SPA.Components
{
    /// <summary>
    /// Abstract class to implement the boilerplate code used in list forms
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class ListFormBase<TRecord> : ComponentBase, IDisposable
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        [Parameter] public EventCallback<int> EditRecord { get; set; }

        [Parameter] public EventCallback<int> ViewRecord { get; set; }

        [Parameter] public EventCallback<int> NewRecord { get; set; }

        [Parameter] public EventCallback ExitAction { get; set; }

        protected IModelViewService<TRecord> Service { get; set; }

        [Inject] protected NavigationManager NavManager { get; set; }

        protected bool IsLoaded => this.Service?.HasRecords ?? false;

        protected bool HasService => this.Service != null;

        protected override async Task OnInitializedAsync()
        {
            if (HasService)
            {
                await this.Service.GetRecordsAsync();
                this.Service.ListHasChanged += OnListChanged;
            }
        }

        protected void OnListChanged(object sender, EventArgs e)
            => this.InvokeAsync(this.StateHasChanged);

        protected virtual void Edit(int id)
            => this.EditRecord.InvokeAsync(id);

        protected virtual void View(int id)
            => this.ViewRecord.InvokeAsync(id);

        protected virtual void New()
            => this.NewRecord.InvokeAsync();

        protected virtual void Exit()
        {
            if (ExitAction.HasDelegate)
                ExitAction.InvokeAsync();
            else
                this.NavManager.NavigateTo("/");
        }

        public void Dispose()
            => this.Service.ListHasChanged -= OnListChanged;
    }
}