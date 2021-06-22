/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Blazor.SPA.Components
{
    /// <summary>
    /// Component Class that adds Edit State and Validation State to a Blazor EditForm Control
    /// Should be placed within thr EditForm Control
    /// </summary>
    public class EditFormState : ComponentBase, IDisposable
    {

        [CascadingParameter] public EditContext EditContext { get; set; }

        [Parameter] public EventCallback<bool> EditStateChanged { get; set; }

        public bool IsDirty => EditFields?.IsDirty ?? false;

        private EditFieldCollection EditFields = new EditFieldCollection();
        private bool disposedValue;

        protected override Task OnInitializedAsync()
        {
            Debug.Assert(this.EditContext != null);

            if (this.EditContext != null)
            {
                // Populates the EditField Collection
                this.GetEditFields();
                // Wires up to the EditContext OnFieldChanged event
                this.EditContext.OnFieldChanged += FieldChanged;

            }
            return Task.CompletedTask;
        }

        protected void GetEditFields()
        {
            // Gets the model from the EditContext and populates the EditFieldCollection
            this.EditFields.Clear();
            var model = this.EditContext.Model;
            var props = model.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(model);
                EditFields.AddField(model, prop.Name, value);
            }
        }

        private void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            // Get the PropertyInfo object for the model property
            // Uses reflection to get property and value
            var prop = e.FieldIdentifier.Model.GetType().GetProperty(e.FieldIdentifier.FieldName);
            if (prop != null)
            {
                // Get the value for the property
                var value = prop.GetValue(e.FieldIdentifier.Model);
                // Sets the edit value in the EditField
                EditFields.SetField(e.FieldIdentifier.FieldName, value);
                // Invokes EditStateChanged
                this.EditStateChanged.InvokeAsync(EditFields?.IsDirty ?? false);
            }
        }

        public void UpdateState()
        {
            this.GetEditFields();
            this.EditStateChanged.InvokeAsync(EditFields?.IsDirty ?? false);
        }

        // IDisposable Implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.EditContext != null)
                        this.EditContext.OnFieldChanged -= this.FieldChanged;
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
