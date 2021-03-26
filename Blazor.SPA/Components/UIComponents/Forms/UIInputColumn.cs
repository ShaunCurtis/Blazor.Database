
using Microsoft.AspNetCore.Components;

namespace Blazor.SPA.Components
{
    public class UIInputColumn : UIColumn
    {
        [Parameter] public override int Cols { get; set; } = 6;

        [Parameter] public string FormCss { get; set; } = "form-control";

        public UIInputColumn()
        {
            this.SecondaryClass.Add(this.FormCss);
        }

    }
}
