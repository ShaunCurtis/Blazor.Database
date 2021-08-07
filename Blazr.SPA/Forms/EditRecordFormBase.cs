/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazr.SPA.Components
{
    /// <summary>
    /// Abstract Class defining all the boilerplate code used in an edit form
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class EditRecordFormBase<TRecord, TEditRecord> :
        RecordFormBase<TRecord>,
        IDisposable
        where TRecord : class, IDbRecord<TRecord>, new()
        where TEditRecord : class, IEditRecord<TRecord>, new()
    {
        [Inject] private IJSRuntime _js { get; set; }

        [Inject] protected EditStateService EditStateService { get; set; }

        protected EditContext EditContext { get; set; }
        protected bool IsDirty => this.EditStateService.IsDirty;

        protected virtual string FormUrl { get; set; }

        protected TEditRecord Model { get; set; }

        // Set of boolean properties/fields used in the razor code and methods to track 
        // state in the form or disable/show/hide buttons.
        protected bool _isNew => this.Service?.IsNewRecord ?? true;
        protected bool _isValid { get; set; } = true;
        protected bool _saveDisabled => !this.IsDirty || !this._isValid;
        protected bool _deleteDisabled => this._isNew || this._confirmDelete || this.IsDirty;
        protected bool _isLoaded { get; set; } = false;
        protected bool _dirtyExit => this.IsDirty;
        protected bool _confirmDelete { get; set; } = false;
        protected bool _isInlineDirty => (!this.IsModal) && this.IsDirty;
        protected string _saveButtonText => this._isNew ? "Save" : "Update";

        protected override ComponentState LoadState => _isLoaded ? ComponentState.Loaded : ComponentState.Loading;

        protected override async Task LoadRecordAsync()
        {
            _isLoaded = false;
            // Get the ID either from a ModalDialog Options or the Id Parameter and set the local id
            var id = this._Id;
            // Check if we have a dirty form reload and if so get the Id from the EditStateService
            if (this.EditStateService.IsDirty)
                id = (Guid)this.EditStateService.RecordID;
            // Get the record
            await this.Service.GetRecordAsync(id);
            // Get a new Edit class instance, populate it from the record and assign it to the EditContext
            this.Model = new TEditRecord();
            this.Model.Populate(this.Service.Record);
            this.EditContext = new EditContext(this.Model);
            // Set up the EditStateService FirmUrl and Record Id
            this.EditStateService.EditFormUrl = FormUrl ?? NavManager.Uri;
            this.EditStateService.RecordID = id;
            _isLoaded = true;
            // wire up the events
            this.EditContext.OnFieldChanged += FieldChanged;
            this.EditStateService.EditStateChanged += OnEditStateChanged;
            // if we have a dirty record or are editing an existing record, run validation
            if (!this._isNew)
                this.EditContext.Validate();
        }

        protected void FieldChanged(object sender, FieldChangedEventArgs e)
            =>  this._confirmDelete = false;

        private void OnEditStateChanged(object sender, EditStateEventArgs e)
        {
            if (this.IsModal) this.Modal.Lock(e.IsDirty);
            this.InvokeAsync(StateHasChanged);
        }

        protected void ValidStateChanged(bool valid)
            => this._isValid = valid;

        protected async Task HandleValidSubmit()
        {
            // Get the readonly record to submit
            var rec = this.Model.GetRecord();
            // Save the record
            await this.Service.SaveRecordAsync(rec);
            // Update the EditStateService to clean
            this.EditStateService.NotifyRecordSaved();
            // Render the component
            await this.InvokeAsync(this.StateHasChanged);
        }

        protected void ResetToRecord()
            => this.Model.Populate(this.Service.Record);

        protected void Delete()
        {
            if (!this._isNew)
                this._confirmDelete = true;
        }

        protected async Task ConfirmDelete()
        {
            if (this._confirmDelete)
            {
                await this.Service.DeleteRecordAsync();
                await this.DoExit();
            }
        }

        protected async Task ConfirmExit()
        {
            this.EditStateService.ResetEditState();
            this.SetPageExitCheck(false);
            await this.DoExit();
        }

        protected async Task DoExit(ModalResult result = null)
        {
            result ??= ModalResult.OK();
            if (this.IsModal)
                this.Modal.Close(result);
            if (ExitAction.HasDelegate)
                await ExitAction.InvokeAsync();
            else
                this.NavManager.NavigateTo("/");
        }

        private void SetPageExitCheck(bool action)
            => _js.InvokeAsync<bool>("cecblazor_setEditorExitCheck", action);

        public void Dispose()
        {
            if (this.EditContext != null)
                this.EditContext.OnFieldChanged -= FieldChanged;
        }
    }
}
