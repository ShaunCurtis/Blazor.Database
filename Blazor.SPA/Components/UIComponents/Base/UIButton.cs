/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazor.SPA.Components
{
    class UIButton : UIComponent
    {
        [Parameter] public string Type { get; set; } = "button";

        public UIButton()
        {
            CssClasses.Add("btn mr-1");
            this.UserAttributes.Add("type", Type);
        }
    }
}
