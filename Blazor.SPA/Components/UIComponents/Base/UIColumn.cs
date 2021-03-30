/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;

namespace Blazor.SPA.Components
{
    public class UIColumn : UIBase
    {
        [Parameter] public virtual int Cols { get; set; } = 0;

        protected override string PrimaryClass => this.Cols > 0 ? $"col-{this.Cols}" : $"col";
    }
}
