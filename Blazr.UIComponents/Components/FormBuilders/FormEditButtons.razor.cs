/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
// ============================================================

using Blazr.SPA.Data;
using Microsoft.AspNetCore.Components;

namespace Blazr.UIComponents
{
    public partial class FormEditButtons
    {
        [Parameter] public AlertData AlertData { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public int AlertColumns { get; set; } = 5;

        [Parameter] public int ButtonColumns { get; set; } = 7;

        [Parameter] public BootstrapSize ContainerSize { get; set; } = BootstrapSize.Fluid;
    }
}
