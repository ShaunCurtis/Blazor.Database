/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazr.UIComponents
{
    public partial class FormViewButtons
    {
        [Parameter] public EventCallback OnExit { get; set; }

    }
}
