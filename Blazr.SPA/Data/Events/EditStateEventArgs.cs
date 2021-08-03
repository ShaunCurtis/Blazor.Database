/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System;

namespace Blazr.SPA.Data.Events
{
    public class EditStateEventArgs : EventArgs
    {
        public bool IsDirty { get; set; }

        public static EditStateEventArgs NewArgs(bool dirtyState)
            => new EditStateEventArgs { IsDirty = dirtyState };
    }
}
