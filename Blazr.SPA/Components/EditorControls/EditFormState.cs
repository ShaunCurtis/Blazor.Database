/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Data;
using Blazr.SPA.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazr.SPA.Components
{
    /// <summary>
    /// Component Class that adds Edit State and Validation State to a Blazor EditForm Control
    /// Should be placed within thr EditForm Control
    /// </summary>
    public class EditFormState : ComponentBase, IDisposable
    {
        private bool disposedValue;
        private EditFieldCollection EditFields = new EditFieldCollection();

        [CascadingParameter] public EditContext EditContext { get; set; }

        [Inject] private EditStateService EditStateService { get; set; }

        [Inject] private IJSRuntime _js { get; set; }

        protected override Task OnInitializedAsync()
        {
            Debug.Assert(this.EditContext != null);

            if (this.EditContext != null)
            {
                // Populates the EditField Collection
                this.LoadEditState();
                // Wires up to the EditContext OnFieldChanged event
                this.EditContext.OnFieldChanged += this.FieldChanged;
                this.EditStateService.RecordSaved += this.OnSave;
            }
            return Task.CompletedTask;
        }

        private void LoadEditState()
        {
            this.GetEditFields();
            if (EditStateService.IsDirty)
                SetEditState();
        }

        private void GetEditFields()
        {
            var model = this.EditContext.Model;
            this.EditFields.Clear();
            if (model is not null)
            {
                var props = model.GetType().GetProperties();
                foreach (var prop in props)
                {
                    if (prop.CanWrite)
                    {
                        var value = prop.GetValue(model);
                        EditFields.AddField(model, prop.Name, value);
                    }
                }
            }
        }

        private void SetEditState()
        {
            var recordtype = this.EditContext.Model.GetType();
            object data = JsonSerializer.Deserialize(EditStateService.Data, recordtype);
            if (data is not null)
            {
                var props = data.GetType().GetProperties();
                foreach (var property in props)
                {
                    var value = property.GetValue(data);
                    EditFields.SetField(property.Name, value);
                }
                this.SetModelToEditState();
                if (EditFields.IsDirty)
                    this.NotifyEditStateChanged();
            }
        }

        private void SetModelToEditState()
        {
            var model = this.EditContext.Model;
            var props = model.GetType().GetProperties();
            foreach (var property in props)
            {
                var value = EditFields.GetEditValue(property.Name);
                if (value is not null && property.CanWrite)
                    property.SetValue(model, value);
            }
        }

        private void FieldChanged(object sender, FieldChangedEventArgs e)
        {
            var isDirty = EditFields?.IsDirty ?? false;
            // Get the PropertyInfo object for the model property
            // Uses reflection to get property and value
            var prop = e.FieldIdentifier.Model.GetType().GetProperty(e.FieldIdentifier.FieldName);
            if (prop != null)
            {
                // Get the value for the property
                var value = prop.GetValue(e.FieldIdentifier.Model);
                // Sets the edit value in the EditField
                EditFields.SetField(e.FieldIdentifier.FieldName, value);
                // Invokes EditStateChanged if changed
                var stateChange = (EditFields?.IsDirty ?? false) != isDirty;
                isDirty = EditFields?.IsDirty ?? false;
                if (stateChange)
                    this.NotifyEditStateChanged();
                if (isDirty)
                    this.SaveEditState();
                else
                    this.ClearEditState();
            }
        }

        private void SaveEditState()
        {
            this.SetPageExitCheck(true);
            var jsonData = JsonSerializer.Serialize(this.EditContext.Model);
            EditStateService.SetEditState(jsonData);
        }

        private void ClearEditState()
        {
            this.SetPageExitCheck(false);
            EditStateService.ClearEditState();
        }

        private void SetPageExitCheck(bool action)
            => _js.InvokeAsync<bool>("cecblazor_setEditorExitCheck", action);

        private void OnSave(object sender, EventArgs e)
        {
            this.ClearEditState();
            this.LoadEditState();
        }

        private void NotifyEditStateChanged()
        {
            var isDirty = EditFields?.IsDirty ?? false;
            this.EditStateService.NotifyEditStateChanged(isDirty);

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
                this.EditStateService.RecordSaved -= this.OnSave;
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
