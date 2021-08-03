/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazr.UIComponents
{
    public class UIInputColumn : UIColumn
    {
        [Parameter] public override int Columns { get; set; } = 6;

        [Parameter] public string FormCss { get; set; } = string.Empty;

        public UIInputColumn()
        {
            this.CssClasses.Add(this.FormCss);
        }

    }
}
