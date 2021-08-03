/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazr.UIComponents
{
    public partial class FormViewRow
    {
        [Parameter] public string Title { get; set; }

        [Parameter] public string Value { get; set; } = "No Value Set";

        [Parameter] public int LabelLargeColumns { get; set; } = 2;

        [Parameter] public int ControlLargeColumns { get; set; } = 6;

        [Parameter] public int LabelMediumColumns { get; set; } = 4;

        [Parameter] public int ControlMediumColumns { get; set; } = 8;

        [Parameter] public RenderFragment ChildContent { get; set; }

    }
}
