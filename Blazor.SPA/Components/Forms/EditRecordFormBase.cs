/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.SPA.Data;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Blazor.SPA.Components
{
    /// <summary>
    /// Abstract Class defining all the boilerplate code used in an edit form
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class EditRecordFormBase<TRecord> :
        RecordFormBase<TRecord>,
        IDisposable
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        /// <summary>
        /// Edit Context for the Editor - built from the service record
        /// </summary>
        protected EditContext EditContext { get; set; }

        /// <summary>
        /// Boolean Property tracking the Edit state of the form
        /// </summary>
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

        /// <summary>
        /// model used by the Edit Context
        /// </summary>
        protected TRecord Model => this.Service?.Record ?? null;

        /// <summary>
        /// Reference to the form EditContextState control
        /// </summary>
        protected EditFormState EditFormState { get; set; }

        // Set of boolean properties/fields used in the razor code and methods to track 
        // state in the form or disable/show/hide buttons.
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

        /// <summary>
        /// inherited
        /// </summary>
        /// <returns></returns>
        protected async override Task OnInitializedAsync()
            => await LoadRecordAsync();

        /// <summary>
        /// Method to load the record
        /// calls the base method to load the record and then sets up the EditContext
        /// </summary>
        /// <returns></returns>
        protected override async Task LoadRecordAsync()
        {
            await base.OnInitializedAsync();
            this.EditContext = new EditContext(this.Model);
            _isLoaded = true;
            this.EditContext.OnFieldChanged += FieldChanged;
            if (!this._isNew)
                this.EditContext.Validate();
        }

        /// <summary>
        /// Event handler for EditContext OnFieldChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            this._dirtyExit = false;
            this._confirmDelete = false;
        }

        /// <summary>
        /// Method to change edit state
        /// </summary>
        /// <param name="dirty"></param>
        protected void EditStateChanged(bool dirty)
            => this.IsDirty = dirty;


        /// <summary>
        /// Method to change the Validation state
        /// </summary>
        /// <param name="valid"></param>
        protected void ValidStateChanged(bool valid)
            => this._isValid = valid;

        /// <summary>
        /// Method to handle EditForm submission
        /// </summary>
        protected async void HandleValidSubmit()
        {
            await this.Service.SaveRecordAsync();
            this.EditFormState.UpdateState();
            this._dirtyExit = false;
            await this.InvokeAsync(this.StateHasChanged);
        }

        /// <summary>
        /// Handler for Delete action
        /// </summary>
        protected void Delete()
        {
            if (!this._isNew)
                this._confirmDelete = true;
        }

        /// <summary>
        /// Handler for Delete confirmation
        /// </summary>
        protected async void ConfirmDelete()
        {
            if (this._confirmDelete)
            {
                await this.Service.DeleteRecordAsync();
                this.IsDirty = false;
                this.DoExit();
            }
        }

        /// <summary>
        /// Handler for a confirmed exit - i.e. dirty exit
        /// </summary>
        protected void ConfirmExit()
        {
            this.IsDirty = false;
            this.DoExit();
        }

        /// <summary>
        /// Handler to Exit the form, dependant on it context
        /// </summary>
        /// <param name="result"></param>
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

        /// <summary>
        /// IDisposable Interface Implementation
        /// </summary>
        public void Dispose()
            => this.EditContext.OnFieldChanged -= FieldChanged;
    }
}
