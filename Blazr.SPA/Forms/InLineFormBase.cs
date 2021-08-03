/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazr.SPA.Forms
{
    /// <summary>
    /// Abstract class to implement the boilerplate code used in list forms
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class InlineFormBase : ComponentBase
    {
        [Inject] NavigationManager NavManager { get; set; }

        [Inject] EditStateService EditStateService { get; set; }

        protected Guid editorId = Guid.Empty;
        protected Guid viewerId = Guid.Empty;

        protected bool ShowViewer => this.viewerId != Guid.Empty;
        protected bool ShowEditor => this.editorId != Guid.Empty;

        protected override Task OnInitializedAsync()
        {
            if (EditStateService.IsDirty && EditStateService.RecordID is Guid)
                GoToEditor((Guid)EditStateService.RecordID);
            return base.OnInitializedAsync();
        }

        public void GoToEditor(Guid id)
            => SetIds(id, Guid.Empty);

        public void GoToNew()
            => SetIds(Guid.Empty, Guid.Empty);

        public void GoToViewer(Guid id)
            => SetIds(Guid.Empty, id);

        public void CloseDialog()
            => SetIds(Guid.Empty, Guid.Empty);

        public void Exit()
            => this.NavManager.NavigateTo("index");

        protected void SetIds(Guid editorId, Guid viewerId)
        {
            this.editorId = editorId;
            this.viewerId = viewerId;
        }

    }
}