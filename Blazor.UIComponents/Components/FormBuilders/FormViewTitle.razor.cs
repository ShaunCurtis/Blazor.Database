/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazor.UIComponents
{
    public partial class FormViewTitle
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string Title { get; set; } = "No Title Set";
    }
}
