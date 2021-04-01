/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Components;
using Blazor.SPA.Data;
using Blazor.SPA.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazor.SPA.Components
{
    /// <summary>
    /// Base form for Displaying a record
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public class RecordFormBase<TRecord> :
        ComponentBase
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        /// <summary>
        /// Cascaded Modal Dialog. If there is one the form is hosted by a modal dialog.
        /// </summary>
        [CascadingParameter] public IModalDialog Modal { get; set; }

        /// <summary>
        /// The ID passed to the form when it's declared as a component
        /// </summary>
        [Parameter]
        public int ID { get; set; } = 0;

        /// <summary>
        /// Callback to the host component to tell it the user has exited this form
        /// </summary>
        [Parameter] public EventCallback ExitAction { get; set; }

        /// <summary>
        /// Injected Navigation Manager - used in the exit fallback
        /// </summary>
        [Inject] protected NavigationManager NavManager { get; set; }

        /// <summary>
        /// The Controller Service
        /// </summary>
        protected IFactoryControllerService<TRecord> Service { get; set; }

        /// <summary>
        /// Boolean to check we have a service with a valid record
        /// </summary>
        protected virtual bool IsLoaded => this.Service != null && this.Service.Record != null;

        /// <summary>
        /// Boolean to check we have a service
        /// </summary>
        protected virtual bool HasServices => this.Service != null;

        /// <summary>
        /// Boolena to check if we are in a Modal Dialog context
        /// </summary>
        protected bool _isModal => this.Modal != null;

        /// <summary>
        /// Modal Id value
        /// </summary>
        protected int _modalId { get; set; } = 0;

        /// <summary>
        /// Internal Id value
        /// </summary>
        protected int _Id => _modalId != 0 ? _modalId : this.ID;

        /// <summary>
        /// Inherited
        /// </summary>
        /// <returns></returns>
        protected async override Task OnInitializedAsync()
        {
            // Get the record
            await LoadRecordAsync();
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Method to get the record to display
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadRecordAsync()
        {
            // If we're in a modal context we nee to get the id from the Modal Options object
            this.TryGetModalID();
            // Get the record
            await this.Service.GetRecordAsync(this._Id);
        }

        /// <summary>
        /// Method to try to get the Id From Modal Options
        /// </summary>
        /// <returns></returns>
        protected virtual bool TryGetModalID()
        {
            if (this._isModal && this.Modal.Options.TryGet<int>("Id", out int value))
            {
                this._modalId = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to exit the form
        /// </summary>
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
