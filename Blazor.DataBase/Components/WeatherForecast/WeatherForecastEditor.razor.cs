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

        [Parameter] public int ID { get; set; } = -1;

        [Parameter] public EventCallback ExitAction { get; set; }

        [Inject] private WeatherControllerService ControllerService { get; set; }

        private bool _isNew => this.ControllerService?.IsNewRecord ?? true;

        public EditContext EditContext { get; set; }

        private bool _isDirty = false;
        private bool _isValid = true;
        private bool _saveDisabled => !this._isDirty || !this._isValid;
        private bool _deleteDisabled => this._isNew || this._confirmDelete;
        private bool _isLoaded = false;
        private bool _dirtyExit = false;
        private bool _confirmDelete = false;
        private string _saveButtonText => this._isNew ? "Save" : "Update";

        public bool ResetEditState { get; set; }

        private WeatherForecast Model => this.ControllerService.Record;

        protected async override Task OnInitializedAsync()
        {
            await LoadRecordAsync();
        }

        private async Task LoadRecordAsync()
        {
            await this.ControllerService.GetRecordAsync(ID);
            this.EditContext = new EditContext(this.Model);
            await base.OnInitializedAsync();
            _isLoaded = true;
            this.EditContext.OnFieldChanged += FieldChanged;
            if (!this._isNew)
                this.EditContext.Validate();
        }

        private void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            this._dirtyExit = false;
            this._confirmDelete = false;
        }

        private void EditStateChanged(bool dirty)
        {
                this._isDirty = dirty;
        }


        private void ValidStateChanged(bool valid)
        {
            this._isValid = valid;
        }

        private async void HandleValidSubmit()
        {
            await this.ControllerService.SaveRecordAsync();
            if (this.ResetEditState) this.ResetEditState = false;
            this.ResetEditState = true;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private void Exit()
        {
            if (this._isDirty)
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
                await this.ExitAction.InvokeAsync();
            }
        }

        private void ConfirmExit()
        {
            if (this.ResetEditState) this.ResetEditState = false;
            this.ResetEditState = true;
            this.ExitAction.InvokeAsync();
        }

        public void Dispose()
        {
            this.EditContext.OnFieldChanged -= FieldChanged;
        }
    }
}
