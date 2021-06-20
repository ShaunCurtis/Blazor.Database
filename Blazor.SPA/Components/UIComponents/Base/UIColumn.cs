/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazor.SPA.Components
{
    public class UIColumn : UIComponent
    {
        [Parameter] public virtual int Cols { get; set; } = 0;

        public UIColumn()
        {
            CssClasses.Add(this.Cols > 0 ? $"col-{this.Cols}" : $"col");
        }
    }
}
