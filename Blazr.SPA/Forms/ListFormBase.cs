/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazr.SPA.Components
{
    /// <summary>
    /// Abstract class to implement the boilerplate code used in list forms
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class ListFormBase<TRecord> : ComponentBase, IDisposable
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        [Parameter] public EventCallback<Guid> EditRecord { get; set; }

        [Parameter] public EventCallback<Guid> ViewRecord { get; set; }

        [Parameter] public EventCallback<Guid> NewRecord { get; set; }

        [Parameter] public EventCallback ExitAction { get; set; }

        [Inject] protected NavigationManager NavManager { get; set; }

        [Inject] protected EditStateService EditStateService { get; set; }

        protected IModelViewService<TRecord> Service { get; set; }

        protected bool IsLoaded => this.Service?.HasRecords ?? false;

        protected ComponentState LoadState => IsLoaded ? ComponentState.Loaded : ComponentState.Loading;

        protected bool HasService => this.Service != null;

        protected override async Task OnInitializedAsync()
        {
            if (HasService)
            {
                await this.Service.GetRecordsAsync();
                this.Service.ListHasChanged += OnListChanged;
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (this.EditStateService.IsDirty)
                this.Edit((Guid)this.EditStateService.RecordID);
        }

        protected void OnListChanged(object sender, EventArgs e)
            => this.InvokeAsync(this.StateHasChanged);

        protected virtual void Edit(Guid id)
            => this.EditRecord.InvokeAsync(id);

        protected virtual void View(Guid id)
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