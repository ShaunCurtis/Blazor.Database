/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazor.SPA.Components
{
    public class UIButton : UIComponent
    {
        public UIButton()
        {
            this.CssClasses.Add("btn mr-1");
        }

        protected override string HtmlTag => "button";

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        protected override bool ShouldRender()
        {
            return base.ShouldRender();
        }
    }
}
