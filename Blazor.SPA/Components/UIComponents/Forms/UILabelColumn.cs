using Microsoft.AspNetCore.Components;

namespace Blazor.SPA.Components
{
    public class UILabelColumn : UIColumn
    {
        [Parameter] public override int Cols { get; set; } = 2;

        [Parameter] public string FormCss { get; set; } = "form-label";

        public UILabelColumn()
        {
            this.SecondaryClass.Add(this.FormCss);
        }

    }
}
