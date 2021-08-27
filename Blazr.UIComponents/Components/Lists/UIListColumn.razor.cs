/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Blazr.UIComponents
{
    public partial class UIListColumn : UIComponentBase
    {
        [CascadingParameter(Name = "IsHeader")] public bool IsHeader { get; set; }
        [Parameter] public bool IsMaxColumn { get; set; }
        [Parameter] public string HeaderTitle { get; set; }
        [Parameter] public bool IsHeaderNoWrap { get; set; }

        private bool isMaxRowColumn => IsMaxColumn && !this.IsHeader;
        private bool isNormalRowColumn => !IsMaxColumn && !this.IsHeader;
        protected override List<string> UnwantedAttributes { get; set; } = new List<string>() { "class" };

        private string HeaderCSS
            => CSSBuilder.Class()
                .AddClass("header-column-nowrap", "header-column", IsHeaderNoWrap)
                .AddClass("align-baseline")
                .Build();
    }
}
