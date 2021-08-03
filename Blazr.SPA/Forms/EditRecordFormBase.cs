/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Components;
using Blazr.SPA.Data;
using Blazr.SPA.Data.Events;
using Blazr.SPA.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazr.SPA.Forms
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

        public virtual Guid FormId { get; } = Guid.Empty;

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
            this.TryGetModalID();
            var id = this._Id;
            if (this.EditStateService.IsDirty)
                id = (Guid)this.EditStateService.RecordID;

            id = id != Guid.Empty ? id : this.ID;
            await this.Service.GetRecordAsync(id);
            // assign the Model to the EditContext
            this.Model = new TEditRecord();
            this.Model.Populate(this.Service.Record);
            this.EditContext = new EditContext(this.Model);
            this.EditStateService.EditFormUrl = FormUrl ?? NavManager.Uri;
            this.EditStateService.RecordID = id;
            _isLoaded = true;
            this.EditContext.OnFieldChanged += FieldChanged;
            this.EditStateService.EditStateChanged += OnEditStateChanged;
            if (!this._isNew)
                this.EditContext.Validate();
        }

        protected void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            this._confirmDelete = false;
        }

        private void OnEditStateChanged(object sender, EditStateEventArgs e)
        {
            if (this.IsModal) this.Modal.Lock(e.IsDirty);
            this.InvokeAsync(StateHasChanged);
        }

        protected void ValidStateChanged(bool valid)
            => this._isValid = valid;

        protected async Task HandleValidSubmit()
        {
            var rec = this.Model.GetRecord();
            await this.Service.SaveRecordAsync(rec);
            this.EditStateService.NotifyRecordSaved();
            await this.InvokeAsync(this.StateHasChanged);
        }

        protected void ResetToRecord()
        {
            this.Model.Populate(this.Service.Record);
        }

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
