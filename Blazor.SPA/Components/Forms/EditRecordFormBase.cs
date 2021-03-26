/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Components;
using Blazor.SPA.Data;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Blazor.SPA.Components
{
    public abstract class EditRecordFormBase<TRecord> : 
        RecordFormBase<TRecord>,
        IDisposable
        where TRecord : class, IDbRecord<TRecord>, new()
    {
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

        protected bool _isNew => this.Service?.IsNewRecord ?? true;
        protected bool _isDirty = false;
        protected bool _isValid = true;
        protected bool _saveDisabled => !this.IsDirty || !this._isValid;
        protected bool _deleteDisabled => this._isNew || this._confirmDelete;
        protected bool _isLoaded = false;
        protected bool _dirtyExit = false;
        protected bool _confirmDelete = false;
        protected bool _isInlineDirty => (!this._isModal) && this._isDirty;
        protected string _saveButtonText => this._isNew ? "Save" : "Update";

        protected object Model => this.Service.Record;

        protected EditFormState EditFormState { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadRecordAsync();
        }

        protected override async Task LoadRecordAsync()
        {
            await base.OnInitializedAsync();
            this.EditContext = new EditContext(this.Model);
            _isLoaded = true;
            this.EditContext.OnFieldChanged += FieldChanged;
            if (!this._isNew)
                this.EditContext.Validate();
        }
        protected void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            this._dirtyExit = false;
            this._confirmDelete = false;
        }

        protected void EditStateChanged(bool dirty)
            => this.IsDirty = dirty;


        protected void ValidStateChanged(bool valid)
            => this._isValid = valid;

        protected async void HandleValidSubmit()
        {
            await this.Service.SaveRecordAsync();
            this.EditFormState.UpdateState();
            this._dirtyExit = false;
            await this.InvokeAsync(this.StateHasChanged);
        }

        protected void Exit()
        {
            if (this.IsDirty)
                this._dirtyExit = true;
            else
                ConfirmExit();
        }

        protected void Delete()
        {
            if (!this._isNew)
                this._confirmDelete = true;
        }

        protected async void ConfirmDelete()
        {
            if (this._confirmDelete)
            {
                await this.Service.DeleteRecordAsync();
                this.IsDirty = false;
                this.DoExit();
            }
        }

        protected void ConfirmExit()
        {
            this.IsDirty = false;
            this.DoExit();
        }

        public void Dispose()
        {
            this.EditContext.OnFieldChanged -= FieldChanged;
        }

        protected void DoExit(ModalResult result = null)
        {
            result = result ?? ModalResult.OK();
            if (this._isModal)
                this.Modal.Close(result);
            if (ExitAction.HasDelegate)
                ExitAction.InvokeAsync();
            else
                this.NavManager.NavigateTo("/");
        }

    }
}
