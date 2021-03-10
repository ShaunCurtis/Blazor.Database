using Blazor.Database.Data;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Diagnostics;

namespace Blazor.Database.Components.Controls
{
    public class EditStateTracker : IDisposable
    {
        public bool IsDirty => EditFields?.IsDirty ?? false;

        public bool DoValidationOnFieldChange { get; set; } = false;

        public bool IsValid { get; private set; } = true;

        public bool DoValidation { get; set; } = true;

        private EditFieldCollection EditFields = new EditFieldCollection();
        private EditContext editContext;
        private ValidationMessageStore validationMessageStore;
        private bool validating = false;
        private bool disposedValue;

        public EditStateTracker(EditContext editContext)
        {
            Debug.Assert(editContext != null);

            if (editContext != null)
            {
                this.editContext = editContext;
                var model = this.editContext.Model;
                var props = model.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(model);
                    EditFields.AddField(model, prop.Name, value);
                }
                this.editContext.OnFieldChanged += FieldChanged;
                if (model is IValidation && this.DoValidation)
                {
                    // Get the Validation Message Store from the EditContext
                    this.validationMessageStore = new ValidationMessageStore(editContext);

                    this.editContext.OnValidationRequested += ValidationRequested;
                }
            }
        }

        private void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            var prop = e.FieldIdentifier.Model.GetType().GetProperty(e.FieldIdentifier.FieldName);
            if (prop != null)
            {
                var value = prop.GetValue(e.FieldIdentifier.Model);
                EditFields.SetField(e.FieldIdentifier.FieldName, value);
                if (DoValidationOnFieldChange) this.Validate(e.FieldIdentifier.FieldName);
            }
        }

        private void ValidationRequested(object sender, ValidationRequestedEventArgs e)
            => this.Validate();

        private void Validate(string fieldname = null)
        {
            var validator = this.editContext.Model as IValidation;
            if (validator != null || !this.validating)
            {
                this.validating = false;
                this.validationMessageStore.Clear();
                this.IsValid = validator.Validate(validationMessageStore, fieldname, this.editContext.Model);
                this.editContext.NotifyValidationStateChanged();
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
                    if (this.editContext != null)
                    {
                        this.editContext.OnFieldChanged -= this.FieldChanged;
                        this.editContext.OnValidationRequested -= this.ValidationRequested;
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
