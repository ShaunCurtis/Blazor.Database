using Blazor.Database.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public class EditState : ComponentBase, IDisposable
    {
        [Parameter] public bool DoValidationOnFieldChange { get; set; } = true;

        [CascadingParameter] public EditContext EditContext { get; set; }

        [Parameter] public bool DoValidation { get; set; } = true;

        [Parameter] public EventCallback<bool> EditStateChanged { get; set; }

        [Parameter] public EventCallback<bool> ValidStateChanged { get; set; }

        [Parameter] public bool Reset
        {
            get => false;
            set
            {
                if (value) this.Clear();
            }
        }

        public bool IsValid { get; private set; } = true;

        public bool IsDirty => EditFields?.IsDirty ?? false;

        private EditFieldCollection EditFields = new EditFieldCollection();
        private ValidationMessageStore validationMessageStore;
        private bool validating = false;
        private bool disposedValue;

        protected override Task OnInitializedAsync()
        {
            Debug.Assert(this.EditContext != null);

            if (this.EditContext != null)
            {
                var model = this.EditContext.Model;
                var props = model.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(model);
                    EditFields.AddField(model, prop.Name, value);
                }
                this.EditContext.OnFieldChanged += FieldChanged;
                if (model is IValidation && this.DoValidation)
                {
                    // Get the Validation Message Store from the EditContext
                    this.validationMessageStore = new ValidationMessageStore(this.EditContext);

                    this.EditContext.OnValidationRequested += ValidationRequested;
                }
            }
            return Task.CompletedTask;
        }

        private void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            var prop = e.FieldIdentifier.Model.GetType().GetProperty(e.FieldIdentifier.FieldName);
            if (prop != null)
            {
                var value = prop.GetValue(e.FieldIdentifier.Model);
                EditFields.SetField(e.FieldIdentifier.FieldName, value);
                if (DoValidationOnFieldChange) this.Validate(e.FieldIdentifier.FieldName);
                this.EditStateChanged.InvokeAsync(EditFields?.IsDirty ?? false);
            }
        }

        private void ValidationRequested(object sender, ValidationRequestedEventArgs e)
            => this.Validate();

        private void Validate(string fieldname = null)
        {
            var validator = this.EditContext.Model as IValidation;
            if (validator != null || !this.validating)
            {
                this.validating = false;
                this.validationMessageStore.Clear();
                this.IsValid = validator.Validate(validationMessageStore, fieldname, this.EditContext.Model);
                this.EditContext.NotifyValidationStateChanged();
                this.ValidStateChanged.InvokeAsync(this.IsValid);
                this.validating = false;
            }
        }

        public void Clear()
        {
            this.EditFields.ResetValues();
            this.validationMessageStore.Clear();
            this.IsValid = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.EditContext != null)
                    {
                        this.EditContext.OnFieldChanged -= this.FieldChanged;
                        this.EditContext.OnValidationRequested -= this.ValidationRequested;
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
