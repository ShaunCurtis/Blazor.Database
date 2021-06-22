/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Components;
using Blazor.SPA.Data;
using Blazor.SPA.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.SPA.Forms
{
    /// <summary>
    /// Abstract class to implement the boilerplate code used to Display a record
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class RecordFormBase<TRecord> :
        ComponentBase
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        [CascadingParameter] public IModalDialog Modal { get; set; }

        [Parameter]
        public Guid ID { get; set; } = Guid.Empty;

        [Parameter] public EventCallback ExitAction { get; set; }

        [Inject] protected NavigationManager NavManager { get; set; }

        protected IModelViewService<TRecord> Service { get; set; }

        protected virtual bool IsLoaded => this.Service != null && this.Service.Record != null;

        protected virtual ComponentState LoadState => IsLoaded ? ComponentState.Loaded : ComponentState.Loading;

        protected virtual bool HasServices => this.Service != null;

        protected bool _isModal => this.Modal != null;

        protected Guid _modalId { get; set; } = Guid.Empty;

        protected Guid _Id => TryGetModalID() ? _modalId : this.ID;

        protected async override Task OnInitializedAsync()
        {
            // Get the record
            await LoadRecordAsync();
            await base.OnInitializedAsync();
        }

        protected virtual async Task LoadRecordAsync()
        {
            // Get the record
            await this.Service.GetRecordAsync(this._Id);
        }

        protected virtual bool TryGetModalID()
        {
            if (this._isModal && this.Modal.Options.TryGet<Guid>("Id", out Guid value))
            {
                this._modalId = value;
                return true;
            }
            return false;
        }

        protected virtual async Task Exit()
        {
            // If we're in a modal context, call Close on the cascaded Modal object
            if (this._isModal)
                this.Modal.Close(ModalResult.OK());
            // If there's a delegate registered on the ExitAction, execute it. 
            else if (ExitAction.HasDelegate)
                await ExitAction.InvokeAsync();
            // else fallback action is to navigate to root
            else
                this.NavManager.NavigateTo("/");
        }
    }
}
