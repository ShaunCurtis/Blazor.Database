/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.SPA.Data;
using Blazor.SPA.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Diagnostics;
using System.Text.Json;
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

        [Inject] private RouteViewService RouteViewService { get; set; }

        public bool IsDirty => EditFields?.IsDirty ?? false;

        private Guid FormId => RouteViewService.EditFormId;

        private EditFieldCollection EditFields = new EditFieldCollection();
        private bool disposedValue;

        protected async override Task OnInitializedAsync()
        {
            Debug.Assert(this.EditContext != null);

            if (this.EditContext != null)
            {
                // Populates the EditField Collection
                await this.GetEditStateValues();
                // Wires up to the EditContext OnFieldChanged event
                this.EditContext.OnFieldChanged += FieldChanged;

            }
        }

        protected async void GetEditFields(object model, object editedsource = null)
        {
            // Gets the fields from the model
            this.EditFields.Clear();
            if (model is not null)
            {
                var props = model.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(model);
                    EditFields.AddField(model, prop.Name, value);
                }
            }
            // Update the fields with the values from the source
            if (editedsource is not null)
            {
                var props = editedsource.GetType().GetProperties();
                foreach (var property in props)
                {
                    var value = property.GetValue(editedsource);
                    var prop = model.GetType().GetProperty(property.Name);
                    if (prop != null)
                    {
                        // Sets the edit value in the EditField
                        EditFields.SetField(property.Name, value);
                    }
                }
            }
            // Update the edit state if we're dirty
            if (EditFields.IsDirty)
                await this.EditStateChanged.InvokeAsync(true);
        }

        private async void FieldChanged(object sender, FieldChangedEventArgs e)
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
                    await this.EditStateChanged.InvokeAsync(isDirty);
                if (isDirty)
                    this.SaveEditState();
                else
                    this.ClearEditState();
            }
        }

        public void SetModelToEditState(object model)
        {
            var props = model.GetType().GetProperties();
            foreach (var property in props)
            {
                var value = EditFields.GetEditValue(property.Name);
                if (value is not null && property.CanWrite)
                property.SetValue(model, value);
            }
        }

        public void UpdateState()
        {
            this.GetEditFields(this.EditContext.Model);
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

        protected void SaveEditState()
        {
            if (!this.FormId.Equals(Guid.Empty))
            {
                var json = JsonSerializer.Serialize(this.EditContext.Model);
                var data = new EditStateData { Data = json, FormId = this.FormId };
                RouteViewService.AddEditState(data);
                RouteViewService.SetViewToEditForm();
            }
        }

        public void ClearEditState()
        {
            RouteViewService.ClearEditState(this.FormId);
            RouteViewService.EditorView = null;
            RouteViewService.EditFormId = Guid.Empty;
        }

        protected async ValueTask<bool> GetEditStateValues()
        {
            object data = null;
            var recordtype = this.EditContext.Model.GetType();
            var rec = RouteViewService.GetEditState(this.FormId);
            var hasRecord = rec != null;
            if (hasRecord)
                data = JsonSerializer.Deserialize(rec.Data, recordtype);

            this.GetEditFields(this.EditContext.Model, data);
            this.SetModelToEditState(this.EditContext.Model);
            if (EditFields.IsDirty)
                await this.EditStateChanged.InvokeAsync(true);

            return hasRecord;
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
