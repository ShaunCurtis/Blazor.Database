using Blazor.Database.Data;
using Blazor.Database.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;
namespace Blazor.Database.Components
{
    public partial class WeatherForecastEditor : ComponentBase, IDisposable
    {

        [Parameter]
        public int ID
        {
            get => this._Id;
            set => this._Id = value;
        }

        [Parameter] public EventCallback ExitAction { get; set; }

        [CascadingParameter] IModalDialog Modal { get; set; }

        [Inject] private WeatherForecastControllerService ControllerService { get; set; }

        protected EditContext EditContext { get; set; }

        protected bool IsDirty
        {
            get => this._isDirty;
            set
            {
                if (value != this.IsDirty)
                {
                    this._isDirty = value;
                    if (this._isModal) this.Modal.Lock(value);
                }
            }
        }

        private bool _isNew => this.ControllerService?.IsNewRecord ?? true;
        private bool _isDirty = false;
        private bool _isValid = true;
        private bool _saveDisabled => !this.IsDirty || !this._isValid;
        private bool _deleteDisabled => this._isNew || this._confirmDelete;
        private bool _isLoaded = false;
        private bool _dirtyExit = false;
        private bool _confirmDelete = false;
        private bool _isModal => this.Modal != null;
        private bool _isInlineDirty => (!this._isModal) && this._isDirty;
        private string _saveButtonText => this._isNew ? "Save" : "Update";
        private int _Id = -1;

        private WeatherForecast Model => this.ControllerService.Record;

        private EditFormState EditFormState { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadRecordAsync();
        }

        private async Task LoadRecordAsync()
        {
            this.TryGetModalID();
            await this.ControllerService.GetRecordAsync(this._Id);
            this.EditContext = new EditContext(this.Model);
            await base.OnInitializedAsync();
            _isLoaded = true;
            this.EditContext.OnFieldChanged += FieldChanged;
            if (!this._isNew)
                this.EditContext.Validate();
        }

        private bool TryGetModalID()
        {
            if (this._isModal && this.Modal.Options.TryGet<int>("Id", out int value))
            {
                this._Id = value;
                return true;
            }
            return false;
        }

        private void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            this._dirtyExit = false;
            this._confirmDelete = false;
        }

        private void EditStateChanged(bool dirty)
            => this.IsDirty = dirty;


        private void ValidStateChanged(bool valid)
            => this._isValid = valid;

        private async void HandleValidSubmit()
        {
            await this.ControllerService.SaveRecordAsync();
            this.EditFormState.UpdateState();
            this._dirtyExit = false;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private void Exit()
        {
            if (this.IsDirty)
                this._dirtyExit = true;
            else
                ConfirmExit();
        }

        private void Delete()
        {
            if (!this._isNew)
                this._confirmDelete = true;
        }

        private async void ConfirmDelete()
        {
            if (this._confirmDelete)
            {
                await this.ControllerService.DeleteRecordAsync();
                this.IsDirty = false;
                this.DoExit();
            }
        }

        private void ConfirmExit()
        {
            this.IsDirty = false;
            this.DoExit();
        }

        public void Dispose()
        {
            this.EditContext.OnFieldChanged -= FieldChanged;
        }

        private void DoExit(ModalResult result = null)
        {
            result = result ?? ModalResult.OK();
            if (this._isModal) 
                this.Modal.Close(result);
            else 
                this.ExitAction.InvokeAsync();
        }
    }
}
