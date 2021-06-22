﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazor.SPA.Components
{
    public class UIValidationColumn : UIColumn
    {
        [Parameter] public override int Cols { get; set; } = 4;

        [Parameter] public string FormCss { get; set; } = "form-label";

        public UIValidationColumn()
        {
            this.CssClasses.Add(this.FormCss);
        }

    }
}
