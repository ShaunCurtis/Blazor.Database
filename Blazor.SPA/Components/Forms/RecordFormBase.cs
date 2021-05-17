/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using Blazor.SPA.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazor.SPA.Components
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
        public int ID { get; set; } = 0;

        [Parameter] public EventCallback ExitAction { get; set; }

        [Inject] protected NavigationManager NavManager { get; set; }

        protected IModelViewService<TRecord> Service { get; set; }

        protected virtual bool IsLoaded => this.Service != null && this.Service.Record != null;

        protected virtual bool HasServices => this.Service != null;

        protected bool _isModal => this.Modal != null;

        protected int _modalId { get; set; } = 0;

        protected int _Id => _modalId != 0 ? _modalId : this.ID;

        protected async override Task OnInitializedAsync()
        {
            // Get the record
            await LoadRecordAsync();
            await base.OnInitializedAsync();
        }

        protected virtual async Task LoadRecordAsync()
        {
            // If we're in a modal context we nee to get the id from the Modal Options object
            this.TryGetModalID();
            // Get the record
            await this.Service.GetRecordAsync(this._Id);
        }

        protected virtual bool TryGetModalID()
        {
            if (this._isModal && this.Modal.Options.TryGet<int>("Id", out int value))
            {
                this._modalId = value;
                return true;
            }
            return false;
        }

        protected virtual void Exit()
        {
            // If we're in a modal context, call Close on the cascaded Modal object
            if (this._isModal)
                this.Modal.Close(ModalResult.OK());
            // If there's a delegate registered on the ExitAction, execute it. 
            else if (ExitAction.HasDelegate)
                ExitAction.InvokeAsync();
            // else fallback action is to navigate to root
            else
                this.NavManager.NavigateTo("/");
        }
    }
}
