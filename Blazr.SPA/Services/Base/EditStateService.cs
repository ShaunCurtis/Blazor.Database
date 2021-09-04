/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System;

namespace Blazr.SPA.Components
{
    /// <summary>
    /// Service Class for managing Form Edit State
    /// </summary>
    public class EditStateService
    {
        private bool _isDirty;

        public object RecordID { get; set; }

        public bool IsDirty => _isDirty && !string.IsNullOrWhiteSpace(this.Data) && !string.IsNullOrWhiteSpace(this.Data) && this.RecordID != null;

        public string Data { get; set; }

        public string EditFormUrl { get; set; }

        public bool ShowEditForm => (!String.IsNullOrWhiteSpace(EditFormUrl)) && IsDirty;

        public bool DoFormReload { get; set; }

        public event EventHandler RecordSaved;

        public event EventHandler<EditStateEventArgs> EditStateChanged;

        public void SetEditState(string data, string formUrl)
        {
            this.Data = data;
            this.EditFormUrl = formUrl;
            this._isDirty = true;
        }

        public void ClearEditState()
        {
            this.Data = null;
            this._isDirty = false;
            this.EditFormUrl = string.Empty;
        }

        public void ResetEditState()
        {
            this.RecordID = null;
            this.Data = null;
            this._isDirty = false;
            this.EditFormUrl = string.Empty;
        }

        public void NotifyRecordSaved()
        {
            RecordSaved?.Invoke(this, EventArgs.Empty);
            EditStateChanged?.Invoke(this, EditStateEventArgs.NewArgs(false));
        }

        public void NotifyRecordExit()
            => this.NotifyRecordSaved();

        public void NotifyEditStateChanged(bool dirtyState)
            => EditStateChanged?.Invoke(this, EditStateEventArgs.NewArgs(dirtyState));
    }
}
