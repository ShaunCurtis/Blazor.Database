/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

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
    /// Should be placed within thr EditForm ontrol
    /// </summary>
    public class EditFormState : ComponentBase, IDisposable
    {

        /// <summary>
        /// EditContext - cascaded from EditForm
        /// </summary>
        [CascadingParameter] public EditContext EditContext { get; set; }

        /// <summary>
        /// EventCallback for parent to link into for Edit State Change Events
        /// passes the the current Dirty state
        /// </summary>
        [Parameter] public EventCallback<bool> EditStateChanged { get; set; }

        /// <summary>
        /// Property to expose the Edit/Dirty state of the control
        /// </summary>
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

        /// <summary>
        /// Method to populate the edit field collection
        /// </summary>
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

        /// <summary>
        /// Event Handler for Editcontext.OnFieldChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Method to Update the Edit State to current values 
        /// </summary>
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
