/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Components;
using Blazor.SPA.Data;
using Blazor.SPA.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazor.SPA.Components
{
    public class RecordFormBase<TRecord> : 
        ComponentBase 
        where TRecord : class, IDbRecord<TRecord>, new()

    {
        [CascadingParameter] public IModalDialog Modal { get; set; }

        [Parameter]
        public int ID
        {
            get => this._Id;
            set => this._Id = value;
        }

        [Parameter] public EventCallback ExitAction { get; set; }

        [Inject] protected NavigationManager NavManager { get; set; }

        protected IFactoryControllerService<TRecord> Service { get; set; }

        protected virtual bool IsLoaded => this.Service != null && this.Service.Record != null;

        protected virtual bool HasServices => this.Service != null;
        protected bool _isModal => this.Modal != null;

        protected int _Id = -1;

        protected async override Task OnInitializedAsync()
        {
            await LoadRecordAsync();
        }

        protected virtual async Task LoadRecordAsync()
        {
            this.TryGetModalID();
            await this.Service.GetRecordAsync(this._Id);
            await base.OnInitializedAsync();
        }

        protected virtual bool TryGetModalID()
        {
            if (this._isModal && this.Modal.Options.TryGet<int>("Id", out int value))
            {
                this._Id = value;
                return true;
            }
            return false;
        }

        protected virtual void Exit()
        {
            if (this._isModal)
                this.Modal.Close(ModalResult.OK());
            else if (ExitAction.HasDelegate)
                ExitAction.InvokeAsync();
            else
                this.NavManager.NavigateTo("/");
        }
    }
}
