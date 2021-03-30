/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Components;
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
        /// <summary>
        /// Callback for Edit Action
        /// </summary>
        [Parameter] public EventCallback<int> EditRecord { get; set; }

        /// <summary>
        /// Callback for View Action
        /// </summary>
        [Parameter] public EventCallback<int> ViewRecord { get; set; }

        /// <summary>
        /// Callback for New Action
        /// </summary>
        [Parameter] public EventCallback<int> NewRecord { get; set; }

        /// <summary>
        /// Callback for Exit Action
        /// </summary>
        [Parameter] public EventCallback ExitAction { get; set; }

        /// <summary>
        /// Controller Data Service
        /// </summary>
        protected IFactoryControllerService<TRecord> Service { get; set; }

        /// <summary>
        /// Navigation Manager Service
        /// </summary>
        /// </summary>
        [Inject] protected NavigationManager NavManager { get; set; }

        /// <summary>
        /// Boolean indicating of we have records to display
        /// </summary>
        protected bool IsLoaded => this.Service?.HasRecords ?? false;

        /// <summary>
        /// Boolean indicating if we have a service
        /// </summary>
        protected bool HasService => this.Service != null;

        /// <summary>
        /// inherited Initialization
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            if (HasService)
            {
                await this.Service.GetRecordsAsync();
                this.Service.ListHasChanged += OnListChanged;
            }
        }

        /// <summary>
        /// Event handler to call StatehasChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnListChanged(object sender, EventArgs e)
            => this.InvokeAsync(this.StateHasChanged);

        /// <summary>
        /// Edit action to invoke the callback
        /// </summary>
        /// <param name="id"></param>
        protected virtual void Edit(int id)
            => this.EditRecord.InvokeAsync(id);

        /// <summary>
        /// View action to invoke the callback
        /// </summary>
        /// <param name="id"></param>
        protected virtual void View(int id)
            => this.ViewRecord.InvokeAsync(id);

        /// <summary>
        /// New action to invoke the callback
        /// </summary>
        protected virtual void New()
            => this.NewRecord.InvokeAsync();

        /// <summary>
        /// Exit action to invoke the callback
        /// fallback is to exit to root
        /// </summary>
        protected virtual void Exit()
        {
            if (ExitAction.HasDelegate)
                ExitAction.InvokeAsync();
            else
                this.NavManager.NavigateTo("/");
        }

        /// <summary>
        /// IDisosable Interface implementation
        /// </summary>
        public void Dispose()
            => this.Service.ListHasChanged -= OnListChanged;
    }
}