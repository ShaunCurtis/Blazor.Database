/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.NavigationLocker.Components;
using Blazr.SPA.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

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
        [CascadingParameter] private NavigationLock? navigationLock { get; set; }

        protected EditContext? EditContext { get; set; }

        protected bool IsDirty => this.EditStateService!.IsDirty;

        protected virtual string? FormUrl { get; set; }

        protected TEditRecord? Model { get; set; }

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
            // Get the record
            await this.Service.GetRecordAsync(id);
            // Get a new Edit class instance, populate it from the record and assign it to the EditContext
            this.Model = new TEditRecord();
            this.Model.Populate(this.Service.Record);
            this.EditContext = new EditContext(this.Model);
            _isLoaded = true;
            // wire up the events
            this.EditContext.OnFieldChanged += FieldChanged;
            // if we have a dirty record or are editing an existing record, run validation
            if (!this._isNew)
                this.EditContext.Validate();
        }

        protected void FieldChanged(object? sender, FieldChangedEventArgs e)
            => this._confirmDelete = false;

        private void OnEditStateChanged(object? sender, EditStateEventArgs e)
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

        protected RenderFragment ButtonContent => (builder) =>
        {
            {
                builder.OpenElement(0, "button");
                builder.AddAttribute(1, "type", "button");
                builder.AddAttribute(2, "Show", true);
                if (this._deleteDisabled)
                    builder.AddAttribute(3, "Disabled");
                builder.AddAttribute(4, "class", "btn mr-1 btn-outline-danger");
                builder.AddAttribute(5, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.Delete));
                builder.AddContent(6, "Delete");
                builder.CloseComponent();
            }
            {
                if (this._confirmDelete)
                {
                    builder.OpenElement(10, "button");
                    builder.AddAttribute(11, "type", "button");
                    builder.AddAttribute(14, "class", "btn mr-1 btn-danger");
                    builder.AddAttribute(15, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.ConfirmDelete));
                    builder.AddContent(16, "Confirm Delete");
                    builder.CloseComponent();
                }
            }
            {
                if (this.IsDirty)
                {
                    builder.OpenElement(20, "button");
                    builder.AddAttribute(21, "type", "button");
                    builder.AddAttribute(24, "class", "btn mr-1 btn-outline-warning");
                    builder.AddAttribute(25, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.ResetToRecord));
                    builder.AddContent(26, "Reset");
                    builder.CloseComponent();
                }
            }
            {
                builder.OpenElement(30, "button");
                builder.AddAttribute(31, "type", "submit");
                if (this._saveDisabled)
                    builder.AddAttribute(33, "Disabled");
                builder.AddAttribute(34, "class", "btn mr-1 btn-success");
                builder.AddContent(36, this._saveButtonText);
                builder.CloseComponent();
            }
            {
                if (this.IsDirty)
                {
                    builder.OpenElement(40, "button");
                    builder.AddAttribute(41, "type", "button");
                    if (this._saveDisabled)
                        builder.AddAttribute(43, "Disabled");
                    builder.AddAttribute(44, "class", "btn mr-1 btn-danger");
                    builder.AddAttribute(45, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.ConfirmExit));
                    builder.AddContent(46, "Exit Without Saving");
                    builder.CloseComponent();
                }
            }
            {
                if (!this.IsDirty)
                {
                    builder.OpenElement(50, "button");
                    builder.AddAttribute(51, "type", "button");
                    builder.AddAttribute(54, "class", "btn mr-1 btn-dark");
                    builder.AddAttribute(55, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.Exit));
                    builder.AddContent(56, "Exit");
                    builder.CloseComponent();
                }
            }
        };

        public void Dispose()
        {
            if (this.EditContext != null)
                this.EditContext.OnFieldChanged -= FieldChanged;
        }
    }
}
