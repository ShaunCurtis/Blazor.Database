
using Microsoft.AspNetCore.Components;

namespace Blazor.SPA.Components
{
    public class UIValidationColumn : UIColumn
    {
        [Parameter] public override int Cols { get; set; } = 4;

        [Parameter] public string FormCss { get; set; } = "form-label";

        public UIValidationColumn()
        {
            this.SecondaryClass.Add(this.FormCss);
        }

    }
}
