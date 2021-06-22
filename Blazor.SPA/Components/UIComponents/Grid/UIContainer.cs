/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazor.SPA.Components
{
    public class UIContainer : UIComponent
    {
        public UIContainer()
        {
            CssClasses.Add("container - fluid");
        }
    }
}
