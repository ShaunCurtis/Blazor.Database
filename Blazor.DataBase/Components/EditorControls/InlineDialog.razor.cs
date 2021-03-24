/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Blazor.Database.Components
{
    /// <summary>
    /// Blazor Component to control access to the rest of the render page
    /// Calling Lock() and Unlock() to lock out the rest of the page except the content within the control
    /// If the site js file is loaded on the page it will block exit by toobar, buttons, favourites, ... browser navigation
    /// </summary>
    public partial class InlineDialog : ComponentBase
    {

        /// <summary>
        /// Captured attributes that are added to the control
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Boolean to control if a Cacade for "this" is added to the control so all children have access to this control's public members
        /// </summary>
        [Parameter] public bool Cascade { get; set; } = true;

        /// <summary>
        /// Boolean to control if the background blocker is transparent or tranluscent (the default).
        /// </summary>
        [Parameter] public bool Transparent { get; set; } = true;

        /// <summary>
        /// The Child Content of the component
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool Lock {
            get => _lock;
            set 
            {
                    if (value && !_lock)
                    {
                        SetLock();
                        this._lock = true;
                    }
                    else if (!value && _lock)
                    {
                        SetUnlock();
                        this._lock = false;
                    }
            } 
        }

        private bool _lock = false;

        /// <summary>
        /// Read Only Boolean to check the lock state of the component
        /// </summary>
        public bool IsLocked => this._isLocked;

        [Inject] private IJSRuntime _js { get; set; }

        /// <summary>
        /// Gets the full CSS Class for the container using any provided class data concatenated onto the end of the calculated Css
        /// </summary>
        private string CssClass => (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out var obj))
            ? $"{this.frontcss} { Convert.ToString(obj, CultureInfo.InvariantCulture)}"
            : this.frontcss;

        // Set of private properties and fields to control the UI bits
        private string backcss = string.Empty;
        private string frontcss = string.Empty;

        private string _backcss => this.Transparent ? "back-block-transparent" : "back-block";
        private string _frontcss => this.Transparent ? "fore-block-transparent" : "fore-block";

        private string __backcss => string.Empty;
        private string __frontcss => string.Empty;

        private bool _isLocked;

        /// <summary>
        /// Method called to lock out the rest of the page
        /// </summary>
        public void SetLock()
        {
            this._isLocked = true;
            this.backcss = this._backcss;
            this.frontcss = this._frontcss;
            this.SetPageExitCheck(true);
            this.InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Method called to remove the lock on the rest of the page
        /// </summary>
        public void SetUnlock()
        {
            this._isLocked = false;
            this.backcss = this.__backcss;
            this.frontcss = this.__frontcss;
            this.SetPageExitCheck(false);
            this.InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Method to interact with the page js to enable/disable the "beforeunload" browser event
        /// </summary>
        /// <param name="action"></param>
        private void SetPageExitCheck(bool action)
            => _js.InvokeAsync<bool>("cecblazor_setEditorExitCheck", action);

    }
}
