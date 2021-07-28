/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazor.UIComponents
{
    public partial class FormEditRow
    {
        [Parameter] public string Title { get; set; }

        [Parameter] public string Value { get; set; } = "No Value Set";

        [Parameter] public int LabelColumns { get; set; } = 2;

        [Parameter] public int ControlColumns { get; set; } = 6;

        [Parameter] public int ValidationColumns { get; set; } = 4;

        private int paddingColumns => 12 - (this.LabelColumns + this.ControlColumns + (this.ValidationContent != null ? ValidationColumns : 0));

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment ValidationContent { get; set; }

    }
}
