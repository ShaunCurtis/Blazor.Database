/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazor.SPA.Components
{
    public class UIButtonColumn : UIColumn
    {
        public UIButtonColumn()
        {
            CssClasses.Add(this.Cols > 0 ? $"col-{this.Cols} text-right" : $"col text-right");
        }
    }
}
